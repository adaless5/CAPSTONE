using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager_Universal : AudioManager
{
    public AudioSource _AudioSource;

    public override void Initialize()
    {
    }

    public void Play()
    {

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _AudioSource.pitch += randomPitchScale;

        //Play Sound
        _AudioSource.PlayOneShot(_AudioSource.clip,randVolumeScale);

        //Reset initial pitch
        _AudioSource.pitch = 1.0f;
    }
}
