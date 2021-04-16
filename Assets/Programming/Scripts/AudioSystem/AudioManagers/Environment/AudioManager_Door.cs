using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager_Door : AudioManager
{
    public AudioSource _AudioSource;
    public AudioClip _OpenSound;
    public AudioClip _CloseSound;

    public enum Playing
    {
        none,
        open,
        close
    }
    public Playing _Playing = Playing.none;

    public override void Initialize()
    {
    }

    public void Play(bool open)
    {

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _AudioSource.pitch += randomPitchScale;

        //Play Sound
        if (open) { _AudioSource.Stop();  _AudioSource.PlayOneShot(_OpenSound, randVolumeScale); _Playing = Playing.open; }
        else { _AudioSource.Stop(); _AudioSource.PlayOneShot(_CloseSound, randVolumeScale); _Playing = Playing.close; }

        //Reset initial pitch
        _AudioSource.pitch = 1.0f;
    }
}