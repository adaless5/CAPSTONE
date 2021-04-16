using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager_Squash : AudioManager
{
    public AudioSource _AudioSource;
    public AudioClip Squash1;
    public AudioClip Squash2;
    public AudioClip Squash3;
    public AudioClip Squash4;
    public AudioClip Squash5;
    public AudioClip Squash6;

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
        int randomInt = Random.Range(1, 7);

        switch(randomInt)
        {
            case 1: _AudioSource.PlayOneShot(Squash1, randVolumeScale); break;
            case 2: _AudioSource.PlayOneShot(Squash2, randVolumeScale); break;
            case 3: _AudioSource.PlayOneShot(Squash3, randVolumeScale); break;
            case 4: _AudioSource.PlayOneShot(Squash4, randVolumeScale); break;
            case 5: _AudioSource.PlayOneShot(Squash5, randVolumeScale); break;
            case 6: _AudioSource.PlayOneShot(Squash6, randVolumeScale); break;

        }
        

        //Reset initial pitch
        _AudioSource.pitch = 1.0f;
    }
}
