using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathMenuUI : MonoBehaviour
{

    public ALTPlayerController _player;
    CanvasGroup _deathMenuCanvas;

    private void Awake()
    {
        _deathMenuCanvas = transform.GetChild(0).GetComponent<CanvasGroup>();
        _player = GetComponentInParent<ALTPlayerController>();
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
        _deathMenuCanvas.interactable = true;
        _deathMenuCanvas.blocksRaycasts = true;
        StartCoroutine(FadeTo(1.0f, 1.0f));
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
        _player.PlayerRespawn();
        StartCoroutine(FadeTo(0.0f, 1.5f));
        _deathMenuCanvas.interactable = false;
        _deathMenuCanvas.blocksRaycasts = false;
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(0);
        //_deathMenuCanvas.alpha = 0;
        _deathMenuCanvas.interactable = false;
        _deathMenuCanvas.blocksRaycasts = false;
    }
}
