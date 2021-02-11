using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    bool bToggleGui = false;
    bool bCanShow = true;

    string _userInput;

    public ALTPlayerController _player;

    public static DebugCommand ALL_WEAPONS;
    public static DebugCommand ALL_TOOLS;
    public static DebugCommand ADD_MONEY;
    public List<DebugCommandBase> commandList;

    public void Awake()
    {
        ALL_WEAPONS = new DebugCommand("all_weapons", "Unlocks all weapons.", "all_weapons", () =>
        {
            ALTPlayerController.instance.DebugUnlockAllWeapons();
        });

        ALL_TOOLS = new DebugCommand("all_tools", "Unlocks all tools.", "all_tools", () => 
        {
            ALTPlayerController.instance.DebugUnlockAllTools();        
        });

        ADD_MONEY = new DebugCommand("add_money", "Adds +10 currency.", "add_money", () => 
        {
            ALTPlayerController.instance.DebugGainCurrency();
        });

        commandList = new List<DebugCommandBase>
        {
            ALL_WEAPONS,
            ALL_TOOLS, 
            ADD_MONEY
        };
    }

    public void Update()
    {
        if (bCanShow)
        {
            if (Keyboard.current.rightCtrlKey.wasPressedThisFrame)
            {
                bToggleGui = !bToggleGui;
            }
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                if (bToggleGui)
                {
                    ProcessInput();
                    _userInput = "";
                    bToggleGui = false;
                }
            }
        }

        if (ALTPlayerController.instance.m_ControllerState == ALTPlayerController.ControllerState.Menu || ALTPlayerController.instance.m_ControllerState == ALTPlayerController.ControllerState.Wheel)
        {
            bCanShow = false;
        }
        else if (ALTPlayerController.instance.m_ControllerState == ALTPlayerController.ControllerState.Play || ALTPlayerController.instance.m_ControllerState == ALTPlayerController.ControllerState.Debug)
        {
            bCanShow = true;
        }
    }

    private void OnGUI()
    {
        if (bCanShow)
        {
            if (!bToggleGui)
            {
                ALTPlayerController.instance.m_ControllerState = ALTPlayerController.ControllerState.Play;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                return;
            }

            ALTPlayerController.instance.m_ControllerState = ALTPlayerController.ControllerState.Debug;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            float y = Screen.height / 2f;

            GUIStyle style = new GUIStyle();
            style.fontSize = 20;
            style.normal.textColor = Color.white;

            GUI.Box(new Rect(0, y, Screen.width, 40), "");
            GUI.backgroundColor = Color.red;
            _userInput = GUI.TextField(new Rect(0, y + 5f, Screen.width - 20f, 100f), _userInput, style);

        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
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
