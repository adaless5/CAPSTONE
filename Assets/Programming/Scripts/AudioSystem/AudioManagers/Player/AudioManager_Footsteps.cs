using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager_Footsteps : AudioManager
{
    const int NUM_CONCRETE_FOOTSTEPS = 10;
    const int NUM_DIRT_FOOTSTEPS = 32;
    const int NUM_DIRT_JUMP_STARTS = 2;
    const int NUM_CONCRETE_JUMP_STARTS = 3;
    const int NUM_DIRT_JUMP_LANDS = 6;
    const int NUM_CONCRETE_JUMP_LANDS = 6;

    const float MIN_LANDING_VOLUME_SCALE = 1.0f;
    const float MAX_LANDING_VOLUME_SCALE = 2f;

    enum TerrainType
    {
        Dirt,
        Concrete,
    }
    TerrainType _terrainType = TerrainType.Dirt;

    AudioSource _LeftFoot;
    AudioSource _RightFoot;
    AudioSource _CenterFoot;

    Camera _camRef;
    CharacterController _controllerRef;

    Vector3 _lastPosition;
    float _distanceMoved = 0f;

    int _footToggle = 0;
    int _lastFootstepIndex = 0;

    float _footSeperationWidthFromCenter = 0.2f;
    float _distanceTillFootstepTriggered = 3f;

    int _dirtTerrainLayerIndex;
    int _concreteTerrainLayerIndex;

    bool _isFalling = false;
    float _maxFallVelocity = 0;

    public override void Initialize()
    {
        //Fetch Mixer
        AudioMixer footstepsMixer = Resources.Load<AudioMixer>("Data/AudioData/Mixers/Footsteps");

        //Fetch references
        _camRef = GetComponentInChildren<Camera>();
        _controllerRef = GetComponent<CharacterController>();
        _lastPosition = gameObject.transform.position;
        _dirtTerrainLayerIndex = LayerMask.NameToLayer("Terrain_Dirt");
        _concreteTerrainLayerIndex = LayerMask.NameToLayer("Terrain_Concrete");

        ////Setup Sounds and mixer routing
        
        //Concrete footsteps
        for (int i = 1; i <= NUM_CONCRETE_FOOTSTEPS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Footsteps/Concrete/Footstep_Concrete_ (" + i + ")"),
            "Footstep_Concrete_" + i,
            footstepsMixer.FindMatchingGroups("Master")[1]
            );
            _sounds.Add(s);
        }

        //Dirt footsteps
        for (int i = 1; i <= NUM_DIRT_FOOTSTEPS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Footsteps/Dirt/Footstep_Dirt_ (" + i + ")"),
            "Footstep_Dirt_" + i,
            footstepsMixer.FindMatchingGroups("Master")[2]
            ); _sounds.Add(s);
        }

        //Jump Start Dirt
        for (int i = 1; i <= NUM_DIRT_JUMP_STARTS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Footsteps/Jump_Start/Jump_Start_Dirt_"+ i),
            "Jump_Start_Dirt_" + i,
            footstepsMixer.FindMatchingGroups("Master")[3]
            ); _sounds.Add(s);
        }
        //

        //Jump Start Concrete
        for (int i = 1; i <= NUM_CONCRETE_JUMP_STARTS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Footsteps/Jump_Start/Jump_Start_Concrete_" + i),
            "Jump_Start_Concrete_" + i,
            footstepsMixer.FindMatchingGroups("Master")[3]
            ); _sounds.Add(s);
        }
        //

        //Jump Land Dirt
        for (int i = 1; i <= NUM_DIRT_JUMP_LANDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Footsteps/Jump_Land/Jump_Land_Dirt_" + i),
            "Jump_Land_Dirt_" + i,
            footstepsMixer.FindMatchingGroups("Master")[4]
            ); _sounds.Add(s);
        }
        //

        //Jump Land Concrete
        for (int i = 1; i <= NUM_CONCRETE_JUMP_LANDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Footsteps/Jump_Land/Jump_Land_Concrete_" + i),
            "Jump_Land_Concrete_" + i,
            footstepsMixer.FindMatchingGroups("Master")[4]
            ); _sounds.Add(s);
        }
        //

        ////

        //Add AudioSources
        _LeftFoot = _camRef.transform.Find("LeftFoot").gameObject.GetComponent<AudioSource>();
        _LeftFoot.panStereo = - _footSeperationWidthFromCenter;
        _sources.Add(_LeftFoot);

        _RightFoot = _camRef.transform.Find("RightFoot").gameObject.GetComponent<AudioSource>();
        _RightFoot.panStereo = _footSeperationWidthFromCenter;
        _sources.Add(_RightFoot);

        _CenterFoot = _camRef.transform.Find("CenterFoot").gameObject.GetComponent<AudioSource>();
        _sources.Add(_CenterFoot);
        //
    }

    void FixedUpdate()
    {
        //Raycast down to find terrain type
        RaycastHit hit;
        if (Physics.Raycast(transform.position,-transform.up,out hit,10f, 
                (1 << _dirtTerrainLayerIndex) | (1 << _concreteTerrainLayerIndex)))
        {
            if (hit.collider.gameObject.layer == _dirtTerrainLayerIndex)
            {
                //Hit Dirt
                _terrainType = TerrainType.Dirt;
            }

            else if (hit.collider.gameObject.layer == _concreteTerrainLayerIndex)
            {
                //Hit Concrete
                _terrainType = TerrainType.Concrete;
            }
        }
        //
    }

    private void Update()
    {
        //Handle falling
        if (CheckIfFalling())
        {
            _isFalling = true;

            //register max falling velocity (used for landing sound volume)
            _maxFallVelocity = Mathf.Max(Mathf.Abs(transform.position.y - _lastPosition.y), _maxFallVelocity);
        }
        if (CheckHasLanded() && _isFalling == true) //Fall Finished
        {
            _isFalling = false;
            Debug.Log(_maxFallVelocity);
            //Trigger a landing sound
            TriggerJump(true,
                QualityOfLifeFunctions.Scale
                    (0, 2f, //old scale
                    MIN_LANDING_VOLUME_SCALE, MAX_LANDING_VOLUME_SCALE, //new scale
                    Mathf.Min(_maxFallVelocity, 2f))); //value to scale

            //Reset fall velocity
            _maxFallVelocity = 0f;
        }
        //
    }

    void LateUpdate()
    {
        //Calculate distance moved
        if (_controllerRef.isGrounded)
        {
            _distanceMoved += Vector3.Distance(transform.position, _lastPosition);
        }
        //

        //Update Last Position
        _lastPosition = transform.position;
        //

        //Trigger Footstep
        if (_distanceMoved >= _distanceTillFootstepTriggered)
        {
            _distanceMoved = 0f;
            TriggerFootstep();
        }
        //
    }

    //Swaps foot from left to right when a footstep occurs
    void SwapFoot()
    {
        if (_footToggle == 0) { _footToggle = 1; return; }
        if (_footToggle == 1) { _footToggle = 0; return; }
    }

    bool CheckIfFalling()
    {
        return ((transform.position.y - _lastPosition.y) < -0.2f);
    }

    private bool CheckHasLanded()
    {
        return Mathf.Abs(transform.position.y - _lastPosition.y) < .05f;
    }

    const int CONCRETE_FOOTSTEP_INDEX_START = 0;
    const int CONCRETE_FOOTSTEP_INDEX_END = 9;
    //Picks a random Concrete footstep index, (never the same one in a row)
    int PickRandomConcreteFootstep()
    {
        int i;
        do
        {
            i = Random.Range(CONCRETE_FOOTSTEP_INDEX_START, CONCRETE_FOOTSTEP_INDEX_END+1);
        } while (i == _lastFootstepIndex);
        _lastFootstepIndex = i;

        return i;
    }

    const int DIRT_FOOTSTEP_INDEX_START = 10;
    const int DIRT_FOOTSTEP_INDEX_END = 41;
    //Picks a random Dirt footstep index, (never the same one in a row)
    int PickRandomDirtFootstep()
    {
        int i;
        do
        {
            i = Random.Range(DIRT_FOOTSTEP_INDEX_START, DIRT_FOOTSTEP_INDEX_END+1);
        } while (i == _lastFootstepIndex);
        _lastFootstepIndex = i;

        return i;
    }

    const int DIRT_JUMP_START_INDEX_START = 42;
    const int DIRT_JUMP_START_INDEX_END = 43;
    //Picks a random Dirt Jump Start index, (never the same one in a row)
    int PickRandomDirtJumpStart()
    {
        int i;
        do
        {
            i = Random.Range(DIRT_JUMP_START_INDEX_START, DIRT_JUMP_START_INDEX_END+1);
        } while (i == _lastFootstepIndex);
        _lastFootstepIndex = i;

        return i;
    }

    const int CONCRETE_JUMP_START_INDEX_START = 44;
    const int CONCRETE_JUMP_START_INDEX_END = 46;
    //Picks a random Concrete Jump Start index, (never the same one in a row)
    int PickRandomConcreteJumpStart()
    {
        int i;
        do
        {
            i = Random.Range(CONCRETE_JUMP_START_INDEX_START, CONCRETE_JUMP_START_INDEX_END+1);
        } while (i == _lastFootstepIndex);
        _lastFootstepIndex = i;

        return i;
    }

    const int DIRT_JUMP_LAND_INDEX_START = 47;
    const int DIRT_JUMP_LAND_INDEX_END = 52;
    //Picks a random Dirt Jump Land index, (never the same one in a row)
    int PickRandomDirtJumpLand()
    {
        int i;
        do
        {
            i = Random.Range(DIRT_JUMP_LAND_INDEX_START, DIRT_JUMP_LAND_INDEX_END+1);
        } while (i == _lastFootstepIndex);
        _lastFootstepIndex = i;

        return i;
    }

    const int CONCRETE_JUMP_LAND_INDEX_START = 53;
    const int CONCRETE_JUMP_LAND_INDEX_END = 58;
    //Picks a random Concrete Jump Land index, (never the same one in a row)
    int PickRandomConcreteJumpLand()
    {
        int i;
        do
        {
            i = Random.Range(CONCRETE_JUMP_LAND_INDEX_START, CONCRETE_JUMP_LAND_INDEX_END+1);
        } while (i == _lastFootstepIndex);
        _lastFootstepIndex = i;

        return i;
    }

    void TriggerFootstep()
    {
        //Get Footstep Index
        int i = 0;
        switch (_terrainType)
        {
            case TerrainType.Dirt:
                i = PickRandomDirtFootstep();break;

            case TerrainType.Concrete:
                i = PickRandomConcreteFootstep();break;
        }
        //

        //Route source to proper mixer group
        _sources[_footToggle].outputAudioMixerGroup = _sounds[i]._mixerGroup;

        //Randomize volume and pitch slightly
        float randVolumeScale = Random.Range(0.8f, 1.2f);
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _sources[_footToggle].pitch += randomPitchScale;

        //Play footstep
        _sources[_footToggle].PlayOneShot(_sounds[i]._clip, randVolumeScale);

        //Reset initial pitch
        _sources[_footToggle].pitch = 1.0f;

        //Swap foot for next trigger
        SwapFoot();
    }

    public void TriggerJump(bool isLanding, float volumeScale = 1f)
    {
        //Get Footstep Index
        int i = 0;
        switch (_terrainType)
        {
            case TerrainType.Dirt:
                if (isLanding) i = PickRandomDirtJumpLand();
                else { i = PickRandomDirtJumpStart(); }
                break;

            case TerrainType.Concrete:
                if (isLanding) i = PickRandomConcreteJumpLand();
                else { i = PickRandomConcreteJumpStart(); }
                break;
        }
        //

        //Route source to proper mixer group
        _sources[2].outputAudioMixerGroup = _sounds[i]._mixerGroup;

        //Randomize pitch slightly
        float randomPitchScale = Random.Range(-0.3f, 0.3f);
        _sources[2].pitch += randomPitchScale;

        //Play jump sound
        _sources[2].PlayOneShot(_sounds[i]._clip, volumeScale);

        //Reset initial pitch
        _sources[2].pitch = 1.0f;
    }
}
