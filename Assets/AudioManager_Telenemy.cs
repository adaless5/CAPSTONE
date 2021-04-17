using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_Telenemy : AudioManager
{
    public AudioSource _audioManagerAttack;

    public AudioClip[] _attacks;

    public override void Initialize()
    {
    }

    public void TriggerAttack()
    {
        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _audioManagerAttack.pitch += randomPitchScale;

        //Play Sound
        int index = Random.Range(0, _attacks.Length);
        _audioManagerAttack.PlayOneShot(_attacks[index], randVolumeScale);

        //Reset initial pitch
        _audioManagerAttack.pitch = 1f;
    }

    
}
