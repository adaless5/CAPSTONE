using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseOptionsMenuUI : MonoBehaviour
{
    public Slider slider;
    public AudioMixer audioMaster;
    void Awake()
    {
        if(FindObjectOfType<ALTPlayerController>())
        FindObjectOfType<ALTPlayerController>().m_LookSensitivity = slider.value;

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
