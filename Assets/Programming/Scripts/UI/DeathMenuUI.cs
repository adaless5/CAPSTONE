using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        _playerController.m_ControllerState = ALTPlayerController.ControllerState.Menu;
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
        _playerController.m_ControllerState = ALTPlayerController.ControllerState.Play;
        //_playerController.PlayerRespawn();
        StartCoroutine(FadeTo(0.0f, 1.5f));
        _deathMenuCanvas.interactable = false;
        _deathMenuCanvas.blocksRaycasts = false;

        //MainMenuUI..Continue()
        DestroyImmediate(GameObject.FindGameObjectWithTag("Player"));
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SaveSystem.RespawnInfo_Data data = new SaveSystem.RespawnInfo_Data();
        data.FromString(FileIO.FetchRespawnInfo());

        if (data.sceneName == null || data.sceneName == "")
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(data.sceneName);
            _player.transform.position = data.pos;
            _player.transform.rotation = data.rot;
        }
    }

    public void QuitToMainMenu()
    {
        _playerController.m_ControllerState = ALTPlayerController.ControllerState.Play;
        _deathMenuCanvas.interactable = false;
        _deathMenuCanvas.blocksRaycasts = false;
        _deathMenuCanvas.alpha = 0;
        _deathMenuCanvas.interactable = false;

        DestroyImmediate(GameObject.FindGameObjectWithTag("Player"));
        SceneManager.LoadScene(1);

    }
}
