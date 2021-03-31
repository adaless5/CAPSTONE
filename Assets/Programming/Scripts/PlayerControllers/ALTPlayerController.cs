﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public enum ControllerType
{
    Mouse,
    Controller,
}

public class ALTPlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Grappling,
        GrappleDeployed,
    }

    public enum ControllerState
    {
        Play,
        Menu,
        Wheel,
        Debug
    }

    public PlayerState m_PlayerState { get; private set; }
    public ControllerState m_ControllerState;
    public ControllerType m_ControllerType;

    public PauseMenuUI _pauseMenu;

    public Camera _camera;
    public float m_LookSensitivity = 10f;
    private float m_XRotation = 0f;

    CameraBehaviour _cameraBehaviour;

    public CharacterController _controller;

    // == Player Movement Variables ==
    Vector3 m_Velocity;
    float m_MoveSpeed = 10.0f;
    const float WALK_SPEED = 10.0f;
    const float SPRINT_SPEED = 20.0f;
    Vector3 m_Gravity = new Vector3(0.0f, -40.0f, 0.0f);
    Vector3 ORIGINAL_GRAVITY = new Vector3(0.0f, -40.0f, 0.0f);
    float m_JumpHeight = 5f;
    const float NORMAL_JUMP_HEIGHT = 5f;
    const float GRAPPLE_JUMP_HEIGHT = 10.0f;
    public Vector3 m_Momentum = Vector3.zero;
    float _slopeAngle;
    Vector3 _slopeAcceleration;
    [SerializeField] private float _slopeForce;
    [SerializeField] private float _slopeForceRayLength;
    float _Acceleration = 0f;
    bool bisGrounded;
    Transform _groundCheck;
    float _groundDistance = 0.3f;
    bool bcanJump;
    bool bDidJump;
    bool bIsMoving;
    Vector3 _preJumpVelocity;
    Vector3 _lastMoveVelocity;
    Vector3 _jumpVelocity;
    const float IN_AIR_CONTROL = 0.9f;
    const float MIN_SLOPE_ANGLE = 30.0f;
    const float DECELERATION_SPEED = 10.0f;
    float _coyoteTime = 2.1f;
    const float MAX_COYOTE_TIME = 0.1f;
    public bool bWasGrappling = false;

    public int m_UpgradeCurrencyAmount = 0;

    public Belt _equipmentBelt;
    public Belt _weaponBelt;

    public Health m_health;
    public Armor m_armor;
    public Stamina m_stamina;

    public Canvas EquipmentWheel;
    public Canvas WeaponWheel;

    public event Action<float> OnTakeDamage;
    public event Action<float> OnHeal;
    public event Action OnDeath;

    bool bIsInThermalView = false;
    bool bIsInDarknessVolume = false;

    string[] _controllerNames;
    float joyX;
    float joyY;
    int _equipIndex;
    int _wepIndex;

    Button[] _equipButtons;
    Button[] _wepButtons;

    float joyAngle;

    bool isSelected = false;

    bool bOnSlope = false;
    Vector3 _ControllerCollisionPos = Vector3.zero;

    //bool bHasEnteredDarkness = false;

    //Death mechanic stuff
    public bool isDead = false;
    Vector3 _respawnPosition;

    //New Controller
    public PlayerControls _controls;
    Vector2 _movement;
    Vector2 _look;
    bool _bIsShooting;
    bool _bIsRunning;
    bool _bPaused;
    bool _bIsJumping;

    //Special case: These guys needed some re-routing to make sure they didn't collude with the Pause logic 
    bool _bEquipWheel = true;
    bool _bWepWheel = true;

    bool _bEquipment;
    bool _bThermal;
    bool _bInteract;

    DebugController debugController;

    public static ALTPlayerController instance;

    private bool bInvertXAxis = false;
    private bool bInvertYAxis = false;

    bool bDebug = false;

    private void Awake()
    {
        OnTakeDamage += TakeDamage;
        EventBroker.OnPlayerDeath += PlayerDeath;
        _respawnPosition = gameObject.transform.position;
        m_ControllerState = ControllerState.Play;
        InitializeControls();
        _controller = GetComponent<CharacterController>();
        _cameraBehaviour = GetComponent<CameraBehaviour>();
        instance = this;
        _groundCheck = GameObject.Find("GroundCheck").transform;
    }
    #region Debug Functions
    public void DebugUnlockAllWeapons()
    {
        for (int i = 0; i < _weaponBelt._items.Length; i++)
        {
            _weaponBelt._items[i].ObtainEquipment();
        }
    }

    public void DebugUnlockAllTools()
    {
        for (int i = 0; i < _weaponBelt._items.Length; i++)
        {
            _equipmentBelt._items[i].ObtainEquipment();
        }
    }

    public void DebugGainCurrency()
    {
        m_UpgradeCurrencyAmount += 10;
    }
    #endregion

    void InitializeControls()
    {
        _controls = new PlayerControls();
        _controls.Player.Move.performed += ctx => _movement = ctx.ReadValue<Vector2>();
        _controls.Player.Move.canceled += ctx => _movement = Vector2.zero;
        _controls.Player.Camera.performed += ctx => _look = ctx.ReadValue<Vector2>();
        _controls.Player.Camera.canceled += ctx => _look = Vector2.zero;
        _controls.Player.Shoot.performed += ctx => _bIsShooting = true;
        _controls.Player.Shoot.canceled += ctx => _bIsShooting = false;
        _controls.Player.Sprint.performed += ctx => _bIsRunning = true;
        _controls.Player.Sprint.canceled += ctx => _bIsRunning = false;
        _controls.Player.Pause.performed += ctx => PlayerPause();
        _controls.Player.Jump.performed += ctx => _bIsJumping = true;
        _controls.Player.Jump.canceled += ctx => _bIsJumping = false;
        _controls.Player.EquipmentWheel.performed += ctx => HandleEquipmentWheel();
        _controls.Player.EquipmentWheel.canceled += ctx => HandleEquipmentWheel();
        _controls.Player.WeaponWheel.performed += ctx => HandleWeaponWheel();
        _controls.Player.WeaponWheel.canceled += ctx => HandleWeaponWheel();
        _controls.Player.Equipment.performed += ctx => _bEquipment = true;
        _controls.Player.Equipment.canceled += ctx => _bEquipment = false;
        _controls.Player.Thermal.started += ctx => _bThermal = true;
        _controls.Player.Thermal.canceled += ctx => _bThermal = false;
        _controls.Player.Interact.started += ctx => _bInteract = true;
        _controls.Player.Interact.canceled += ctx => _bInteract = false;
    }

    void Start()
    {
        //Debug.Log("Start");
        DontDestroyOnLoad(this);
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;

        _controller = GetComponent<CharacterController>();

        //Initializing Members
        m_health = GetComponent<Health>();
        m_armor = GetComponent<Armor>();
        m_stamina = GetComponent<Stamina>();
        _equipmentBelt = FindObjectOfType<EquipmentBelt>();
        _weaponBelt = FindObjectOfType<WeaponBelt>();
        _pauseMenu = FindObjectOfType<PauseMenuUI>();
        isDead = false;

        Canvas[] wheelsInScene;
        wheelsInScene = FindObjectsOfType<Canvas>();
        foreach (Canvas obj in wheelsInScene)
        {
            if (obj.tag == "EquipmentWheel")
            {
                EquipmentWheel = obj;
                EquipmentWheel.enabled = false;
            }
            else if (obj.tag == "WeaponWheel")
            {
                WeaponWheel = obj;
                WeaponWheel.enabled = false;
            }
        }
        SceneManager.sceneLoaded += PlayerSceneChange;
        //Subscribing to Event Broker
        EventBroker.CallOnPlayerSpawned(gameObject);
        OnTakeDamage += m_armor.ResetArmorTimer;


        Cursor.lockState = CursorLockMode.Locked;

        _equipButtons = _equipmentBelt.GetComponentsInChildren<Button>();
        _wepButtons = _weaponBelt.GetComponentsInChildren<Button>();
    }

    private void PlayerSceneChange(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log("Scene Changed");
        if (this != null)
            EventBroker.CallOnPlayerSpawned(gameObject);

    }

    void Update()
    {
        float dist = 100.0f;
        Vector3 dir = _ControllerCollisionPos - transform.position;
        Vector3 downdir = new Vector3(0.0f, -1.0f, 0.0f);
        RaycastHit hit;

        switch (m_ControllerState)
        {
            case ControllerState.Play:
                PlayerRotation();
                PlayerMovement();
                if (_cameraBehaviour != null)
                    _cameraBehaviour.SetIsInMenu(false);
                break;

            case ControllerState.Menu:
                if (_cameraBehaviour != null)
                    _cameraBehaviour.SetIsInMenu(true);
                break;

            case ControllerState.Wheel:
                PlayerMovement();
                break;
        }

        if (EquipmentWheel.enabled == true)
        {
            joyX = _look.x;
            joyY = _look.y;
            if (Gamepad.current != null)
            {
                if (Gamepad.current.rightStick.IsActuated())
                {
                    joyAngle = Mathf.Atan2(joyX, joyY) * Mathf.Rad2Deg;
                    Debug.Log("Joy Angle: " + joyAngle);
                    if (joyAngle > -90.0f && joyAngle < -45.0f)
                    {
                        _equipIndex = 0;
                    }
                    if (joyAngle > 0.0f && joyAngle < 90.0f)
                    {
                        _equipIndex = 1;
                    }
                    EventSystem.current.SetSelectedGameObject(_equipButtons[_equipIndex].gameObject);
                    _equipmentBelt.EquipToolAtIndex(_equipIndex);
                }
            }
        }

        if (WeaponWheel.enabled == true)
        {
            //EventSystem.current.SetSelectedGameObject(null);
            joyX = _look.x;
            joyY = _look.y;

            {
                if (Gamepad.current != null)
                {
                    if (Gamepad.current.rightStick.IsActuated())
                    {
                        joyAngle = Mathf.Atan2(joyX, joyY) * Mathf.Rad2Deg;
                        Debug.Log(joyAngle);
                        if (joyAngle > -90.0f && joyAngle < -45.0f)
                        {
                            _wepIndex = 1;

                        }
                        if (joyAngle > -45.0f && joyAngle < 0.0f)
                        {
                            _wepIndex = 0;
                        }
                        if (joyAngle > 0.0f && joyAngle < 90.0f)
                        {
                            _wepIndex = 2;
                        }
                        Debug.Log(_wepIndex);
                        EventSystem.current.SetSelectedGameObject(_wepButtons[_wepIndex].gameObject);
                        _weaponBelt.EquipToolAtIndex(_wepIndex);
                    }
                }
            }
        }

    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {

    }

    private void PlayerPause()
    {
        m_ControllerState = ControllerState.Menu;
        _pauseMenu.Pause();
    }

    //Death and Respawn functionality -LCC
    public void PlayerRespawn()
    {
        m_health.Heal(m_health.GetMaxHealth());
        m_armor.ResetArmor();
        isDead = false;
        m_ControllerState = ControllerState.Play;
        _controller.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.transform.rotation = Quaternion.identity;
    }

    void PlayerDeath()
    {
        isDead = true;
        StartCoroutine(DeathAnimation());
        m_ControllerState = ControllerState.Menu;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (_controller != null)
        {
            Debug.Log("Controller vibe passed");
            _controller.enabled = false;
        }
    }

    public IEnumerator DeathAnimation()
    {
        Quaternion deathRotation = Quaternion.Euler(new Vector3(0, 0, 100));
        while (gameObject.transform.rotation != deathRotation)
        {
            gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 100)), Time.deltaTime * 5.0f);
            yield return new WaitForSeconds(0.03f * Time.deltaTime);
        }
        yield return null;
    }

    private void CursorVisibility()
    {
        Cursor.visible = true;
    }

    private void TakeDamage(float damage)
    {
        if (m_armor.GetCurrentArmor() > 0)
        {
            m_armor.TakeDamage(damage);
            if (m_armor.GetCurrentArmor() > 0)
                GetComponentInChildren<VisorHitEffects>().ShieldHit();
            else
                GetComponentInChildren<VisorHitEffects>().ShieldBreak();

        }
        else
        {
            GetComponentInChildren<VisorHitEffects>().HealthHit();
            m_health.TakeDamage(damage);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _ControllerCollisionPos = hit.point;
    }

    public bool CheckForInteract()
    {
        return _bInteract;
    }

    public bool CheckForJumpInput()
    {
        return _bIsJumping;
    }

    public bool CheckForEquipWheelInput()
    {
        return _bEquipWheel;
    }

    public bool CheckForWepWheelInput()
    {
        return _bWepWheel;
    }

    public bool CheckForUseEquipmentInput()
    {
        return _bEquipment;
    }

    public bool CheckForUseThermalInput()
    {
        return _bThermal;
    }

    public bool CheckForUseWeaponInput()
    {
        return _bIsShooting;
    }

    public bool CheckForSprintInput()
    {
        return _bIsRunning;
    }

    public void PlayerRotation()
    {
        float mouseX = _look.x * Time.deltaTime * m_LookSensitivity;
        float mouseY = _look.y * Time.deltaTime * m_LookSensitivity;

        if (bInvertXAxis)
            mouseX *= -1f;

        if (bInvertYAxis)
            mouseY *= -1f;

        if (m_ControllerState == ControllerState.Play)
        {
            m_XRotation += mouseY;
            m_XRotation = Mathf.Clamp(m_XRotation, -90.0f, 90.0f);
            _camera.transform.localRotation = Quaternion.Euler(-m_XRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }

    public void PlayerMovement()
    {
        int triggermask = 1 << 2;
        int playermask = 1 << 9;

        RaycastHit[] allHits;
        allHits = Physics.SphereCastAll(transform.position, 0.5f, Vector3.down, _controller.height, ~(playermask | triggermask));
        RaycastHit nearestHit = new RaycastHit();

        //Collecting all collisions from spherecast and establishing the nearest collision as the active one.
        if (allHits.Length > 0)
        {
            float dist = Vector3.Distance(transform.position, allHits[0].point);
            foreach (RaycastHit hit in allHits)
            {
                if (Vector3.Distance(transform.position, hit.point) <= dist)
                {
                    nearestHit = hit;
                }
            }

            _slopeAngle = Mathf.Rad2Deg * Mathf.Asin(nearestHit.normal.y);
            _slopeAcceleration = Vector3.zero;

            if (_slopeAngle < MIN_SLOPE_ANGLE)
            {
                //This allows us to find the vector of the ground tangent that we can add to gravity to force the player down the slope.
                Vector3 groundTangent = Vector3.Cross(nearestHit.normal, Vector3.up);
                _slopeAcceleration = Vector3.Cross(nearestHit.normal, groundTangent);

                #region Drawing Debug Rays for normal and ground tangent.
                if (bDebug)
                {
                    Debug.DrawRay(nearestHit.point, nearestHit.normal, Color.cyan);
                    Debug.DrawRay(transform.position, _slopeAcceleration, Color.green);
                }
                #endregion

                m_Gravity += _slopeAcceleration;
                bOnSlope = true;
            }
            else
            {
                m_Gravity = ORIGINAL_GRAVITY;
            }
        }
        else
        {
            m_Gravity = ORIGINAL_GRAVITY;
        }

        //Checking if player is grounded
        bisGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, ~(playermask | triggermask));

        #region Vector Debugs
        if (bDebug)
        {
            print("Velocity = " + m_Velocity);
            print("Gravity = " + m_Gravity);
            print("SlopeAcceleration = " + _slopeAcceleration);
        }
        #endregion

        //Calculating Acceleration
        if (_movement.magnitude > 0f)
        {
            _Acceleration += Time.deltaTime;
        }
        else
        {
            _Acceleration -= Time.deltaTime;
        }
        _Acceleration = Mathf.Clamp(_Acceleration, 0.0f, 1.0f);

        //Forcing Player onto ground
        if (bisGrounded && m_Velocity.y < 0f)
        {
            m_Velocity.y = 0.0f;

            if (bWasGrappling)
            {
                bWasGrappling = false;
            }
        }

        //Changing movement speed based on sprint input
        if (CheckForSprintInput() && m_stamina.GetCurrentStamina() > 0 && _movement.magnitude > 0)
        {
            m_MoveSpeed = Mathf.Lerp(m_MoveSpeed, SPRINT_SPEED, Time.deltaTime);
            m_stamina.UseStamina();
            GetComponent<CameraBehaviour>().bobFrequency = 7.5f;
        }
        else
        {
            m_MoveSpeed = Mathf.Lerp(m_MoveSpeed, WALK_SPEED, Time.deltaTime);
            GetComponent<CameraBehaviour>().bobFrequency = 5.0f;
        }

        //Regenerate stamina if player isn't sprinting
        if (CheckForSprintInput() == false || (_movement.magnitude == 0 && m_stamina.bCanRegenerate))
            m_stamina.StartCoroutine(m_stamina.RegenerateStamina());

        //Using Player Input to Calculate movement vector and applying movement
        Vector3 movement = ((transform.right * _movement.x) + (transform.forward * _movement.y)) * _Acceleration;

        //Store the last recorded movement velocity for deceleration
        if (movement.magnitude > Mathf.Epsilon)
        {
            bIsMoving = true;
            _lastMoveVelocity = movement;

            //TODO: Figure out Why Camera behaviour is bugging out.
            //if (!_bIsJumping)
            //{
            //    if (_cameraBehaviour != null)
            //        _cameraBehaviour.SetIsWalking(true);
            //}
        }
        else
        {
            bIsMoving = false;

            //TODO: Figure out Why Camera behaviour is bugging out.
            //if (_cameraBehaviour != null)
            //    _cameraBehaviour.SetIsWalking(false);
        }

        if (!bDidJump)
        {
            //If Moving use input, otherwise use the last recorded movement input and decelerate to zero
            if (bIsMoving)
                _controller.Move(movement * Time.deltaTime * m_MoveSpeed);
            else
            {
                _lastMoveVelocity = Vector3.Lerp(_lastMoveVelocity, Vector3.zero, Time.deltaTime * DECELERATION_SPEED);
                _controller.Move(_lastMoveVelocity * Time.deltaTime * m_MoveSpeed);
            }
        }

        //Resetting After Jump
        if (bisGrounded && !_bIsJumping || _coyoteTime < MAX_COYOTE_TIME)
        {
            if (!bcanJump)
            {
                m_Velocity = Vector3.zero;
                _preJumpVelocity = Vector3.zero;
                bcanJump = true;
                bDidJump = false;
            }
        }

        if (bcanJump)
        {
            if (CheckForJumpInput())
            {
                if (m_PlayerState != PlayerState.Grappling)
                {
                    if (bWasGrappling)
                        m_JumpHeight = GRAPPLE_JUMP_HEIGHT;
                    else
                        m_JumpHeight = NORMAL_JUMP_HEIGHT;

                    _preJumpVelocity = movement;
                    m_Velocity = _preJumpVelocity;
                    _jumpVelocity.y = Mathf.Sqrt(m_JumpHeight * -2f * m_Gravity.y);
                    m_Velocity.y = _jumpVelocity.y;
                }

                GetComponent<AudioManager_Footsteps>().TriggerJump(false);

                bcanJump = false;
                bDidJump = true;
                _coyoteTime = MAX_COYOTE_TIME + 1f;
            }
        }

        //Preventing you from hanging in air if you jump up into something
        if (_controller.collisionFlags.ToString() == "Above")
        {
            m_Velocity.y = Mathf.Lerp(m_Velocity.y, -1f, Time.deltaTime * 20.0f);
        }

        if (!bisGrounded && bDidJump)
        {
            //TODO: This may need tweaking.
            Vector3 airMovement = (_preJumpVelocity + (movement * IN_AIR_CONTROL)) * m_MoveSpeed;

            if (bWasGrappling)
            {
                airMovement = movement * m_MoveSpeed;
            }

            airMovement = Vector3.ClampMagnitude(airMovement, 10.0f);

            _controller.Move(airMovement * Time.deltaTime);

            if (bDidJump && !CheckForJumpInput() && m_Velocity.y > 0.0f)
            {
                if (m_Velocity.y > 10.0f)
                {
                    if (m_Velocity.y > 0.01f)
                    {
                        m_Velocity.y *= 0.5f;
                    }
                    else
                    {
                        m_Velocity.y = 0.0f;
                    }
                }
            }
        }

        if (!bisGrounded && !bDidJump && m_PlayerState != PlayerState.Grappling)
        {
            _coyoteTime = 0.0f;
            _coyoteTime += Time.deltaTime;
        }

        //Calculate Gravity and Apply Grav to movement
        if (m_PlayerState != PlayerState.Grappling)
            m_Velocity += m_Gravity * Time.deltaTime;

        m_Velocity += m_Momentum * Time.deltaTime;

        //Handling Grappling Hook Momentum
        //TODO: Reapproach momentum to make it feel more satisfying.
        if (m_Momentum.magnitude >= 0f)
        {
            if (!bisGrounded)
            {
                float drag = 5f;
                m_Momentum -= m_Momentum * drag * Time.deltaTime;
            }
            else if (bisGrounded)
            {
                m_Momentum = Vector3.zero;
            }
        }

        if (bisGrounded && bOnSlope)
        {
            bOnSlope = false;
            m_Gravity.x = 0.0f;
            m_Gravity.z = 0.0f;
            m_Velocity.x = 0.0f;
            m_Velocity.z = 0.0f;
            _slopeAcceleration = Vector3.zero;
        }

        //TODO: Test this for edge cases where having a gravity cap feels bad.
        m_Velocity.y = Mathf.Clamp(m_Velocity.y, -60.0f, 20.0f);

        if (bOnSlope || m_PlayerState == PlayerState.Grappling)
        {
            _preJumpVelocity = Vector3.zero;
        }

        _controller.Move(m_Velocity * Time.deltaTime);

        //Preventing player from bouncing while walking down slope.
        if (bisGrounded && !_bIsJumping)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, _controller.height / 2 * _slopeForceRayLength))
                if (hit.normal != Vector3.up)
                    _controller.Move(Vector3.down * _controller.height / 2 * _slopeForce * Time.deltaTime);
        }

        #region Grounded Debug
        if (bDebug)
        {
            print("COYOTE TIME: " + _coyoteTime);
            print("Grounded = " + bisGrounded);
        }
        #endregion
    }

    
    private bool OnWalkableSlope()
    {
        if (_bIsJumping)
            return false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, _controller.height / 2 * _slopeForceRayLength))
            if (hit.normal != Vector3.up)
                return true;
        return false;
    }

    void HandleEquipmentWheel()
    {
        if (WeaponWheel.enabled == false)
        {
            if (_bEquipWheel)
            {
                _bEquipWheel = false;
                EquipmentWheel.enabled = true;
                Time.timeScale = 0.3f;
                EquipmentWheel.GetComponent<CanvasGroup>().interactable = true;
                EquipmentWheel.GetComponent<CanvasGroup>().blocksRaycasts = true;
                Cursor.lockState = CursorLockMode.None;
                m_ControllerState = ControllerState.Wheel;
                return;
            }

            if (_bEquipWheel == false)
            {
                _bEquipWheel = true;
                EquipmentWheel.enabled = false;
                Time.timeScale = 1;
                EquipmentWheel.GetComponent<CanvasGroup>().interactable = false;
                EquipmentWheel.GetComponent<CanvasGroup>().blocksRaycasts = false;
                Cursor.lockState = CursorLockMode.Locked;
                m_ControllerState = ControllerState.Play;
                return;
            }
        }
    }

    public void HandleWeaponWheel()
    {
        if (EquipmentWheel.enabled == false)
        {
            if (_bWepWheel)
            {
                _bWepWheel = false;
                WeaponWheel.enabled = true;
                Time.timeScale = 0.3f;
                WeaponWheel.GetComponent<CanvasGroup>().interactable = true;
                WeaponWheel.GetComponent<CanvasGroup>().blocksRaycasts = true;
                Cursor.lockState = CursorLockMode.None;
                m_ControllerState = ControllerState.Wheel;
                return;
            }

            if (_bWepWheel == false)
            {
                _bWepWheel = true;
                WeaponWheel.enabled = false;
                Time.timeScale = 1;
                WeaponWheel.GetComponent<CanvasGroup>().interactable = false;
                WeaponWheel.GetComponent<CanvasGroup>().blocksRaycasts = false;
                Cursor.lockState = CursorLockMode.Locked;
                m_ControllerState = ControllerState.Play;
                return;
            }
        }
    }

    public void ResetGravity()
    {
        m_Velocity.y = 0.0f;
    }

    public void ApplyMomentum(Vector3 direction, float speed, float grappleMultiplier, float jumpMultiplier)
    {
        m_Momentum = direction * grappleMultiplier;
        bcanJump = true;
    }

    public void ChangePlayerState(PlayerState newPlayerState)
    {
        m_PlayerState = newPlayerState;
    }

    public void CallOnTakeDamage(float damageToTake)
    {
        OnTakeDamage?.Invoke(damageToTake);
    }

    public void CallOnDeath()
    {
        OnDeath?.Invoke();
    }

    public void CallOnHeal(float healthToHeal)
    {
        OnHeal?.Invoke(healthToHeal);
    }

    public void SetThermalView(bool isThermal)
    {
        bIsInThermalView = isThermal;
    }

    public bool GetThermalView()
    {
        return bIsInThermalView;
    }

    public void SetDarknessVolume(bool isDark)
    {
        bIsInDarknessVolume = isDark;
    }

    public bool GetDarknessVolume()
    {
        return bIsInDarknessVolume;
    }

    public ControllerState GetControllerState()
    {
        return m_ControllerState;
    }

    public Vector3 GetVelocity()
    {
        return m_Velocity;
    }

    public void SetXAxisInvert()
    {
        bInvertXAxis = !bInvertXAxis;
    }

    public void SetYAxisInvert()
    {
        bInvertYAxis = !bInvertYAxis;
    }
}
