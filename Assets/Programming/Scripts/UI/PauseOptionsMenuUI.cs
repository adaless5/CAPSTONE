using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PauseOptionsMenuUI : MonoBehaviour
{
    bool bDebug = false;

    public GameObject firstSlider;
    public Slider slider;
    public AudioMixer audioMaster;
    public Button FullScreenButton;
    public Button WindowedButton;

    //Volume m_Volume;
    //Exposure m_Exposure;
    //Light _light;

    private PauseMenuUI _pauseMenu;
   // private 
    bool _isFullScreen;
    void Awake()
    {
        if (FindObjectOfType<ALTPlayerController>())
            FindObjectOfType<ALTPlayerController>().m_LookSensitivity = slider.value;
        _pauseMenu = FindObjectOfType<PauseMenuUI>();
        _pauseMenu = FindObjectOfType<PauseMenuUI>();
        _isFullScreen = Screen.fullScreen;

        //_light = FindObjectOfType<Light>();
        //m_Volume = FindObjectOfType<Volume>();


        //VolumeProfile profile = m_Volume.sharedProfile;

        //if (!profile.TryGet<Exposure>(out var expose))
        //{
        //    expose = profile.Add<Exposure>(false);
        //    m_Exposure = expose;
        //}
        //else
        //{
        //    m_Exposure = expose;
        //}
        //m_Exposure.mode = new ExposureModeParameter(ExposureMode.Automatic, true);
        //expose.compensation = new FloatParameter(7f);
    }

    private void OnEnable()
    {
        try
        {
            if (bDebug) Debug.Log("Options Menu");
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSlider);
        }catch { }
        
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
    public void SetBrightness(float amt)
    {
        //m_Exposure.compensation = new FloatParameter(amt);
        //_light.intensity = amt;
        //RenderSettings.skybox.SetFloat("_Exposure", amt);
        //RenderSettings.ambientIntensity = amt;
        //RenderSettings.ambientLight = new Color(amt, amt, amt, 1);
        //Debug.Log(RenderSettings.ambientLight);
    }
    public void SetVolume(float vol)
    {
        //audioMaster.SetFloat("volume", vol);
    }

    public void SetFullScreen(bool isfull)
    {
        _isFullScreen = isfull;
        Screen.fullScreen = isfull;
    }
}
