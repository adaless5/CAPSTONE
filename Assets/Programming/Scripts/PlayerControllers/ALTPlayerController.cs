using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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
    }



    public PlayerState m_PlayerState { get; private set; }
    public ControllerState m_ControllerState;
    public ControllerType m_ControllerType;


    public PauseMenuUI _pauseMenu;

    public Camera _camera;
    public float m_LookSensitivity = 10f;
    private float m_XRotation = 0f;

    public CharacterController _controller;
    Vector3 m_Velocity;
    float m_YVelocity;
    float m_MoveSpeed = 10.0f;

    float m_SprintSpeed = 15.0f;
    public float m_Gravity = -50.0f;
    public float m_JumpHeight = 20.0f;

    public Vector3 m_Momentum { get; private set; } = Vector3.zero;

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

    private Vector3 _hitNormal;
    float _slopeLimit = 120.0f;
    float _slopeAngle;
    Vector3 _slopeAcceleration;
    float _slopeSpeed = 500.0f;

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

    const float POST_JUMP_FALL_MULTIPLIER = 60.0f;

    bool isSelected = false;

    bool bOnSlope = false;
    Vector3 _ControllerCollisionPos = Vector3.zero;

    //bool bHasEnteredDarkness = false;

    //Death mechanic stuff
    bool isDead = false;
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

    private void Awake()
    {
        OnTakeDamage += TakeDamage;
        _respawnPosition = gameObject.transform.position;
        m_ControllerState = ControllerState.Play;
        InitializeControls();
        _controller = GetComponent<CharacterController>();
    }

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
        _controls.Player.EquipmentWheel.performed += ctx =>HandleEquipmentWheel();
        _controls.Player.EquipmentWheel.canceled += ctx => HandleEquipmentWheel();
        _controls.Player.WeaponWheel.performed += ctx => HandleWeaponWheel();
        _controls.Player.WeaponWheel.canceled += ctx => HandleWeaponWheel();
        _controls.Player.Equipment.performed += ctx => _bEquipment = true;
        _controls.Player.Equipment.canceled += ctx => _bEquipment = false;
        _controls.Player.Thermal.performed += ctx => _bThermal = true;
        _controls.Player.Thermal.canceled += ctx => _bThermal = false;
        _controls.Player.Interact.performed += ctx => _bInteract = true;
        _controls.Player.Interact.canceled += ctx => _bInteract = false;
    }

    void Start()
    {
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

        //Subscribing to Event Broker
        EventBroker.CallOnPlayerSpawned(gameObject);
        OnTakeDamage += m_armor.ResetArmorTimer;
        EventBroker.OnPlayerDeath += PlayerDeath;

        Cursor.lockState = CursorLockMode.Locked;

        _equipButtons = _equipmentBelt.GetComponentsInChildren<Button>();
        _wepButtons = _weaponBelt.GetComponentsInChildren<Button>();
    }

    void Update()
    {
        float dist = 100.0f;
        Vector3 dir = _ControllerCollisionPos - transform.position;
        Vector3 downdir = new Vector3(0.0f, -1.0f, 0.0f);
        RaycastHit hit;

        //Debug.Log(m_ControllerState);
        //Debug.Log(Time.timeScale);

        switch (m_ControllerState)
        {
            case ControllerState.Play:
                PlayerRotation();
                PlayerMovement();
                break;

            case ControllerState.Menu:
                break;

            case ControllerState.Wheel:
                PlayerMovement();
                break;
        }


        //HandleEquipmentWheels();

        //if (EquipmentWheel.enabled == true)
        //{
        //    joyX = 0;
        //    joyY = 0;
        //    //if (m_ControllerType == ControllerType.Controller)
        //    //{



        //    //    joyAngle = Mathf.Atan2(joyX, joyY) * Mathf.Rad2Deg;
        //    //    Debug.Log("Joy Angle: " + joyAngle);
        //    //    if (joyAngle > -45.0f && joyAngle < 0.0f)
        //    //    {
        //    //        _equipIndex = 0;
        //    //    }
        //    //    if (joyAngle > 0.0f && joyAngle < 90.0f)
        //    //    {
        //    //        _equipIndex = 1;
        //    //    }
        //    //    EventSystem.current.SetSelectedGameObject(_equipButtons[_equipIndex].gameObject);
        //    //    _equipmentBelt.EquipToolAtIndex(_equipIndex);
        //    //}
        //}

        //if (WeaponWheel.enabled == true)
        //{
        //    //EventSystem.current.SetSelectedGameObject(null);
        //    joyX = 0;
        //    joyY = 0;

        //    {
        //        if (m_ControllerType == ControllerType.Controller)
        //        {
        //            //joyX += Input.GetAxis("Mouse X") * m_LookSensitivity;
        //            //joyY += Input.GetAxis("Mouse Y") * m_LookSensitivity;
        //            joyAngle = Mathf.Atan2(joyX, joyY) * Mathf.Rad2Deg;
        //            Debug.Log(joyAngle);
        //            if (joyAngle > -90.0f && joyAngle < -45.0f)
        //            {
        //                _wepIndex = 1;

        //            }
        //            if (joyAngle > -45.0f && joyAngle < 0.0f)
        //            {
        //                _wepIndex = 0;
        //            }
        //            if (joyAngle > 0.0f && joyAngle < 90.0f)
        //            {
        //                _wepIndex = 2;
        //            }
        //            Debug.Log(_wepIndex);
        //            EventSystem.current.SetSelectedGameObject(_wepButtons[_wepIndex].gameObject);
        //            _weaponBelt.EquipToolAtIndex(_wepIndex);
        //        }
        //    }
        //}

        //Below I am calculating a vector perpendincular to the surface the player is on to determine what direction to move while sliding.
        //Sliding activates when the angle between the surface normal and the down vector are <= 140 deg. -AD
        int slidemask = 1 << 17;
        if (Physics.Raycast(transform.position, dir, out hit, 100f, ~slidemask))
        {
            _hitNormal = hit.normal;

            _slopeAngle = Vector3.Angle(downdir * dist, hit.normal);

            _slopeAcceleration = transform.TransformDirection(m_Velocity);

            Vector3 groundTangent = _slopeAcceleration - Vector3.Project(_slopeAcceleration, hit.normal);

            groundTangent.Normalize();

            _slopeAcceleration = groundTangent;

            if (_controller.isGrounded && _slopeAngle <= 140.0f)
            {
                bOnSlope = true;

            }
            else
            {
                bOnSlope = false;
            }
        }

        m_Velocity.y = Mathf.Clamp(m_Velocity.y, -15.0f, 1000.0f);  //Clamping the minimum y velocity to prevent rapidly falling after sliding. -AD

        //Stand in death animation -LCC
        if (isDead)
            gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 100)), Time.deltaTime * 5.0f);
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
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
        isDead = false;
        m_ControllerState = ControllerState.Play;
        _controller.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.transform.position = _respawnPosition;
        gameObject.transform.rotation = Quaternion.identity;
    }

    void PlayerDeath()
    {
        if (_controller != null)
        {
            _controller.enabled = false;
            m_ControllerState = ControllerState.Menu;
            isDead = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    //private void ControllerCheck()
    //{
    //    _controllerNames = Input.GetJoystickNames();
    //    if (_controllerNames != null)
    //    {
    //        if (_controllerNames.Length > 0)
    //        {
    //            m_ControllerType = ControllerType.Controller;
    //        }
    //        else
    //        {
    //            m_ControllerType = ControllerType.Mouse;
    //        }
    //    }
    //}

    private void CursorVisibility()
    {
        Cursor.visible = true;
    }

    private void TakeDamage(float damage)
    {
        if (m_armor.GetCurrentArmor() > 0)
        {
            m_armor.TakeDamage(damage);
        }
        else
        {
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


        //Sprinting logic & use of player's stamina - VR
        if (CheckForSprintInput() && m_stamina.GetCurrentStamina() > 0 && _controller.velocity.magnitude > 0)
        {
            m_Velocity = (transform.right * _movement.x * m_SprintSpeed) + (transform.forward * _movement.y * m_SprintSpeed);
            m_stamina.UseStamina();
        }
        else
        {
            m_Velocity = (transform.right * _movement.x * m_MoveSpeed) + (transform.forward * _movement.y * m_MoveSpeed);
        }

        if (CheckForSprintInput() == false || (_controller.velocity.magnitude == 0 && m_stamina.bCanRegenerate))
        {
            m_stamina.StartCoroutine(m_stamina.RegenerateStamina());
        }

        if (!bOnSlope)
        {
            HandleJump();
        }


        ApplyGravity();

        m_Velocity += m_Momentum;

        //If player is on slope apply Vector parallel to ground to movement vector. 
        if (bOnSlope && m_PlayerState != PlayerState.Grappling)
        {
            m_Velocity.y += m_Gravity;
            m_Velocity = Vector3.Lerp(m_Velocity, _slopeAcceleration * _slopeSpeed, Time.deltaTime);
        }

        _controller.Move(m_Velocity * Time.deltaTime);



        if (m_Momentum.magnitude >= 0f)
        {
            if (!_controller.isGrounded)
            {
                float drag = 3f;
                m_Momentum -= m_Momentum * drag * Time.deltaTime;
            }
            else if (_controller.isGrounded)
            {
                m_Momentum = Vector3.zero;
            }
        }
    }

    void ApplyGravity()
    {
        m_YVelocity += m_Gravity * Time.deltaTime;

        m_Velocity.y = m_YVelocity;
    }

    void HandleJump()
    {
        if (m_ControllerState == ControllerState.Play)
        {
            if (_controller.isGrounded)
            {
                m_YVelocity = 0f;

                if (CheckForJumpInput())
                {
                    m_YVelocity = m_JumpHeight;
                    GetComponent<AudioManager_Footsteps>().TriggerJump(false);
                }
            }
            else if (!_controller.isGrounded)
            {
                if (_controller.collisionFlags.ToString() == "Above")
                {
                    m_YVelocity = -2.0f;
                }
                if (CheckForJumpInput() == false && m_YVelocity > 0.0f)
                {
                    if (m_YVelocity > 0.01f)
                    {
                        m_YVelocity *= 0.5f;
                    }
                    else
                    {
                        m_YVelocity = 0.0f;
                    }
                }
            }
        }
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
        m_YVelocity = 0.0f;
    }

    public void ApplyMomentum(Vector3 direction, float speed, float grappleMultiplier, float jumpMultiplier)
    {
        m_Momentum = direction * speed * grappleMultiplier;
        m_Momentum += Vector3.up * jumpMultiplier;
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
}
