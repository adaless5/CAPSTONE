using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager_Sword : AudioManager
{
    const int NUM_SWINGS = 5;
    Camera _camRef;

    AudioSource _AudioSource;
    const int AUDIO_SOURCE_INDEX = 0;

    public override void Initialize()
    {
        //Fetch Mixer
        AudioMixer _mixer = Resources.Load<AudioMixer>("Data/AudioData/Mixers/Sword");

        //Fetch references
        _camRef = GetComponentInParent<Camera>();

        ////Setup Sounds and mixer routing

        //Swing
        for (int i = 1; i <= NUM_SWINGS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Sword/blade_swing_" + i),
            "blade_swing_" + i,
            _mixer.FindMatchingGroups("Master")[0]
            );
            _sounds.Add(s);
        }

        //Add AudioSources
        _AudioSource = _camRef.transform.Find("EquipmentSoundPointClose").gameObject.GetComponent<AudioSource>();
        _sources.Add(_AudioSource);
    }

    const int SWING_INDEX_START = 0;
    const int SWING_INDEX_END = 4;
    int _lastSwingIndex = -1;
    //Picks a random Swing index, (never the same one in a row)
    int PickRandomSwing()
    {
        int i;
        do
        {
            i = Random.Range(SWING_INDEX_START, SWING_INDEX_END + 1);
        } while (i == _lastSwingIndex);
        _lastSwingIndex = i;

        return i;
    }

    public bool isPlaying()
    {
        return _sources[AUDIO_SOURCE_INDEX].isPlaying;
    }

    public void TriggerSwing()
    {
        //Get Sound Index
        int i = 0;
        i = PickRandomSwing();
        //

        //Route source to proper mixer group
        _sources[AUDIO_SOURCE_INDEX].outputAudioMixerGroup = _sounds[i]._mixerGroup;

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _sources[AUDIO_SOURCE_INDEX].pitch += randomPitchScale;

        //Play Sound
        _sources[AUDIO_SOURCE_INDEX].PlayOneShot(_sounds[i]._clip, randVolumeScale);

        //Reset initial pitch
        _sources[AUDIO_SOURCE_INDEX].pitch = 1.0f;
    }


}
