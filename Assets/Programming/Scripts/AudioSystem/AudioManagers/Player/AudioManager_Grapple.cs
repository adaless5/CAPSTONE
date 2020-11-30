using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager_Grapple : AudioManager
{
    const int NUM_CLICKS = 6;
    const int NUM_HITS_DIRT = 6;
    const int NUM_HITS_CONCRETE = 6;
    const int NUM_RETRACTS = 6;
    const int NUM_SHOTS = 6;

    Camera _camRef;

    AudioSource _grapple_Muzzle;
    const int GRAPPLE_MUZZLE_INDEX = 0;

    AudioSource _grapple_CockingPoint;
    const int GRAPPLE_COCKING_POINT_INDEX = 1;

    AudioSource _grapple_ClawGraspPoint;
    const int GRAPPLE_CLAW_GRASP_POINT_INDEX = 2;

    enum HitType
    {
        Dirt,
        Concrete,
    }
    HitType _hitType = HitType.Dirt;

    int _dirtLayerIndex;
    int _concreteLayerIndex;

    public override void Initialize()
    {
        //Fetch Mixer
        AudioMixer grappleMixer = Resources.Load<AudioMixer>("Data/AudioData/Mixers/Grapple Hook");

        //Fetch references
        _camRef = GetComponentInParent<Camera>();
        _dirtLayerIndex = LayerMask.NameToLayer("Terrain_Dirt");
        _concreteLayerIndex = LayerMask.NameToLayer("Terrain_Concrete");

        ////Setup Sounds and mixer routing

        //Click
        for (int i = 1; i <= NUM_CLICKS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Grapple Hook/Grapple_Click_" + i), 
            "Grapple_Click_" + i,
            grappleMixer.FindMatchingGroups("Master")[1]
            );
            _sounds.Add(s);
        }

        //Hit Dirt
        for (int i = 1; i <= NUM_HITS_DIRT; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Grapple Hook/Grapple_Hit_Dirt_" + i), 
            "Grapple_Hit_Dirt_" + i,
            grappleMixer.FindMatchingGroups("Master")[2]
            );
            _sounds.Add(s);
        }

        //Hit Concrete
        for (int i = 1; i <= NUM_HITS_CONCRETE; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Grapple Hook/Grapple_Hit_Concrete_" + i),
            "Grapple_Hit_Concrete_" + i,
            grappleMixer.FindMatchingGroups("Master")[3]
            );
            _sounds.Add(s);
        }

        //Retract
        for (int i = 1; i <= NUM_RETRACTS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Grapple Hook/Grapple_Retracting_" + i), 
            "Grapple_Retracting_" + i,
            grappleMixer.FindMatchingGroups("Master")[4]
            );
            _sounds.Add(s);
        }

        //Shot
        for (int i = 1; i <= NUM_SHOTS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Grapple Hook/Grapple_Shot_" + i), 
            "Grapple_Shot_" + i,
            grappleMixer.FindMatchingGroups("Master")[5]
            );
            _sounds.Add(s);
        }

        ////
        
        //Add AudioSources
        _grapple_Muzzle = _camRef.transform.Find("EquipmentSoundPointFar").gameObject.GetComponent<AudioSource>();
        _sources.Add(_grapple_Muzzle);

        _grapple_CockingPoint = _camRef.transform.Find("EquipmentSoundPointClose").gameObject.GetComponent<AudioSource>();
        _sources.Add(_grapple_CockingPoint);

        _grapple_ClawGraspPoint = _camRef.transform.Find("GrappleClawGraspPoint").gameObject.GetComponent<AudioSource>();
        _sources.Add(_grapple_ClawGraspPoint);
        //
    }

    const int CLICK_INDEX_START = 0;
    const int CLICK_INDEX_END = 5;
    int _lastClickIndex = -1;
    //Picks a random Click index, (never the same one in a row)
    int PickRandomClick()
    {
        int i;
        do
        {
            i = Random.Range(CLICK_INDEX_START, CLICK_INDEX_END + 1);
        } while (i == _lastClickIndex);
        _lastClickIndex = i;

        return i;
    }

    const int HIT_DIRT_INDEX_START = 6;
    const int HIT_DIRT_INDEX_END = 11;
    int _lastHitDirtIndex = -1;
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

    const int HIT_CONCRETE_INDEX_START = 12;
    const int HIT_CONCRETE_INDEX_END = 17;
    int _lastHitConcreteIndex = -1;
    //Picks a random Hit Concrete index, (never the same one in a row)
    int PickRandomHitConcrete()
    {
        int i;
        do
        {
            i = Random.Range(HIT_CONCRETE_INDEX_START, HIT_CONCRETE_INDEX_END + 1);
        } while (i == _lastHitConcreteIndex);
        _lastHitConcreteIndex = i;

        return i;
    }

    const int RETRACT_INDEX_START = 18;
    const int RETRACT_INDEX_END = 23;
    int _lastRetractIndex = -1;
    //Picks a random Retract index, (never the same one in a row)
    int PickRandomRetract()
    {
        int i;
        do
        {
            i = Random.Range(RETRACT_INDEX_START, RETRACT_INDEX_END + 1);
        } while (i == _lastRetractIndex);
        _lastRetractIndex = i;

        return i;
    }

    const int SHOT_INDEX_START = 24;
    const int SHOT_INDEX_END = 29;
    int _lastShotIndex = -1;
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

    //Sets the audio source transform to the given transform. (used to set grapple impact sound point)
    public void SetGrappleHookPointAndHitType(Transform t, int layer)
    {
        //Set Grasp point transform
        _grapple_ClawGraspPoint.transform.position = t.position;

        //Set hit layer
        if (layer == _dirtLayerIndex)
        {
            //Hit Dirt
            _hitType = HitType.Dirt;
        }

        else if (layer == _concreteLayerIndex)
        {
            //Hit Concrete
            _hitType = HitType.Concrete;
        }
        //
    }

    public void TriggerClick()
    {
        //Get Sound Index
        int i = 0;
        i = PickRandomClick();
        //

        //Route source to proper mixer group
        _sources[GRAPPLE_COCKING_POINT_INDEX].outputAudioMixerGroup = _sounds[i]._mixerGroup;

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _sources[GRAPPLE_COCKING_POINT_INDEX].pitch += randomPitchScale;

        //Play Sound
        _sources[GRAPPLE_COCKING_POINT_INDEX].PlayOneShot(_sounds[i]._clip, randVolumeScale);

        //Reset initial pitch
        _sources[GRAPPLE_COCKING_POINT_INDEX].pitch = 1.0f;
    }

    public void TriggerHit()
    {
        //Get Footstep Index
        int i = 0;
        switch (_hitType)
        {
            case HitType.Dirt:
                i = PickRandomHitDirt(); break;

            case HitType.Concrete:
                i = PickRandomHitConcrete(); break;
        }
        //

        //Route source to proper mixer group
        _sources[GRAPPLE_CLAW_GRASP_POINT_INDEX].outputAudioMixerGroup = _sounds[i]._mixerGroup;

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _sources[GRAPPLE_CLAW_GRASP_POINT_INDEX].pitch += randomPitchScale;

        //Play footstep
        _sources[GRAPPLE_CLAW_GRASP_POINT_INDEX].PlayOneShot(_sounds[i]._clip, randVolumeScale);

        //Reset initial pitch
        _sources[GRAPPLE_CLAW_GRASP_POINT_INDEX].pitch = 1.0f;
    }

    public void TriggerRetract()
    {
        //Get Sound Index
        int i = 0;
        i = PickRandomRetract();
        //

        //Route source to proper mixer group
        _sources[GRAPPLE_MUZZLE_INDEX].outputAudioMixerGroup = _sounds[i]._mixerGroup;

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _sources[GRAPPLE_MUZZLE_INDEX].pitch += randomPitchScale;

        //Play Sound
        _sources[GRAPPLE_MUZZLE_INDEX].PlayOneShot(_sounds[i]._clip, randVolumeScale);

        //Reset initial pitch
        _sources[GRAPPLE_MUZZLE_INDEX].pitch = 1.0f;
    }

    public void StopRetract()
    {
        _sources[GRAPPLE_MUZZLE_INDEX].Stop();
    }

    public void TriggerShot()
    {
        //Get Sound Index
        int i = 0;
        i = PickRandomShot();
        //

        //Route source to proper mixer group
        _sources[GRAPPLE_MUZZLE_INDEX].outputAudioMixerGroup = _sounds[i]._mixerGroup;

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _sources[GRAPPLE_MUZZLE_INDEX].pitch += randomPitchScale;

        //Play Sound
        _sources[GRAPPLE_MUZZLE_INDEX].PlayOneShot(_sounds[i]._clip, randVolumeScale);

        //Reset initial pitch
        _sources[GRAPPLE_MUZZLE_INDEX].pitch = 1.0f;
    }
    public void StopShot()
    {
        _sources[GRAPPLE_MUZZLE_INDEX].Stop();
    }
}
