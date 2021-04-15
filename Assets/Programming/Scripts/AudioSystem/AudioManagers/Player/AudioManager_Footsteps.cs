using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager_Footsteps : AudioManager
{
    const int NUM_CONCRETE_FOOTSTEPS = 10;
    const int NUM_DIRT_FOOTSTEPS = 32;
    const int NUM_DIRT_JUMP_STARTS = 6;
    const int NUM_CONCRETE_JUMP_STARTS = 6;
    const int NUM_DIRT_JUMP_LANDS = 6;
    const int NUM_CONCRETE_JUMP_LANDS = 6;
    const int NUM_METAL_FOOTSTEPS = 8;

    const float MIN_LANDING_VOLUME_SCALE = 1.0f;
    const float MAX_LANDING_VOLUME_SCALE = 2f;

    enum TerrainType
    {
        Dirt,
        Concrete,
        Metal,
    }
    TerrainType _terrainType = TerrainType.Dirt;

    AudioSource _LeftFoot;
    AudioSource _RightFoot;
    AudioSource _CenterFoot;

    Camera _camRef;
    CharacterController _controllerRef;
    ALTPlayerController _playerControllerRef;

    Vector3 _lastPosition;
    float _distanceMoved = 0f;

    int _footToggle = 0;
    int _lastFootstepIndex = -1;
    int _lastJumpStartIndex = -1;
    int _lastJumpLandIndex = -1;

    float _footSeperationWidthFromCenter = 0.2f;
    float _distanceTillFootstepTriggered = 6.5f;

    int _dirtTerrainLayerIndex;
    int _concreteTerrainLayerIndex;
    int _metalTerrainLayerIndex;

    bool _isFalling = false;
    public bool _isJumping = false;
    float _maxFallVelocity = 0;

    public override void Initialize()
    {
        //Fetch Mixer
        AudioMixer footstepsMixer = Resources.Load<AudioMixer>("Data/AudioData/Mixers/Footsteps");

        //Fetch references
        _camRef = GetComponentInChildren<Camera>();
        _controllerRef = GetComponent<CharacterController>();
        _playerControllerRef = GetComponent<ALTPlayerController>();
        _lastPosition = gameObject.transform.position;
        _dirtTerrainLayerIndex = LayerMask.NameToLayer("Terrain_Dirt");
        _concreteTerrainLayerIndex = LayerMask.NameToLayer("Terrain_Concrete");
        _metalTerrainLayerIndex = LayerMask.NameToLayer("Terrain_Metal");

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
            footstepsMixer.FindMatchingGroups("Master")[4]
            ); _sounds.Add(s);
        }
        //

        //Jump Start Concrete
        for (int i = 1; i <= NUM_CONCRETE_JUMP_STARTS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Footsteps/Jump_Start/Jump_Start_Concrete_" + i),
            "Jump_Start_Concrete_" + i,
            footstepsMixer.FindMatchingGroups("Master")[4]
            ); _sounds.Add(s);
        }
        //

        //Jump Land Dirt
        for (int i = 1; i <= NUM_DIRT_JUMP_LANDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Footsteps/Jump_Land/Jump_Land_Dirt_" + i),
            "Jump_Land_Dirt_" + i,
            footstepsMixer.FindMatchingGroups("Master")[5]
            ); _sounds.Add(s);
        }
        //

        //Jump Land Concrete
        for (int i = 1; i <= NUM_CONCRETE_JUMP_LANDS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Footsteps/Jump_Land/Jump_Land_Concrete_" + i),
            "Jump_Land_Concrete_" + i,
            footstepsMixer.FindMatchingGroups("Master")[5]
            ); _sounds.Add(s);
        }
        //

        //Metal footsteps
        for (int i = 1; i <= NUM_METAL_FOOTSTEPS; i++)
        {
            Sound s = new Sound(
            Resources.Load<AudioClip>("Data/AudioData/AudioClips/Footsteps/Metal/Footstep_Metal_" + i),
            "Footstep_Metal_" + i,
            footstepsMixer.FindMatchingGroups("Master")[3]
            ); _sounds.Add(s);
        }

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

    //Runs before update
    void FixedUpdate()
    {
        
    }

    private void Update()
    {

        //Raycast down to find terrain type
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10f,
                (1 << _dirtTerrainLayerIndex) | (1 << _concreteTerrainLayerIndex) | (1 << _metalTerrainLayerIndex)))
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

            else if (hit.collider.gameObject.layer == _metalTerrainLayerIndex)
            {
                //Hit Metal
                _terrainType = TerrainType.Metal;
            }

            else _terrainType = TerrainType.Dirt;
        }
        //

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
            _isJumping = false;
            
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

        _lastPosition = gameObject.transform.position;
    }

    float moveCounter = 0;
    public float maxMoveTillFootstep = 30;
    //runs after update
    void LateUpdate()
    {
        ////Calculate distance moved
        //if (_controllerRef.isGrounded)
        //{
        //    _distanceMoved += Vector3.Distance(transform.position, _lastPosition);
        //}
        ////

        ////Update Last Position
        //_lastPosition = transform.position;
        ////
        ///

 


        if (_playerControllerRef._movement != Vector2.zero)
        {
            if (_playerControllerRef._bIsRunning) moveCounter += 1.5f;
            else moveCounter++;
        }

        if (moveCounter >= maxMoveTillFootstep)
        {
            moveCounter = 0;
            if(_isJumping == false) TriggerFootstep();

        }

    }

    //Swaps foot from left to right when a footstep occurs
    void SwapFoot()
    {
        if (_footToggle == 0) { _footToggle = 1; return; }
        if (_footToggle == 1) { _footToggle = 0; return; }
    }

    bool CheckIfFalling()
    {
        return ((transform.position.y - _lastPosition.y) < -.2f);
    }

    bool IsJumping()
    {
        Debug.Log((transform.position.y - _lastPosition.y));
        return ((transform.position.y - _lastPosition.y) > .2f);
    }

    private bool CheckHasLanded()
    {
        return Mathf.Abs(transform.position.y - _lastPosition.y) < .1f;
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
    const int DIRT_JUMP_START_INDEX_END = 47;
    //Picks a random Dirt Jump Start index, (never the same one in a row)
    int PickRandomDirtJumpStart()
    {
        int i;
        do
        {
            i = Random.Range(DIRT_JUMP_START_INDEX_START, DIRT_JUMP_START_INDEX_END+1);
        } while (i == _lastJumpStartIndex);
        _lastJumpStartIndex = i;

        return i;
    }

    const int CONCRETE_JUMP_START_INDEX_START = 48;
    const int CONCRETE_JUMP_START_INDEX_END = 53;
    //Picks a random Concrete Jump Start index, (never the same one in a row)
    int PickRandomConcreteJumpStart()
    {
        int i;
        do
        {
            i = Random.Range(CONCRETE_JUMP_START_INDEX_START, CONCRETE_JUMP_START_INDEX_END+1);
        } while (i == _lastJumpStartIndex);
        _lastJumpStartIndex = i;

        return i;
    }

    const int DIRT_JUMP_LAND_INDEX_START = 54;
    const int DIRT_JUMP_LAND_INDEX_END = 59;
    //Picks a random Dirt Jump Land index, (never the same one in a row)
    int PickRandomDirtJumpLand()
    {
        int i;
        do
        {
            i = Random.Range(DIRT_JUMP_LAND_INDEX_START, DIRT_JUMP_LAND_INDEX_END+1);
        } while (i == _lastJumpLandIndex);
        _lastJumpLandIndex = i;

        return i;
    }

    const int CONCRETE_JUMP_LAND_INDEX_START = 60;
    const int CONCRETE_JUMP_LAND_INDEX_END = 65;
    //Picks a random Concrete Jump Land index, (never the same one in a row)
    int PickRandomConcreteJumpLand()
    {
        int i;
        do
        {
            i = Random.Range(CONCRETE_JUMP_LAND_INDEX_START, CONCRETE_JUMP_LAND_INDEX_END+1);
        } while (i == _lastJumpLandIndex);
        _lastJumpLandIndex = i;

        return i;
    }

    const int METAL_FOOTSTEP_INDEX_START = 66;
    const int METAL_FOOTSTEP_INDEX_END = 73;
    //Picks a random Metal Footstep index, (never the same one in a row)
    int PickRandomMetalFootstep()
    {
        int i;
        do
        {
            i = Random.Range(METAL_FOOTSTEP_INDEX_START, METAL_FOOTSTEP_INDEX_END + 1);
        } while (i == _lastFootstepIndex);
        _lastFootstepIndex = i;

        return i;
    }

    void TriggerFootstep()
    {
        //Stop Previous footstep
        StopPreviousFootstep();
        //

        //Get Footstep Index
        int i = 0;
        switch (_terrainType)
        {
            case TerrainType.Metal:
                i = PickRandomMetalFootstep(); break;

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

    private void StopPreviousFootstep()
    {
        if (_footToggle == 0) _sources[1].Stop();
        else if (_footToggle == 1) _sources[0].Stop();
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

            case TerrainType.Metal:
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
