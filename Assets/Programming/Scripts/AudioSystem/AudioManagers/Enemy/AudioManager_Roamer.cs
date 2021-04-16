using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_Roamer : AudioManager
{
    public AudioSource _audioSourceAttack;
    public AudioSource _audioSourceDie;
    public AudioSource _audioSourceHit;
    public AudioSource _audioSourceStep;

    //Attack Sounds
    public AudioClip[] _bladeSwings;

    //Die Sounds
    public AudioClip[] _dieSounds;

    //Step Sounds
    public AudioClip[] _stepSounds;

    //Hit Sounds
    public AudioClip[] _hitSounds;


    public override void Initialize()
    {
    }

    public void TriggerStep()
    {
        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _audioSourceStep.pitch += randomPitchScale;

        //Play Sound
        int index = Random.Range(0, _stepSounds.Length);
        _audioSourceStep.PlayOneShot(_stepSounds[index], randVolumeScale);

        //Reset initial pitch
        _audioSourceStep.pitch = 1f;
    }

    public void TriggerHit()
    {
        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _audioSourceHit.pitch += randomPitchScale;

        //Play Sound
        int index = Random.Range(0, _hitSounds.Length);
        _audioSourceHit.PlayOneShot(_hitSounds[index], randVolumeScale);

        //Reset initial pitch
        _audioSourceHit.pitch = 1f;
    }

    public void TriggerAttack()
    {
        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _audioSourceAttack.pitch += randomPitchScale;

        //Play Sound
        int index = Random.Range(0, _bladeSwings.Length);
        _audioSourceAttack.PlayOneShot(_bladeSwings[index], randVolumeScale);

        //Reset initial pitch
        _audioSourceAttack.pitch = .8f;
    }

    public void TriggerDeath()
    {

    }
}
