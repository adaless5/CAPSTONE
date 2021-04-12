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
    public AudioMixer audioMusicMixer;
    public AudioMixer audioFXMixer;
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


    public void EnableOptions()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSlider);
    }

    private void Update()
    {
        //if (Input.GetButtonDown("Pause"))
        //{
        //    _pauseMenu.Unpause();
        //    gameObject.SetActive(false);
        //}

        //if (_isFullScreen)
        //{
        //    FullScreenButton.Select();
        //}
        //else
        //{
        //    WindowedButton.Select();
        //}

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
        GameObject _sfVol = GameObject.Find("Sky and Fog Volume");
        if (_sfVol != null)
        {
            Volume volume = _sfVol.GetComponentInChildren<Volume>();
            if (volume != null)
            {
                ColorAdjustments color;
                volume.profile.TryGet<ColorAdjustments>(out color);

                if (color != null)
                {
                    color.postExposure.SetValue(new FloatParameter(amt));
                }
            }
        }
    }

    public void SetMusicVolume(Slider slider)
    {
        //Debug.Log(slider.value);
        audioMusicMixer.SetFloat("musicVol", Mathf.Log10(slider.value) * 20);

    }

    public void SetFXVolume(Slider slider)
    {
        audioFXMixer.SetFloat("FXVol", Mathf.Log10(slider.value) * 20);
        //Debug.Log(slider.value);
    }

    public void InitializeVolumeSliders(Slider musicSlider, Slider fxSlider)
    {
        float f1;
        audioMusicMixer.GetFloat("musicVol", out f1);
        Debug.Log(f1);
        musicSlider.value = Mathf.Pow(10, f1 / 20);

        float f2;
        audioFXMixer.GetFloat("FXVol", out f2);
        Debug.Log(f2);
        fxSlider.value = Mathf.Pow(10, f2 / 20);
    }

    public void SetFullScreen(bool isfull)
    {
        _isFullScreen = isfull;
        Screen.fullScreen = isfull;
    }

    public void SetXAxisInvert(bool bInvert)
    {
        ALTPlayerController.instance.SetXAxisInvert();
    }

    public void SetYAxisInvert(bool bInvert)
    {
        ALTPlayerController.instance.SetYAxisInvert();
    }
}
