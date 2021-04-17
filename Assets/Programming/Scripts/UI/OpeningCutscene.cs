using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpeningCutscene : MonoBehaviour
{
    bool canStart = false;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);
        canStart = true;
    }
    private void Update()
    {
        if (canStart)
        {
            if (Gamepad.current != null)
            {
                if (Gamepad.current.IsPressed())
                {
                    SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
                }
            }
            if (Keyboard.current != null)
            {
                if (Keyboard.current.anyKey.isPressed)
                {
                    SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
                }
            }
            if (Mouse.current != null)
            {
                if (Mouse.current.rightButton.isPressed || Mouse.current.leftButton.isPressed)
                {
                    SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
                }
            }
        }
    }
}
