using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenuUI : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenu;
    public GameObject OptionsMenu;
    public GameObject Player;
    public GameObject pauseFirst;

    ControllerType _playerContType;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (GameIsPaused)
            {
                    Unpause();
            }
            else
            {
                Pause();

            }
        }
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirst);
    }

    public void Unpause()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Player.GetComponent<ALTPlayerController>().enabled = true;
    }

    public void Pause()
    {
        enabled = true;
        PauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirst);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Player.GetComponent<ALTPlayerController>().enabled = false;
    }

    public void LoadMenu()
    {
        PauseMenu.SetActive(false);
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Player.GetComponent<ALTPlayerController>().enabled = true;
        SaveSystem.SaveRespawnInfo(Player.transform, Player.scene.name);
        Destroy(Player);
        SceneManager.LoadScene(0);
        //SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        //tigger save and exit
    }

    public void ExitGame()
    {
        //
        SaveSystem.SaveRespawnInfo(Player.transform, Player.scene.name);
        Debug.Log("Exit");
        Application.Quit();
    }

    public bool IsGamePaused()
    {
        return GameIsPaused;
    }


}
