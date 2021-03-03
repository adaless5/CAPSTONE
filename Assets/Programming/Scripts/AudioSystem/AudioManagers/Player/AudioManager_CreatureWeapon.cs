using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager_CreatureWeapon : AudioManager
{

    const int NUM_SHOT_SOUNDS = 5;
    const int NUM_RELOAD_SOUNDS = 5;
    const int NUM_STICK_SOUNDS = 5;

    int _lastShotIndex = -1;
    int _lastReloadIndex = -1;
    int _lastStickIndex = -1;

    AudioSource _Muzzle;
    const int MUZZLE_INDEX = 0;

    AudioSource _ChamberLoadPoint;
    const int CHAMBER_LOAD_POINT_INDEX = 1;


    Camera _camRef;

    public override void Initialize()
    {
        //Fetch Mixer
        AudioMixer _creatureWeaponMixer = Resources.Load<AudioMixer>("Data/AudioData/Mixers/Creature Weapon");

        //Fetch references
        _camRef = GetComponentInParent<Camera>();

        ////Setup Sounds and mixer routing

        //Shot Sound
        for (int i = 1; i <= NUM_SHOT_SOUNDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Creature Weapon/Shot/Creature_Weapon_Shot_" + i),
            "Creature_Weapon_Shot_",
            _creatureWeaponMixer.FindMatchingGroups("Master")[1]
            );
            _sounds.Add(s);
        }

        //Reload Sounds
        for (int i = 1; i <= NUM_RELOAD_SOUNDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Creature Weapon/Reload/Creature_Weapon_Reload_" + i),
            "Creature_Weapon_Reload_" + i,
            _creatureWeaponMixer.FindMatchingGroups("Master")[2]
            ); _sounds.Add(s);
        }

        //Stick
        for (int i = 1; i <= NUM_STICK_SOUNDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Creature Weapon/Stick/Creature_Weapon_Stick_" + i),
            "Creature_Weapon_Stick_" + i,
            _creatureWeaponMixer.FindMatchingGroups("Master")[3]
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

    const int SHOT_INDEX_START = 0;
    const int SHOT_INDEX_END = 4;
    //Picks a random Shot index, (never the same one in a row)
    int PickRandomShot()
    {
        int i;
        do
        {
            i = Random.Range(SHOT_INDEX_START, SHOT_INDEX_END + 1);
        } while (i == _lastShotIndex);
        _lastShotIndex = i;

        return i;
    }

    const int RELOAD_INDEX_START = 5;
    const int RELOAD_INDEX_END = 9;
    //Picks a random Reload index, (never the same one in a row)
    int PickRandomReload()
    {
        int i;
        do
        {
            i = Random.Range(RELOAD_INDEX_START, RELOAD_INDEX_END + 1);
        } while (i == _lastReloadIndex);
        _lastReloadIndex = i;

        return i;
    }

    const int STICK_INDEX_START = 10;
    const int STICK_INDEX_END = 14;
    //Picks a random Stick index, (never the same one in a row)
    int PickRandomStick()
    {
        int i;
        do
        {
            i = Random.Range(STICK_INDEX_START, STICK_INDEX_END + 1);
        } while (i == _lastStickIndex);
        _lastStickIndex = i;

        return i;
    }

    public void TriggerShootCreatureWeapon()
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

    public void TriggerReloadCreatureWeapon()
    {
        //Get Sound Index
        int i = 0;
        i = PickRandomReload();
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

    public void TriggerStickCreatureWeapon(AudioSource source)
    {
        //Get Sound Index
        int i = 0;
        i = PickRandomStick();
        //

        //Route source to proper mixer group
        source.outputAudioMixerGroup = _sounds[i]._mixerGroup;

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        source.pitch += randomPitchScale;

        //Play Sound
        source.PlayOneShot(_sounds[i]._clip, randVolumeScale);

        //Reset initial pitch
        source.pitch = 1.0f;
    }


}
