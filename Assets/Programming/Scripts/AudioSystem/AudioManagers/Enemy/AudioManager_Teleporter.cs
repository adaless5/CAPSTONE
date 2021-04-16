using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_Teleporter : AudioManager
{
    public AudioSource _audioSourceTeleport;


    public override void Initialize()
    {

    }

    public void TriggerTeleport()
    {
        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _audioSourceTeleport.pitch += randomPitchScale;

        //Play Sound
        _audioSourceTeleport.PlayOneShot(_audioSourceTeleport.clip, randVolumeScale);

        //Reset initial pitch
        _audioSourceTeleport.pitch = 1.44f;
    }

    public bool teleportIsPlaying()
    {
        return _audioSourceTeleport.isPlaying;
    }


}
