using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleScreen : MonoBehaviour
{
    private void Update()
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
