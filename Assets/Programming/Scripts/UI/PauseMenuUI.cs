using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    static bool GameIsPaused = false;
    public GameObject PauseMenu;
    public GameObject OptionsMenu;
    public GameObject Player;
    public GameObject pauseFirst;
    public Animator OptionMenuAnimator;

    ControllerType _playerContType;

    public CanvasGroup _canvasGroup;

    void Awake()
    {
        _canvasGroup = gameObject.transform.GetChild(0).GetChild(0).GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        //EventSystem.current.SetSelectedGameObject(null);
        //EventSystem.current.SetSelectedGameObject(pauseFirst);
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ALTPlayerController pc = Player.GetComponent<ALTPlayerController>();
        //pc.enabled = true;
        pc.m_ControllerState = ALTPlayerController.ControllerState.Play;
        PauseMenu.SetActive(false);
    }

    public void Pause()
    {
        if (GameIsPaused)
        {
            Unpause();
        }
        else
        {
            enabled = true;
            PauseMenu.SetActive(true);
            Time.timeScale = 0f;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseFirst);

            GameIsPaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            ALTPlayerController pc = Player.GetComponent<ALTPlayerController>();
            //pc.enabled = false;
            pc.m_ControllerState = ALTPlayerController.ControllerState.Menu;

        }
    }

    public void LoadMenu()
    {
        PauseMenu.SetActive(false);
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Player.GetComponent<ALTPlayerController>().enabled = true;
        Destroy(Player);
        //SceneManager.LoadScene(0);
        SceneManager.LoadScene("MainMenu");
        //tigger save and exit
    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    public void ExitGame()
    {
        SaveSystem.SaveRespawnInfo(Player.transform, Player.scene.name);
        Debug.Log("Exit");
        Application.Quit();
    }

    public void Quit(GameObject firstQuitButton)
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstQuitButton);
    }

    public void NotQuit()
    {
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirst);
    }

    public bool IsGamePaused()
    {
        return GameIsPaused;
    }

    public void ActivateOptionsMenu()
    {
        OptionMenuAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        Debug.Log("ActivateOptions");
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        OptionsMenu.GetComponent<CanvasGroup>().interactable = true;
        OptionsMenu.GetComponent<CanvasGroup>().blocksRaycasts = true;
        OptionMenuAnimator.SetBool("PauseOptionsActive", true);
    }
    public void DeactivateOptionsMenu()
    {
        Debug.Log("DeactivateOptions");
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        OptionsMenu.GetComponent<CanvasGroup>().interactable = false;
        OptionsMenu.GetComponent<CanvasGroup>().blocksRaycasts = false;
        OptionMenuAnimator.SetBool("PauseOptionsActive", false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirst);
    }

}
