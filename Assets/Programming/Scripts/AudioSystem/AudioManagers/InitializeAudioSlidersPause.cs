using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitializeAudioSlidersPause : MonoBehaviour
{
    public PauseOptionsMenuUI _menuUI;
    public Slider musicSlider;
    public Slider fxSlider;

    private void Awake()
    {
        _menuUI.InitializeVolumeSliders(musicSlider, fxSlider);
    }
}
