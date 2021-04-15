using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System.Linq;

public class OptionsMenuUI : MonoBehaviour
{
    bool bDebug = false;

    public GameObject firstOption;
    public AudioMixer audioMusicMixer;
    public AudioMixer audioFXMixer;
    public TMP_Dropdown resolutionMenu;

    public Button StereoButton;
    public Button MonoButton;

    public Button FullScreenButton;
    public Button WindowedButton;

    bool _isStereo;
    bool _isFullScreen;
    Resolution[] resolutions;
    List<Resolution> validResolutions;

    private void Start()
    {
        validResolutions = new List<Resolution>();
        resolutions = Screen.resolutions;
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        resolutionMenu.ClearOptions();
        string[] validRes = { "1024 x 576", "1152 x 648", "1280 x 720", "1280 x 800", "1366 x 768", "1600 x 900", "1920 x 1080", "1920 x 1200", "1440 x 900", "2560 x 1440", "2560 x 1600", "3840 x 2160" };

        List<string> data = new List<string>();
        int index = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resData = resolutions[i].width.ToString() + " x " + resolutions[i].height.ToString();
            
            foreach (string obj in validRes)
            {
                if (obj == resData)
                {
                    validResolutions.Add(resolutions[i]);
                    data.Add(resData);
                    break;
                }
            }
            if (resolutions[i].width == Screen.currentResolution.width)
            {
                if (resolutions[i].height == Screen.currentResolution.height)
                {
                    index = i;
                }
            }

            //foreach (string obj in validRes)
            //{
            //    if (obj == resData)
            //    {
            //        data.Add(resData);
            //    }
            //}


        }

        resolutionMenu.AddOptions(data);
        resolutionMenu.value = index;
        resolutionMenu.RefreshShownValue();

        _isFullScreen = Screen.fullScreen;
        _isStereo = true;

        Screen.SetResolution(1920, 1080, true);

        if (bDebug) Debug.Log(resolutions[index]);


    }

    private void Update()
    {
        //if(_isStereo)
        //{
        //    StereoButton.Select();
        //}
        //else
        //{
        //    MonoButton.Select();
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
        musicSlider.value = Mathf.Pow(10, f1 / 20);

        float f2;
        audioFXMixer.GetFloat("FXVol", out f2);
        fxSlider.value = Mathf.Pow(10, f2 / 20);
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        Debug.Log(index.ToString());
    }

    public void SetFullScreen(bool isfull)
    {
        _isFullScreen = isfull;
        Screen.fullScreen = isfull;
    }

    public void SetAudioChannel(bool isStereo)
    {
        _isStereo = isStereo;
    }

    public void SetResolution(int index)
    {
        Resolution resolution = validResolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
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

        UI_Brightness[] _allUI = FindObjectsOfType<UI_Brightness>();

        float percentage = (amt + 2) / 4;
        float percentpercent = (percentage * 0.5f) + 0.5f;

        Color newUIColor = new Color(percentpercent, percentpercent, percentpercent, 1.0f);

        foreach (UI_Brightness ui in _allUI)
        {
            ui.SetBrightness(newUIColor);
        }
    }
    public void SetOptions()
    {
        Debug.Log("Options");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstOption);
    }

}
