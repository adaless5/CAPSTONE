using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class DeathMenuUI : MonoBehaviour
{

    public ALTPlayerController _playerController;
    public GameObject _player;
    CanvasGroup _deathMenuCanvas;

    private void Awake()
    {
        _deathMenuCanvas = transform.GetChild(0).GetComponent<CanvasGroup>();
        _playerController = GetComponentInParent<ALTPlayerController>();
        _player = GetComponentInParent<Transform>().gameObject;
        EventBroker.OnPlayerDeath += DisplayDeathMenu;
        _deathMenuCanvas.interactable = false;
        _deathMenuCanvas.blocksRaycasts = false;
        _deathMenuCanvas.alpha = 0;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void DisplayDeathMenu()
    {
        try
        {
            _deathMenuCanvas.interactable = true;
            _deathMenuCanvas.blocksRaycasts = true;
            StartCoroutine(FadeTo(1.0f, 1.0f));
        }
        catch
        {
        }
    }

    void HideDeathMenu()
    {
        try
        {
            _deathMenuCanvas.interactable = false;
            _deathMenuCanvas.blocksRaycasts = false;
            _deathMenuCanvas.alpha = 0;
        }
        catch
        {

        }
    }
    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = _deathMenuCanvas.alpha;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            float newAlpha = Mathf.Lerp(alpha, aValue, t);
            _deathMenuCanvas.alpha = newAlpha;
            yield return null;
        }
        _deathMenuCanvas.alpha = Mathf.Round(_deathMenuCanvas.alpha);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadSave()
    {
        _deathMenuCanvas.interactable = false;
        _deathMenuCanvas.blocksRaycasts = false;

        //MainMenuUI..Continue()
        //Destroy(GameObject.FindGameObjectWithTag("Player"));
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        MainMenuUI.bNewGame = false;
        HideDeathMenu();
        for (int i = 1; i < SceneManager.sceneCount; i++)
        {
  
            if (SceneManager.GetSceneAt(i).name != SceneManager.GetSceneByBuildIndex(3).name)
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).name);
            }
        }
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        
        //SaveSystem.RespawnInfo_Data data = new SaveSystem.RespawnInfo_Data();
        //data.FromString(FileIO.FetchRespawnInfo());

        //if (data.sceneName == null || data.sceneName == "")
        //{

        //    //SceneManager.LoadSceneAsync(4, LoadSceneMode.Additive);
        //    StartCoroutine(LoadAsyncScene(4));
        //}
        //else
        //{

        //    //SceneManager.LoadSceneAsync(data.sceneName, LoadSceneMode.Additive);
        //    StartCoroutine(LoadAsyncScene(data.sceneName));



        //}
        //_playerController.PlayerRespawn();
        //StartCoroutine(FadeTo(0.0f, 1.0f));
    }


    IEnumerator LoadAsyncScene(int sceneIndex)
    {
        AsyncOperation firstLevel = SceneManager.LoadSceneAsync(3);
        AsyncOperation secondlevel = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);


        while (firstLevel.progress < 1f)
        {
            yield return new WaitForEndOfFrame();
        }

        while (secondlevel.progress < 1f)
        {
            yield return new WaitForEndOfFrame();
        }

    }
    IEnumerator LoadAsyncScene(string scenename)
    {
        AsyncOperation firstLevel = SceneManager.LoadSceneAsync(3);
        AsyncOperation secondlevel = SceneManager.LoadSceneAsync(scenename, LoadSceneMode.Additive);


        while (firstLevel.progress < 1f)
        {
            yield return new WaitForEndOfFrame();
        }

        while (secondlevel.progress < 1f)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    public void QuitToMainMenu()
    {
        _deathMenuCanvas.interactable = false;
        _deathMenuCanvas.blocksRaycasts = false;


        Destroy(_playerController.gameObject);
        SceneManager.LoadScene(1);
        //_deathMenuCanvas.alpha = 0;

    }
}
