using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//Add this line to play trigger the sound from another script
//GetComponent<AudioManager_VoiceOver>().PlayVoiceOver();

public class AudioManager_VoiceOver : AudioManager
{
    AudioSource _AudioSource;

    public string ClipName;

    public override void Initialize()
    {
        //Fetch Mixer
        AudioMixer _menuMixer = Resources.Load<AudioMixer>("Data/AudioData/Mixers/VoiceOver");

        ////Setup Sounds and mixer routing
        Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/VoiceOver/"+ClipName),
            ClipName,
            _menuMixer.FindMatchingGroups("Master")[0]
            );
        _sounds.Add(s);

        //Add AudioSources
        _AudioSource = GetComponent<AudioSource>();
        _sources.Add(_AudioSource);
        //

    }

    public void PlayVoiceOver()
    {

        //Route source to proper mixer group
        _sources[0].outputAudioMixerGroup = _sounds[0]._mixerGroup;

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _sources[0].pitch += randomPitchScale;

        //Play Sound
        _sources[0].PlayOneShot(_sounds[0]._clip, randVolumeScale);

        //Reset initial pitch
        _sources[0].pitch = 1.0f;
    }
}
