using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PauseOptionsMenuUI : MonoBehaviour
{
    public GameObject firstSlider;
    public Slider slider;
    public AudioMixer audioMaster;
    public Button FullScreenButton;
    public Button WindowedButton;

    private PauseMenuUI _pauseMenu;
    bool _isFullScreen;
    void Awake()
    {
        if (FindObjectOfType<ALTPlayerController>())
            FindObjectOfType<ALTPlayerController>().m_LookSensitivity = slider.value;
        _pauseMenu = FindObjectOfType<PauseMenuUI>();
        _isFullScreen = Screen.fullScreen;
    }

    private void OnEnable()
    {
        Debug.Log("Options Menu");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSlider);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            _pauseMenu.Unpause();
            gameObject.SetActive(false);
        }

        if (_isFullScreen)
        {
            FullScreenButton.Select();
        }
        else
        {
            WindowedButton.Select();
        }
    }

    public void SetSensitivity(float amount)
    {
        if (FindObjectOfType<ALTPlayerController>())
        {
            ALTPlayerController pc = FindObjectOfType<ALTPlayerController>();

            pc.m_LookSensitivity = amount;
        }
    }

    public void SetVolume(float vol)
    {
        audioMaster.SetFloat("volume", vol);
    }

    public void SetFullScreen(bool isfull)
    {
        _isFullScreen = isfull;
        Screen.fullScreen = isfull;
    }
}
