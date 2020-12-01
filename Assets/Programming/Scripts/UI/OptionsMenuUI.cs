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
        Screen.fullScreen = isfull;
    }

    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetOptions()
    {
        Debug.Log("Options");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstOption);
    }

    
}
