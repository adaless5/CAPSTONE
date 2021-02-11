using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    bool bToggleGui = false;

    string _userInput;

    public ALTPlayerController _player;

    public static DebugCommand ALL_WEAPONS;
    public static DebugCommand ALL_TOOLS;
    public List<DebugCommandBase> commandList;

    public void Awake()
    {
        ALL_WEAPONS = new DebugCommand("all_weapons", "Unlocks all weapons.", "all_weapons", () =>
        {
            ALTPlayerController.instance.UnlockAllWeapons();
        });
        ALL_TOOLS = new DebugCommand("all_tools", "Unlocks all tools.", "all_tools", () => 
        {
            ALTPlayerController.instance.UnlockAllTools();        
        });

        commandList = new List<DebugCommandBase>
        {
            ALL_WEAPONS,
            ALL_TOOLS
        };
    }

    public void Update()
    {
        if (Keyboard.current.rightCtrlKey.wasPressedThisFrame)
        {
            bToggleGui = !bToggleGui;
        }
        if (Keyboard.current.numpadEnterKey.wasPressedThisFrame)
        {
            if (bToggleGui)
            {
                ProcessInput();
                _userInput = "";
            }
        }
    }

    private void OnGUI()
    {
        if (!bToggleGui)
        {
            ALTPlayerController.instance.m_ControllerState = ALTPlayerController.ControllerState.Play;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return;
        }

        ALTPlayerController.instance.m_ControllerState = ALTPlayerController.ControllerState.Menu;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        float y = Screen.height / 2f;

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        _userInput = GUI.TextField(new Rect(0, y + 5f, Screen.width - 20f, 20f), _userInput);

        //print("This is the _userInput: " + _userInput);

    }

    private void ProcessInput()
    {
        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandbase = commandList[i] as DebugCommandBase;

            if (_userInput.Contains(commandbase.commandName))
            {
                if (commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
            }
        }
    }
}
