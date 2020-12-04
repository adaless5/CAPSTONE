using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class OptionsMenuUI : MonoBehaviour
{
    public GameObject firstOption;
    public AudioMixer audioMaster;
    public TMP_Dropdown resolutionMenu;

    public Button StereoButton;
    public Button MonoButton;

    public Button FullScreenButton;
    public Button WindowedButton;

    bool _isStereo;
    bool _isFullScreen;
    Resolution[] resolutions;
    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionMenu.ClearOptions();

        List<string> data = new List<string>();
        int index = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resData = resolutions[i].width + " x " + resolutions[i].height;
            data.Add(resData);

            if (resolutions[i].width == Screen.currentResolution.width)
            {
                if (resolutions[i].height == Screen.currentResolution.height)
                {
                    index = i;
                }
            }
        }

        resolutionMenu.AddOptions(data);
        resolutionMenu.value = index;
        resolutionMenu.RefreshShownValue();

        _isFullScreen = Screen.fullScreen;
        _isStereo = true;

        Debug.Log(resolutions[index]);
    }

    private void Update()
    {
        if(_isStereo)
        {
            StereoButton.Select();
        }
        else
        {
            MonoButton.Select();
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

    public void SetVolume(float vol)
    {
        audioMaster.SetFloat("volume", vol);
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
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetBrightness(float amt)
    {
        RenderSettings.ambientLight = new Color(amt, amt, amt, 1);
        Debug.Log(RenderSettings.ambientLight);
    }
    public void SetOptions()
    {
        Debug.Log("Options");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstOption);
    }


}
