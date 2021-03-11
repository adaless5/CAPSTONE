﻿using System.Collections;
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
    public GameObject ControlScheme;
    public GameObject Player;
    public GameObject pauseFirst;
    public GameObject quitFirst;
    public GameObject defaultControlOption;
    public Animator OptionMenuAnimator;
    public Animator ControlSchemeAnimator;

    ControllerType _playerContType;

    public CanvasGroup _canvasGroup;
    public CanvasGroup _quitCanvasGroup;

    bool _isOptionsActive = false;
    Vector3 _menupos;
    void Awake()
    {
        _canvasGroup = gameObject.transform.GetChild(0).GetChild(0).GetComponent<CanvasGroup>();
        _menupos = OptionsMenu.transform.position;
    }

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
        if (_isOptionsActive)
        {
            DeactivateOptionsMenu();
        }
        else
        {
            OptionsMenu.transform.position = _menupos;
            Time.timeScale = 1f;
            GameIsPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            ALTPlayerController pc = Player.GetComponent<ALTPlayerController>();
            //pc.enabled = true;
            pc.m_ControllerState = ALTPlayerController.ControllerState.Play;
            PauseMenu.SetActive(false);
        }
    }

    public void Pause()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (GameIsPaused)
        {
            Unpause();
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(pauseFirst);
            Time.timeScale = 0f;
            enabled = true;
            PauseMenu.SetActive(true);
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
        Destroy(GameObject.Find("EventSystem"));
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
        Application.Quit();
        Debug.Log("Exit");
    }

    public void Quit()
    {
        _quitCanvasGroup.gameObject.SetActive(true);
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _quitCanvasGroup.interactable = true;
        _quitCanvasGroup.blocksRaycasts = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(quitFirst);
    }

    public void NotQuit()
    {
        _quitCanvasGroup.gameObject.SetActive(false);
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _quitCanvasGroup.interactable = false;
        _quitCanvasGroup.blocksRaycasts = false;
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
        _isOptionsActive = true;
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
        _isOptionsActive = false;
    }

    public void ActivateControlScheme()
    {
        ControlSchemeAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;       
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        ControlScheme.GetComponent<CanvasGroup>().interactable = true;
        ControlScheme.GetComponent<CanvasGroup>().blocksRaycasts = true;
        ControlSchemeAnimator.SetBool("IsActive", true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultControlOption);
    }

    public void DeactivateControlScheme()
    {       
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        ControlScheme.GetComponent<CanvasGroup>().interactable = false;
        ControlScheme.GetComponent<CanvasGroup>().blocksRaycasts = false;
        ControlSchemeAnimator.SetBool("IsActive", false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirst);
    }
}
