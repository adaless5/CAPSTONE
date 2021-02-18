using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager_MetalDoor : AudioManager
{
    const int NUM_OPEN_SOUNDS = 5;
    const int NUM_CLOSE_SOUNDS = 5;

    int _lastOpenIndex = -1;
    int _lastCloseIndex = -1;

    Camera _camRef;

    AudioSource _audioSource;
    const int AUDIO_SOURCE_INDEX = 0;

    public override void Initialize()
    {
        //Fetch Mixer
        AudioMixer _metalDoorMixer = Resources.Load<AudioMixer>("Data/AudioData/Mixers/MetalDoor");

        //Fetch references
        _camRef = GetComponentInParent<Camera>();

        ////Setup Sounds and mixer routing

        //Shot Sound
        for (int i = 1; i <= NUM_OPEN_SOUNDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/MetalDoor/Open/MetalDoor_Open_" + i),
            "MetalDoor_Open_",
            _metalDoorMixer.FindMatchingGroups("Master")[1]
            );
            _sounds.Add(s);
        }

        //Reload Sounds
        for (int i = 1; i <= NUM_CLOSE_SOUNDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/MetalDoor/Close/MetalDoor_Close_" + i),
            "MetalDoor_Close_" + i,
            _metalDoorMixer.FindMatchingGroups("Master")[2]
            ); _sounds.Add(s);
        }
    }

    const int OPEN_INDEX_START = 0;
    const int OPEN_INDEX_END = 4;
    //Picks a random OPEN index, (never the same one in a row)
    int PickRandomOpen()
    {
        int i;
        do
        {
            i = Random.Range(OPEN_INDEX_START, OPEN_INDEX_END + 1);
        } while (i == _lastOpenIndex);
        _lastOpenIndex = i;

        return i;
    }

    const int CLOSE_INDEX_START = 5;
    const int CLOSE_INDEX_END = 9;
    //Picks a random CLOSE index, (never the same one in a row)
    int PickRandomClose()
    {
        int i;
        do
        {
            i = Random.Range(CLOSE_INDEX_START, CLOSE_INDEX_END + 1);
        } while (i == _lastCloseIndex);
        _lastCloseIndex = i;

        return i;
    }

    public void TriggerOpenDoor()
    {
        //Get Sound Index
        int i = 0;
        i = PickRandomOpen();
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

    public void TriggerCloseDoor()
    {
        //Get Sound Index
        int i = 0;
        i = PickRandomClose();
        //

        //Stop open door sound
        _sources[AUDIO_SOURCE_INDEX].Stop();

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

