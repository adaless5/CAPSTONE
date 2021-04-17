using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class AutoSaveMenu : MonoBehaviour
{
    private void Update()
    {

        if (Gamepad.current != null)
        {
            if (Gamepad.current.IsPressed())
            {
                SceneManager.LoadScene("OpeningCutscene", LoadSceneMode.Single);
            }
        }
        if (Keyboard.current != null)
        {
            if (Keyboard.current.anyKey.isPressed)
            {
                SceneManager.LoadScene("OpeningCutscene", LoadSceneMode.Single);
            }
        }
        if (Mouse.current != null)
        {
            if (Mouse.current.rightButton.isPressed || Mouse.current.leftButton.isPressed)
            {
                SceneManager.LoadScene("OpeningCutscene", LoadSceneMode.Single);
            }
        }
    }
}
