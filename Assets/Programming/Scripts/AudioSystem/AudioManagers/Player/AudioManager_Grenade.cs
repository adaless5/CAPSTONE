using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager_Grenade : AudioManager
{
    public enum HitType
    {
        Dirt,
        Metal,
        Rock,
    }

    const int NUM_COOK_SOUNDS = 6;
    const int NUM_EXPLODE_SOUNDS = 8;
    const int NUM_HIT_DIRT_SOUNDS = 8;
    const int NUM_HIT_METAL_SOUNDS = 6;
    const int NUM_HIT_ROCK_SOUNDS = 5;
    const int NUM_THROW_SOUNDS = 5;

    Camera _camRef;

    AudioSource _LaunchPoint;
    const int LAUNCH_POINT_INDEX = 0;

    AudioSource _grenadeCenterPoint;
    const int GRENADE_CENTER_POINT_INDEX = 1;

    AudioSource _grenadeExplodePoint;
    const int GRENADE_EXPLODE_POINT_INDEX = 2;

    int _lastCookIndex = -1;
    int _lastExplodeIndex = -1;
    int _lastHitDirtIndex = -1;
    int _lastHitMetalIndex = -1;
    int _lastHitRockIndex = -1;
    int _lastThrowIndex = -1;

    public override void Initialize()
    {
        //Fetch Mixer
        AudioMixer _grenadeMixer = Resources.Load<AudioMixer>("Data/AudioData/Mixers/Grenade");

        //Fetch references
        _camRef = GetComponentInParent<Camera>();


        ////Setup Sounds and mixer routing

        //Cook Sound
        for (int i = 1; i <= NUM_COOK_SOUNDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Grenade/Cook/Grenade_Cook_"+ i),
            "Cook_",
            _grenadeMixer.FindMatchingGroups("Master")[1]
            );
            _sounds.Add(s);
        }

        //Explode Sounds
        for (int i = 1; i <= NUM_EXPLODE_SOUNDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Grenade/Explode/Grenade_Explode_"+ i),
            "Grenade_Explode_" + i,
            _grenadeMixer.FindMatchingGroups("Master")[2]
            ); _sounds.Add(s);
        }

        //Hit Dirt
        for (int i = 1; i <= NUM_HIT_DIRT_SOUNDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Grenade/Hit_Dirt/Grenade_Hit_Dirt_"+ i),
            "Grenade_Hit_Dirt_" + i,
            _grenadeMixer.FindMatchingGroups("Master")[3]
            ); _sounds.Add(s);
        }
        //

        //Hit Metal
        for (int i = 1; i <= NUM_HIT_METAL_SOUNDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Grenade/Hit_Metal/Grenade_Hit_Metal_"+ i),
            "Grenade_Hit_Metal_" + i,
            _grenadeMixer.FindMatchingGroups("Master")[4]
            ); _sounds.Add(s);
        }
        //

        //Hit Rock
        for (int i = 1; i <= NUM_HIT_ROCK_SOUNDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Grenade/Hit_Rock/Grenade_Hit_Rock_"+ i),
            "Grenade_Hit_Rock_" + i,
            _grenadeMixer.FindMatchingGroups("Master")[5]
            ); _sounds.Add(s);
        }
        //

        //Throw
        for (int i = 1; i <= NUM_THROW_SOUNDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Grenade/Throw/Grenade_Throw_"+ i),
            "Grenade_Throw_" + i,
            _grenadeMixer.FindMatchingGroups("Master")[6]
            ); _sounds.Add(s);
        }
        //
        ////

        //Add AudioSources
        _LaunchPoint = _camRef.transform.Find("EquipmentSoundPointClose").gameObject.GetComponent<AudioSource>();
        _sources.Add(_LaunchPoint);

        _grenadeCenterPoint = GetComponentsInChildren<AudioSource>()[0];
        _sources.Add(_grenadeCenterPoint);

        _grenadeExplodePoint = GetComponentsInChildren<AudioSource>()[1];
        _sources.Add(_grenadeExplodePoint);

        //
    }

    public void SetGrenadeSoundPointTransform(Transform t)
    {
        _grenadeCenterPoint.transform.position = t.position;
        _grenadeExplodePoint.transform.position = t.position;
    }


    const int COOK_INDEX_START = 0;
    const int COOK_INDEX_END = 5;
    //Picks a random Cook index, (never the same one in a row)
    int PickRandomCook()
    {
        int i;
        do
        {
            i = Random.Range(COOK_INDEX_START, COOK_INDEX_END + 1);
        } while (i == _lastCookIndex);
        _lastCookIndex = i;

        return i;
    }

    const int EXPLODE_INDEX_START = 6;
    const int EXPLODE_INDEX_END = 13;
    //Picks a random Explode index, (never the same one in a row)
    int PickRandomExplode()
    {
        int i;
        do
        {
            i = Random.Range(EXPLODE_INDEX_START, EXPLODE_INDEX_END + 1);
        } while (i == _lastExplodeIndex);
        _lastExplodeIndex = i;

        return i;
    }

    const int HIT_DIRT_INDEX_START = 14;
    const int HIT_DIRT_INDEX_END = 21;
    //Picks a random Hit Dirt index, (never the same one in a row)
    int PickRandomHitDirt()
    {
        int i;
        do
        {
            i = Random.Range(HIT_DIRT_INDEX_START, HIT_DIRT_INDEX_END + 1);
        } while (i == _lastHitDirtIndex);
        _lastHitDirtIndex = i;

        return i;
    }

    const int HIT_METAL_INDEX_START = 22;
    const int HIT_METAL_INDEX_END = 27;
    //Picks a random Hit Metal index, (never the same one in a row)
    int PickRandomHitMetal()
    {
        int i;
        do
        {
            i = Random.Range(HIT_METAL_INDEX_START, HIT_METAL_INDEX_END + 1);
        } while (i == _lastHitMetalIndex);
        _lastHitMetalIndex = i;

        return i;
    }

    const int HIT_ROCK_INDEX_START = 28;
    const int HIT_ROCK_INDEX_END = 32;
    //Picks a random Hit Rock index, (never the same one in a row)
    int PickRandomHitRock()
    {
        int i;
        do
        {
            i = Random.Range(HIT_ROCK_INDEX_START, HIT_ROCK_INDEX_END + 1);
        } while (i == _lastHitRockIndex);
        _lastHitRockIndex = i;

        return i;
    }

    const int THROW_INDEX_START = 33;
    const int THROW_INDEX_END = 38;
    //Picks a random Throw index, (never the same one in a row)
    int PickRandomThrow()
    {
        int i;
        do
        {
            i = Random.Range(THROW_INDEX_START, THROW_INDEX_END + 1);
        } while (i == _lastThrowIndex);
        _lastThrowIndex = i;

        return i;
    }

    public void TriggerThrowGrenade()
    {
        //Get Sound Index
        int i = 0;
        i = PickRandomThrow();
        //

        //Route source to proper mixer group
        _sources[LAUNCH_POINT_INDEX].outputAudioMixerGroup = _sounds[i]._mixerGroup;

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _sources[LAUNCH_POINT_INDEX].pitch += randomPitchScale;

        //Play Sound
        _sources[LAUNCH_POINT_INDEX].PlayOneShot(_sounds[i]._clip, randVolumeScale);

        //Reset initial pitch
        _sources[LAUNCH_POINT_INDEX].pitch = 1.0f;
    }

    public void TriggerGrenadeCook()
    {
        //Get Sound Index
        int i = 0;
        i = PickRandomCook();
        //

        //Route source to proper mixer group
        _sources[GRENADE_EXPLODE_POINT_INDEX].outputAudioMixerGroup = _sounds[i]._mixerGroup;

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _sources[GRENADE_EXPLODE_POINT_INDEX].pitch += randomPitchScale;

        //Play Sound
        _sources[GRENADE_EXPLODE_POINT_INDEX].PlayOneShot(_sounds[i]._clip, randVolumeScale);

        //Reset initial pitch
        _sources[GRENADE_EXPLODE_POINT_INDEX].pitch = 1.0f;
    }

    public void TriggerGrenadeExplode()
    {
        // Get Sound Index
        int i = 0;
        i = PickRandomExplode();
        //

        //Route source to proper mixer group
        _sources[GRENADE_EXPLODE_POINT_INDEX].outputAudioMixerGroup = _sounds[i]._mixerGroup;

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _sources[GRENADE_EXPLODE_POINT_INDEX].pitch += randomPitchScale;

        //Play Sound
        _sources[GRENADE_EXPLODE_POINT_INDEX].PlayOneShot(_sounds[i]._clip, randVolumeScale);

        //Reset initial pitch
        _sources[GRENADE_EXPLODE_POINT_INDEX].pitch = 1.0f;
    }

    public void TriggerGrenadeHit(HitType type)
    {
        // Get Sound Index
        int i = 0;
        
        switch(type)
        {
            case HitType.Dirt: i = PickRandomHitDirt(); break;
            case HitType.Metal: i = PickRandomHitMetal(); break;
            case HitType.Rock: i = PickRandomHitRock(); break;
        }
        //

        //Route source to proper mixer group
        _sources[GRENADE_CENTER_POINT_INDEX].outputAudioMixerGroup = _sounds[i]._mixerGroup;

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _sources[GRENADE_CENTER_POINT_INDEX].pitch += randomPitchScale;

        //Play Sound
        _sources[GRENADE_CENTER_POINT_INDEX].PlayOneShot(_sounds[i]._clip, randVolumeScale);

        //Reset initial pitch
        _sources[GRENADE_CENTER_POINT_INDEX].pitch = 1.0f;
    }

}
