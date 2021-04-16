using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager_Goggles : AudioManager
{
    public AudioSource _AudioSource;
    public AudioClip _gogglesOn;
    public AudioClip _gogglesOff;

    public enum ThermalStates
    {
        on,
        off
    }
    public ThermalStates _thermalState = ThermalStates.off;

    public override void Initialize()
    {
    }

    public void Play(bool bTurnOn)
    {

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _AudioSource.pitch += randomPitchScale;

        //Play Sound
        if (bTurnOn) { _thermalState = ThermalStates.on; _AudioSource.Stop(); _AudioSource.PlayOneShot(_gogglesOn, randVolumeScale); }
        else { _thermalState = ThermalStates.off; _AudioSource.Stop(); _AudioSource.PlayOneShot(_gogglesOff, randVolumeScale); }

        //Reset initial pitch
        _AudioSource.pitch = 1.0f;
    }
}

