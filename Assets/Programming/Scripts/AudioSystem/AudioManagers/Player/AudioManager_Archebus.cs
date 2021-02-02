using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager_Archebus : AudioManager
{
    const int NUM_EMPTY_SOUNDS = 1;
    const int NUM_NEW_SHELL_SOUNDS = 6;
    const int NUM_RELOAD_END_SOUNDS = 8;
    const int NUM_RELOAD_START_SOUNDS = 8;
    const int NUM_SHOT_SOUNDS = 10;

    int _lastNewShellIndex = -1;
    int _lastReloadEndIndex = -1;
    int _lastReloadStartIndex = -1;
    int _lastShotIndex = -1;

    AudioSource _Muzzle;
    const int MUZZLE_INDEX = 0;

    AudioSource _ChamberLoadPoint;
    const int CHAMBER_LOAD_POINT_INDEX = 1;

    Camera _camRef;

    public override void Initialize()
    {
        //Fetch Mixer
        AudioMixer _archebusMixer = Resources.Load<AudioMixer>("Data/AudioData/Mixers/Archebus");

        //Fetch references
        _camRef = GetComponentInParent<Camera>();


        ////Setup Sounds and mixer routing

        //Empty Sound
        for (int i = 1; i <= NUM_EMPTY_SOUNDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Archebus/Empty/Empty"),
            "Empty",
            _archebusMixer.FindMatchingGroups("Master")[1]
            );
            _sounds.Add(s);
        }

        //New Shell Sounds
        for (int i = 1; i <= NUM_NEW_SHELL_SOUNDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Archebus/New Shell/New_Shell_" + i),
            "New_Shell_" + i,
            _archebusMixer.FindMatchingGroups("Master")[2]
            ); _sounds.Add(s);
        }

        //Reload_End Sounds
        for (int i = 1; i <= NUM_RELOAD_END_SOUNDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Archebus/Reload_End/Reload_End_" + i),
            "Reload_End_" + i,
            _archebusMixer.FindMatchingGroups("Master")[3]
            ); _sounds.Add(s);
        }
        //

        //Reload_Start Sounds
        for (int i = 1; i <= NUM_RELOAD_START_SOUNDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Archebus/Reload_Start/Reload_Start_" + i),
            "Reload_Start_" + i,
            _archebusMixer.FindMatchingGroups("Master")[4]
            ); _sounds.Add(s);
        }
        //

        //Shot Sounds
        for (int i = 1; i <= NUM_SHOT_SOUNDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Archebus/Shot/Default_Gun_Shot (" + i + ")"),
            "Default_Gun_Shot_" + i,
            _archebusMixer.FindMatchingGroups("Master")[0]
            ); _sounds.Add(s);
        }
        //
        ////

        //Add AudioSources
        _Muzzle = _camRef.transform.Find("EquipmentSoundPointFar").gameObject.GetComponent<AudioSource>();
        _sources.Add(_Muzzle);

        _ChamberLoadPoint = _camRef.transform.Find("EquipmentSoundPointClose").gameObject.GetComponent<AudioSource>();
        _sources.Add(_ChamberLoadPoint);
        //
    }

    const int NEW_SHELL_INDEX_START = 1;
    const int NEW_SHELL_INDEX_END = 6;
    //Picks a random New Shell index, (never the same one in a row)
    int PickRandomNewShell()
    {
        int i;
        do
        {
            i = Random.Range(NEW_SHELL_INDEX_START, NEW_SHELL_INDEX_END + 1);
        } while (i == _lastNewShellIndex);
        _lastNewShellIndex = i;

        return i;
    }

    const int RELOAD_END_INDEX_START = 7;
    const int RELOAD_END_INDEX_END = 14;
    //Picks a random RELOAD_END index, (never the same one in a row)
    int PickRandomReloadEnd()
    {
        int i;
        do
        {
            i = Random.Range(RELOAD_END_INDEX_START, RELOAD_END_INDEX_END + 1);
        } while (i == _lastReloadEndIndex);
        _lastReloadEndIndex = i;

        return i;
    }

    const int RELOAD_START_INDEX_START = 15;
    const int RELOAD_START_INDEX_END = 22;
    //Picks a random RELOAD_Start index, (never the same one in a row)
    int PickRandomReloadStart()
    {
        int i;
        do
        {
            i = Random.Range(RELOAD_START_INDEX_START, RELOAD_START_INDEX_END + 1);
        } while (i == _lastReloadStartIndex);
        _lastReloadStartIndex = i;

        return i;
    }

    const int SHOT_START_INDEX = 23;
    const int SHOT_END_INDEX = 32;
    //Picks a random Shot index, (never the same one in a row)
    int PickRandomShot()
    {
        int i;
        do
        {
            i = Random.Range(SHOT_START_INDEX, SHOT_END_INDEX + 1);
        } while (i == _lastShotIndex);
        _lastShotIndex = i;

        return i;
    }

    public void TriggerEmpty()
    {
        //Get Sound Index
        int i = 0;
        //

        //Route source to proper mixer group
        _sources[CHAMBER_LOAD_POINT_INDEX].outputAudioMixerGroup = _sounds[i]._mixerGroup;

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _sources[CHAMBER_LOAD_POINT_INDEX].pitch += randomPitchScale;

        //Play Sound
        _sources[CHAMBER_LOAD_POINT_INDEX].PlayOneShot(_sounds[i]._clip, randVolumeScale);

        //Reset initial pitch
        _sources[CHAMBER_LOAD_POINT_INDEX].pitch = 1.0f;
    }

    public void TriggerNewShell()
    {
        //Get Sound Index
        int i = 0;
        i = PickRandomNewShell();
        //

        //Route source to proper mixer group
        _sources[CHAMBER_LOAD_POINT_INDEX].outputAudioMixerGroup = _sounds[i]._mixerGroup;

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _sources[CHAMBER_LOAD_POINT_INDEX].pitch += randomPitchScale;

        //Play Sound
        _sources[CHAMBER_LOAD_POINT_INDEX].PlayOneShot(_sounds[i]._clip, randVolumeScale);

        //Reset initial pitch
        _sources[CHAMBER_LOAD_POINT_INDEX].pitch = 1.0f;
    }

    public void TriggerReloadEnd()
    {
        //Get Sound Index
        int i = 0;
        i = PickRandomReloadEnd();
        //

        //Route source to proper mixer group
        _sources[CHAMBER_LOAD_POINT_INDEX].outputAudioMixerGroup = _sounds[i]._mixerGroup;

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _sources[CHAMBER_LOAD_POINT_INDEX].pitch += randomPitchScale;

        //Play Sound
        _sources[CHAMBER_LOAD_POINT_INDEX].PlayOneShot(_sounds[i]._clip, randVolumeScale);

        //Reset initial pitch
        _sources[CHAMBER_LOAD_POINT_INDEX].pitch = 1.0f;
    }

    public void TriggerReloadStart()
    {
        //Get Sound Index
        int i = 0;
        i = PickRandomReloadStart();
        //

        //Route source to proper mixer group
        _sources[CHAMBER_LOAD_POINT_INDEX].outputAudioMixerGroup = _sounds[i]._mixerGroup;

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _sources[CHAMBER_LOAD_POINT_INDEX].pitch += randomPitchScale;

        //Play Sound
        _sources[CHAMBER_LOAD_POINT_INDEX].PlayOneShot(_sounds[i]._clip, randVolumeScale);

        //Reset initial pitch
        _sources[CHAMBER_LOAD_POINT_INDEX].pitch = 1.0f;
    }

    public void TriggerShot()
    {
        //Get Sound Index
        int i = 0;
        i = PickRandomShot();
        //

        //Route source to proper mixer group
        _sources[MUZZLE_INDEX].outputAudioMixerGroup = _sounds[i]._mixerGroup;

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _sources[MUZZLE_INDEX].pitch += randomPitchScale;

        //Play Sound
        _sources[MUZZLE_INDEX].PlayOneShot(_sounds[i]._clip, randVolumeScale);

        //Reset initial pitch
        _sources[MUZZLE_INDEX].pitch = 1.0f;
    }
}
