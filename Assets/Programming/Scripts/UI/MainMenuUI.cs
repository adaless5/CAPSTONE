using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuUI : MonoBehaviour
{
    public GameObject firstMenuOption;
    public GameObject firstOptionsOption;
    public GameObject firstQuitOption;
    public GameObject firstControlOption;
    public GameObject firstCreditsOption;
    public GameObject _quitMenu;
    public GameObject _controlScheme;
    public Animator OptionMenuAnimator;
    public Animator ControlSchemeAnimator;
    public CanvasGroup _optionsGroup;
    public CanvasGroup _canvasGroup;
    public CanvasGroup _controlsGroup;
    public CanvasGroup _quitGroup;

    public static bool bNewGame = true;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Continue()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SaveSystem.RespawnInfo_Data data = new SaveSystem.RespawnInfo_Data();
        data.FromString(FileIO.FetchRespawnInfo());


        if (data.sceneName == "" || data.sceneName == null)

        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            //SceneManager.LoadScene(2);
            bNewGame = true;
        }
        else
        {
            SceneManager.LoadScene(data.sceneName);
            //SceneManager.LoadScene(2);
            bNewGame = false;
        }

        //get info from save system and load accordnaly
    }
    public void NewGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        FileIO.ClearAllSavedData();

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("Loading_Scene");
        bNewGame = true;
    }

    void Start()
    {
        OptionMenuAnimator.SetBool("OptionsActive", false);
        ControlSchemeAnimator.SetBool("IsActive", false);
        InitializeMenu();
        _canvasGroup = GetComponent<CanvasGroup>();
        _optionsGroup = FindObjectOfType<OptionsMenuUI>().GetComponent<CanvasGroup>();
        _optionsGroup.interactable = false;
        _optionsGroup.blocksRaycasts = false;
        _controlsGroup = _controlScheme.GetComponent<CanvasGroup>();
        _controlsGroup.interactable = false;
        _controlsGroup.blocksRaycasts = false;
        _quitGroup = _quitMenu.GetComponent<CanvasGroup>();
        _quitGroup.interactable = false;
        _quitGroup.blocksRaycasts = false;
        Time.timeScale = 1.0f;
    }


    public void InitializeMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        //EventSystem.current.SetSelectedGameObject(firstMenuOption);

    }
    public void ActivateOptionsMenu()
    {
        OptionMenuAnimator.SetBool("OptionsActive", true);
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstOptionsOption);
        _optionsGroup.interactable = true;
        _optionsGroup.blocksRaycasts = true;

    }
    public void DeactivateOptionsMenu()
    {
        OptionMenuAnimator.SetBool("OptionsActive", false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstMenuOption);
        _optionsGroup.interactable = false;
        _optionsGroup.blocksRaycasts = false;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void ActivateControlScheme()
    {
        ControlSchemeAnimator.SetBool("IsActive", true);
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstControlOption);
        _controlsGroup.interactable = true;
        _controlsGroup.blocksRaycasts = true;       
    }

    public void DeactivateControlScheme()
    {
        ControlSchemeAnimator.SetBool("IsActive", false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstMenuOption);
        _controlsGroup.interactable = false;
        _controlsGroup.blocksRaycasts = false;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void ActivateQuitMenu()
    {
        _quitMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstQuitOption);

        _quitGroup.interactable = true;
        _quitGroup.blocksRaycasts = true;


        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }


    public void DeactivateQuitMenu()
    {
        _quitMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstMenuOption);

        _quitGroup.interactable = false;
        _quitGroup.blocksRaycasts = false;

        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void ExitGame()
    {
        //Debug.Log("exit");
        Application.Quit();
    }

    void Update()
    {
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
        EventSystem.current.SetSelectedGameObject(firstMenuOption);
    }

    public void ChangeCanvas(bool active)
    {
        _canvasGroup.interactable = active;
        _canvasGroup.blocksRaycasts = active;
    }  

    public void LoadCredits()
    {
        SceneManager.LoadScene("R4_CreditsScene");
    }
    //TODO:: make a new game function that wipes the save and starts from the begining 
}
