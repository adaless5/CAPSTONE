using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_Drone : AudioManager
{
    public AudioSource _audioSourceShoot;

    public AudioClip[] _shootingSounds;

    public override void Initialize()
    {
    }
    public bool isPlaying()
    {
        return _audioSourceShoot.isPlaying;
    }

    public void TriggerShot()
    {
        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _audioSourceShoot.pitch += randomPitchScale;

        //Play Sound
        int index = Random.Range(0, _shootingSounds.Length);
        _audioSourceShoot.PlayOneShot(_shootingSounds[index], randVolumeScale);

        //Reset initial pitch
        _audioSourceShoot.pitch = 1f;
    }
}
