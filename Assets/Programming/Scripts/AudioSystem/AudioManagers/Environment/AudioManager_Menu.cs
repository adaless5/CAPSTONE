using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager_Menu : AudioManager
{
    public enum MENU_SOUND_TYPE
    {
        Play,
        Hover,
        Click_Forward,
        Click_Back,
        Weapon_Swap,
    }

    const int NUM_PLAY_SOUNDS = 5;

    AudioSource _AudioSource;
    const int _AUDIO_SOURCE_INDEX = 0;

    public override void Initialize()
    {
        //Fetch Mixer
        AudioMixer _menuMixer = Resources.Load<AudioMixer>("Data/AudioData/Mixers/SFX_Menu");

        ////Setup Sounds and mixer routing
        //Play Sounds
        for (int i = 1; i <= NUM_PLAY_SOUNDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Menu/Play/Menu_Click_Play_"+i),
            "Menu_Click_Play_"+i,
            _menuMixer.FindMatchingGroups("Master")[2]
            );
            _sounds.Add(s);
        }

        //Hover
        Sound s1 = new Sound(
        Resources.Load<AudioClip>("Data/AudioData/AudioClips/Menu/Hover/Menu_Hover"),
        "Menu_Hover",
        _menuMixer.FindMatchingGroups("Master")[1]
        ); _sounds.Add(s1);

        //Click Forward
        Sound s2 = new Sound(
        Resources.Load<AudioClip>("Data/AudioData/AudioClips/Menu/Select/Menu_Click_Forward"),
        "Menu_Click_Back",
        _menuMixer.FindMatchingGroups("Master")[3]
        ); _sounds.Add(s2);

        //Click Back
        Sound s3 = new Sound(
        Resources.Load<AudioClip>("Data/AudioData/AudioClips/Menu/Select/Menu_Click_Back"),
        "Menu_Click_Forward",
        _menuMixer.FindMatchingGroups("Master")[4]
        ); _sounds.Add(s3);

        //Weapon Swap
        Sound s4 = new Sound(
        Resources.Load<AudioClip>("Data/AudioData/AudioClips/Menu/WeaponSwap/Select_Weapon"),
        "Select_Weapon",
        _menuMixer.FindMatchingGroups("Master")[5]
        ); _sounds.Add(s4);


        //Add AudioSources
        _AudioSource = GetComponent<AudioSource>();
        _sources.Add(_AudioSource);
        //
    }

    const int NEW_PLAY_INDEX_START = 0;
    const int NEW_PLAY_INDEX_END = 4;
    int _lastPlayIndex = -1;
    //Picks a random Play sound index, (never the same one in a row)
    int PickRandomPlay()
    {
        int i;
        do
        {
            i = Random.Range(NEW_PLAY_INDEX_START, NEW_PLAY_INDEX_END + 1);
        } while (i == _lastPlayIndex);
        _lastPlayIndex = i;

        return i;
    }

    public void TriggerPlaySound()
    {
        TriggerMenuSound(PickRandomPlay());
    }

    public void TriggerHoverSound()
    {
        TriggerMenuSound(5);
    }

    public void TriggerClickForwardSound()
    {
        TriggerMenuSound(6);
    }

    public void TriggerClickBackwardSound()
    {
        TriggerMenuSound(7);
    }

    public void TriggerWeaponSwapSound()
    {
        TriggerMenuSound(8);
    }
    
    void TriggerMenuSound(int i)
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

    public IEnumerator DeleteAudioListenerAsync()
    {
        yield return new WaitForSeconds(5);
        Destroy(transform.GetChild(0).gameObject);
    }

    public void DeleteAudioListener()
    {
        StartCoroutine(DeleteAudioListenerAsync());
    }
}
