using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_Leaper : AudioManager
{
    public AudioSource _audioSourceLeap;
    public AudioSource _audioSourceLand;

    public AudioClip[] _leapSounds;
    public AudioClip[] _landSounds;

    public override void Initialize()
    {
    }

    public void TriggerLeap()
    {
        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _audioSourceLeap.pitch += randomPitchScale;

        //Play Sound
        int index = Random.Range(0, _leapSounds.Length);
        _audioSourceLeap.PlayOneShot(_leapSounds[index], randVolumeScale);

        //Reset initial pitch
        _audioSourceLeap.pitch = 1f;
    }

    public void TriggerLand()
    {
        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _audioSourceLand.pitch += randomPitchScale;

        //Play Sound
        int index = Random.Range(0, _landSounds.Length);
        _audioSourceLand.PlayOneShot(_landSounds[index], randVolumeScale);

        //Reset initial pitch
        _audioSourceLand.pitch = 1f;
    }
}
