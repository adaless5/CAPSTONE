﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuUI : MonoBehaviour
{
    public GameObject firstOption;
    public Animator OptionMenuAnimator;
    public CanvasGroup _canvasGroup;

    public void StartGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SaveSystem.RespawnInfo_Data data = SaveSystem.FetchRespawnInfo();



        if (data.sceneName == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        //else
        //{
        //    SceneManager.LoadScene(data.sceneName, LoadSceneMode.Additive);
        //}

        //get info from save system and load accordnaly
    }

    void Start()
    {
        InitializeMenu();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void InitializeMenu()
    {
        EventSystem.current.SetSelectedGameObject(firstOption);

    }
    public void ActivateOptionsMenu()
    {
        OptionMenuAnimator.SetBool("OptionsActive", true);
    }
    public void DeactivateOptionsMenu()
    {
        OptionMenuAnimator.SetBool("OptionsActive", false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstOption);
    }

    public void ExitGame()
    {
        //Debug.Log("exit");
        Application.Quit();
    }

    void Update()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject);
    }

    public void Quit(GameObject firstSelected)
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void UnQuit()
    {
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstOption);
    }

    public void ChangeCanvas(bool active)
    {
        _canvasGroup.interactable = active;
        _canvasGroup.blocksRaycasts = active;
    }

    //TODO:: make a new game function that wipes the save and starts from the begining 
}
