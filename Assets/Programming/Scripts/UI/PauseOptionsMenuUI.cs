using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PauseOptionsMenuUI : MonoBehaviour
{
    public Slider slider;
    public AudioMixer audioMaster;
    private PauseMenuUI _pauseMenu;
    void Awake()
    {
        if (FindObjectOfType<ALTPlayerController>())
            FindObjectOfType<ALTPlayerController>().m_LookSensitivity = slider.value;
        _pauseMenu = FindObjectOfType<PauseMenuUI>();
    }

    private void OnEnable()
    {
        Debug.Log("Options Menu");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(FindObjectOfType<Slider>().gameObject);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            _pauseMenu.Unpause();
            gameObject.SetActive(false);
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

}
