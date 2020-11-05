using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void StartGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        //get info from save system and load accordnaly
    }

    public void ExitGame()
    {
        Debug.Log("exit");
        Application.Quit();
    }
}
