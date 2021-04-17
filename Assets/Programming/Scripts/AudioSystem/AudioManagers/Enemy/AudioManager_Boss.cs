using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_Boss : MonoBehaviour
{
    public AudioSource _audioSourceUmbilical;
    public AudioSource _audioSourceDeath;
    public AudioSource _audioSourceStun;
    public AudioSource _audioSourceAttack1;
    public AudioSource _audioSourceAttack2;

    public AudioClip[] _UmbilicalSounds;
    public AudioClip _deathSounds;
    public AudioClip _stunSounds;
    public AudioClip _attack1Sound;
    public AudioClip _attack2Sound;


    public void TriggerUmbilical()
    {
        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _audioSourceUmbilical.pitch += randomPitchScale;

        //Play Sound
        int index = Random.Range(0, _UmbilicalSounds.Length);
        _audioSourceUmbilical.PlayOneShot(_UmbilicalSounds[index], randVolumeScale);

        //Reset initial pitch
        _audioSourceUmbilical.pitch = 1f;
    }

    public void TriggerDeath()
    {
        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _audioSourceDeath.pitch += randomPitchScale;

        //Play Sound
        _audioSourceDeath.PlayOneShot(_deathSounds, randVolumeScale);

        //Reset initial pitch
        _audioSourceDeath.pitch = 1f;
    }

    public void TriggerStun()
    {
        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _audioSourceStun.pitch += randomPitchScale;

        //Play Sound
        _audioSourceStun.PlayOneShot(_stunSounds, randVolumeScale);

        //Reset initial pitch
        _audioSourceStun.pitch = 1f;
    }

    public void TriggerAttack1()
    {
        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _audioSourceAttack1.pitch += randomPitchScale;

        //Play Sound
        _audioSourceAttack1.PlayOneShot(_attack1Sound, randVolumeScale);

        //Reset initial pitch
        _audioSourceAttack1.pitch = 1f;
    }

    public void TriggerAttack2()
    {
        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _audioSourceAttack2.pitch += randomPitchScale;

        //Play Sound
        _audioSourceAttack2.PlayOneShot(_attack2Sound, randVolumeScale);

        //Reset initial pitch
        _audioSourceAttack2.pitch = 1f;
    }
}
