using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager_Ambiance : AudioManager
{

    AudioSource _AudioSource;
    const int _AUDIO_SOURCE_INDEX = 0;

    public override void Initialize()
    {
        //Fetch Mixer
        AudioMixer _ambianceMixer = Resources.Load<AudioMixer>("Data/AudioData/Mixers/Ambiance");

        //Menu Ambiance
        Sound s1 = new Sound(
        Resources.Load<AudioClip>("Data/AudioData/AudioClips/Ambiance/Abyssian Atmosphere_menu"),
        "Menu_Ambiance",
        _ambianceMixer.FindMatchingGroups("Master")[1]
        ); _sounds.Add(s1);


        //Level Ambiance
        Sound s2 = new Sound(
        Resources.Load<AudioClip>("Data/AudioData/AudioClips/Ambiance/Abyssian Atmosphere_level"),
        "Level_Ambiance",
        _ambianceMixer.FindMatchingGroups("Master")[2]
        ); _sounds.Add(s2);

        //Add AudioSources
        _AudioSource = GetComponent<AudioSource>();
        _sources.Add(_AudioSource);
        //

    }

    public void TriggerMenuAmbiance()
    {
        StopAmbiance();
        TriggerMusicSound(0);
    }

    public void StopAmbiance()
    {
        _sources[0].Stop();
    }

    public bool isPlaying()
    {
        return _sources[0].isPlaying;
    }

    public void TriggerLevelAmbiance()
    {
        StopAmbiance();
        TriggerMusicSound(1);
    }


    void TriggerMusicSound(int i)
    {
        //Route source to proper mixer group
        _sources[_AUDIO_SOURCE_INDEX].outputAudioMixerGroup = _sounds[i]._mixerGroup;

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _sources[_AUDIO_SOURCE_INDEX].pitch += randomPitchScale;

        //Play Sound
        _sources[_AUDIO_SOURCE_INDEX].PlayOneShot(_sounds[i]._clip, randVolumeScale);

        //Reset initial pitch
        _sources[_AUDIO_SOURCE_INDEX].pitch = 1.0f;
    }


}
