using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;

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
    }

    public PlayerState m_PlayerState { get; private set; }
    public ControllerState m_ControllerState;
    public ControllerType m_ControllerType;


    public PauseMenuUI _pauseMenu;

    public Camera _camera;
    public float m_LookSensitivity = 100.0f;
    private float m_XRotation = 0f;

    public CharacterController _controller;
    Vector3 m_Velocity;
    float m_YVelocity;
    float m_MoveSpeed = 10.0f;

    float m_SprintSpeed = 15.0f;
    public float m_Gravity = -50.0f;
    public float m_JumpHeight = 20.0f;

    public Vector3 m_Momentum { get; private set; } = Vector3.zero;

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

   

    private void Awake()
    {        
        OnTakeDamage += TakeDamage;
    }

    void Start()
    {
        DontDestroyOnLoad(this);
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;

        //Initializing Members
        m_health = GetComponent<Health>();
        m_armor = GetComponent<Armor>();
        m_stamina = GetComponent<Stamina>();
        _equipmentBelt = FindObjectOfType<Belt>();
        _weaponBelt = FindObjectOfType<WeaponBelt>();


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

        ControllerCheck();

        switch (m_ControllerState)
        {
            case ControllerState.Play:
                PlayerRotation();
                PlayerMovement();
                break;

            case ControllerState.Menu:
                PlayerRotation();
                PlayerMovement();
                break;

        }

        if (Input.GetButtonDown("Pause"))
        {
            _pauseMenu.Pause();
        }

        HandleEquipmentWheels();


        if (EquipmentWheel.enabled == true)
        {
            EventSystem.current.SetSelectedGameObject(null);
            joyX = 0;
            joyY = 0;
            if (m_ControllerType == ControllerType.Controller)
            {

                joyX += Input.GetAxis("Mouse X") * m_LookSensitivity;
                joyY += Input.GetAxis("Mouse Y") * m_LookSensitivity;


                joyAngle = Mathf.Atan2(joyX, joyY) * Mathf.Rad2Deg;
                Debug.Log(joyAngle);
                if (joyAngle > -90.0f && joyAngle < -45.0f)
                {
                    _equipIndex = 1;
                }
                if (joyAngle > -45.0f && joyAngle < 0.0f)
                {
                    _equipIndex = 0;
                }
                if (joyAngle > 0.0f && joyAngle < 90.0f)
                {
                    _equipIndex = 2;
                }
                EventSystem.current.SetSelectedGameObject(_equipButtons[_equipIndex].gameObject);
            }
        }

        if (WeaponWheel.enabled == true)
        {
            //EventSystem.current.SetSelectedGameObject(null);
            joyX = 0;
            joyY = 0;

            {
                if (m_ControllerType == ControllerType.Controller)
                {
                    joyX += Input.GetAxis("Mouse X") * m_LookSensitivity;
                    joyY += Input.GetAxis("Mouse Y") * m_LookSensitivity;
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
                else if (m_ControllerType == ControllerType.Mouse)
                {

                }
            }
        }

        Debug.DrawRay(transform.position, dir);

        if (Physics.Raycast(transform.position,  dir, out hit))
        {
            _hitNormal = hit.normal;

            _slopeAngle = Vector3.Angle(downdir * dist, hit.normal);

            _slopeAcceleration = transform.TransformDirection(m_Velocity);

            Vector3 groundTangent = _slopeAcceleration - Vector3.Project(_slopeAcceleration, hit.normal);

            groundTangent.Normalize();

            _slopeAcceleration = groundTangent;

            if(_controller.isGrounded && _slopeAngle <= 140.0f )
            {
                bOnSlope = true;
            }
            else
            {
                bOnSlope = false;
            }
        }
    }

    private void ControllerCheck()
    {
        _controllerNames = Input.GetJoystickNames();
        if (_controllerNames != null)
        {
            if (_controllerNames.Length > 0)
            {
                m_ControllerType = ControllerType.Controller;
            }
            else
            {
                m_ControllerType = ControllerType.Mouse;
            }
        }
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

    public bool CheckForJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }

    public bool CheckForJumpInputReleased()
    {
        return Input.GetButtonUp("Jump");
    }

    public bool CheckForUseEquipmentInput()
    {
        return Input.GetButtonDown("Equipment");
    }

    public bool CheckForUseEquipmentInputReleased()
    {
        return Input.GetButtonUp("Equipment");
    }

    public bool CheckForUseWeaponInput()
    {
        bool rightTrigger = Mathf.Abs(Input.GetAxis("Fire1")) == 0 ? false : true;
        return Input.GetButtonDown("Fire1") || rightTrigger;
    }

    public bool CheckForUseWeaponInputReleased()
    {
        bool rightTrigger = Mathf.Abs(Input.GetAxis("Fire1")) == 0 ? true : false;
        return Input.GetButtonUp("Fire1") || rightTrigger;
    }

    public bool CheckForSprintInput()
    {
        return Input.GetButton("Sprint");        
    }

    public bool CheckForSprintInputReleased()
    {
        return Input.GetButtonUp("Sprint");
    }



    public void PlayerRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * m_LookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * m_LookSensitivity;

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
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        //Sprinting logic & use of player's stamina - VR
        if(CheckForSprintInput() && m_stamina.GetCurrentStamina() > 0)
        {
            m_Velocity = transform.right * x * m_SprintSpeed + transform.forward * z * m_SprintSpeed;
            m_stamina.UseStamina();
        }
        else if(CheckForSprintInputReleased())
        {
            m_stamina.StartCoroutine(m_stamina.RegenerateStamina());
        }
        else
        {
            m_Velocity = transform.right * x * m_MoveSpeed + transform.forward * z * m_MoveSpeed;
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
                if(CheckForJumpInputReleased() && m_YVelocity > 0.0f)
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

    void HandleEquipmentWheels()
    {
        if (Input.GetButtonDown("EquipmentBelt"))
        {
            EquipmentWheel.enabled = true;
            Time.timeScale = 0.3f;
            Cursor.lockState = CursorLockMode.None;
            m_ControllerState = ControllerState.Menu;
        }

        if (Input.GetButtonUp("EquipmentBelt"))
        {
            EquipmentWheel.enabled = false;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            m_ControllerState = ControllerState.Play;
        }


        if (Input.GetButtonDown("WeaponBelt"))
        {
            WeaponWheel.enabled = true;
            Time.timeScale = 0.3f;
            Cursor.lockState = CursorLockMode.None;
            m_ControllerState = ControllerState.Menu;

        }

        if (Input.GetButtonUp("WeaponBelt"))
        {
            WeaponWheel.enabled = false;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            m_ControllerState = ControllerState.Play;
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
