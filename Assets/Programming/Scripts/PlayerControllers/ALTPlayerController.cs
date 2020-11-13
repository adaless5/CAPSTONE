using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class ALTPlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Grappling,
        GrappleDeployed,
    }

    public enum ControllerType
    {
        Mouse,
        Controller,
    }


    public enum ControllerState
    {
        Play,
        Menu,
    }

    public PlayerState m_PlayerState { get; private set; }
    public ControllerState m_ControllerState;
    public ControllerType m_ControllerType;


    public Camera _camera;
    public float m_LookSensitivity = 100.0f;
    private float m_XRotation = 0f;

    public CharacterController _controller;
    Vector3 m_Velocity;
    float m_YVelocity;
    float m_MoveSpeed = 10.0f;
    public float m_Gravity = -60.0f;
    public float m_JumpHeight = 30.0f;
    public Vector3 m_Momentum { get; private set; } = Vector3.zero;

    public Belt _equipmentBelt;
    public Belt _weaponBelt;

    public Health m_health;
    public Armor m_armor;

    public Canvas EquipmentWheel;
    public Canvas WeaponWheel;

    public event Action<float> OnTakeDamage;
    public event Action<float> OnHeal;
    public event Action OnDeath;

    bool bIsInThermalView = false;

    string[] _controllerNames;
    float joyX;
    float joyY;

    bool isSelected = false;

    private void Awake()
    {
        OnTakeDamage += TakeDamage;
    }

    void Start()
    {
        DontDestroyOnLoad(this);

        m_health = GetComponent<Health>();
        m_armor = GetComponent<Armor>();

        Application.targetFrameRate = 60;

        Belt[] beltsInScene;
        beltsInScene = FindObjectsOfType<Belt>();
        foreach (Belt obj in beltsInScene)
        {
            if (obj.tag == "EquipmentBelt")
            {
                _equipmentBelt = obj;
            }
            else if (obj.tag == "WeaponBelt")
            {
                _weaponBelt = obj;
            }
        }

        Canvas[] wheelsInScene;
        wheelsInScene = FindObjectsOfType<Canvas>();
        foreach (Canvas obj in wheelsInScene)
        {
            if (obj.tag == "EquipmentWheel")
            {
                EquipmentWheel = obj;
            }
            else if (obj.tag == "WeaponWheel")
            {
                WeaponWheel = obj;
            }
        }

        EquipmentWheel.enabled = false;
        WeaponWheel.enabled = false;

        EventBroker.CallOnPlayerSpawned(gameObject);

        OnTakeDamage += m_armor.ResetArmorTimer;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

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


        //Slowdown time idea -LCC
        if (Input.GetButtonDown("EquipmentBelt"))// && m_ControllerState == ControllerState.Play)
        {
            EquipmentWheel.enabled = true;
            Time.timeScale = 0.3f;
            Cursor.lockState = CursorLockMode.None;
            CursorVisibility();
            m_ControllerState = ControllerState.Menu;
        }

        if (Input.GetButtonUp("EquipmentBelt")) //&& m_ControllerState == ControllerState.Menu)
        {
            EquipmentWheel.enabled = false;
            Time.timeScale = 1;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            m_ControllerState = ControllerState.Play;
        }


        if (Input.GetButtonDown("WeaponBelt"))// && m_ControllerState == ControllerState.Play)
        {
            WeaponWheel.enabled = true;
            Time.timeScale = 0.3f;
            Cursor.lockState = CursorLockMode.None;
            CursorVisibility();
            m_ControllerState = ControllerState.Menu;
        }

        if (Input.GetButtonUp("WeaponBelt"))// && m_ControllerState == ControllerState.Menu)
        {
            WeaponWheel.enabled = false;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            m_ControllerState = ControllerState.Play;

        }

        if (EquipmentWheel.enabled == true)
        {
            joyX = 0;
            joyY = 0;
            if (m_ControllerType == ControllerType.Controller)
            {
                joyX += Input.GetAxis("Mouse X") * m_LookSensitivity;
                joyY += Input.GetAxis("Mouse Y") * m_LookSensitivity;

                float joyAngle = Mathf.Atan2(joyX, joyY) * Mathf.Rad2Deg;

                if (joyAngle < 90)
                {
                    Button[] buttons = EquipmentWheel.GetComponentsInChildren<Button>();

                    foreach (Button b in buttons)
                    {
                        if (b.name.Contains("Grapple"))
                            b.image.sprite = b.spriteState.highlightedSprite;
                    }

                }
            }
        }

        //Test cube code (Remove this after Demo)
        if (Input.GetKeyDown(KeyCode.F))
        {
            Vector3 forward = _camera.transform.TransformDirection(Vector3.forward) * 3;

            RaycastHit hit;


            Debug.DrawRay(_camera.transform.position, forward, Color.green, 5);

            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 10.0f, 1 << 4, QueryTriggerInteraction.Ignore))
            {
                hit.collider.gameObject.SendMessage("ChangeColor");
            }
        }//


        //Damage debug -LCC
        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(20.0f);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            m_health.Heal(20.0f);
        }

    }

    private void ControllerCheck()
    {
        _controllerNames = Input.GetJoystickNames();
        if (_controllerNames[0].Contains("Controller"))
        {
            m_ControllerType = ControllerType.Controller;
        }
        else
        {
            m_ControllerType = ControllerType.Mouse;
        }
    }

    private void CursorVisibility()
    {
        //switch (m_ControllerType)
        //{
        //    case ControllerType.Controller:
        //        Cursor.visible = false;
        //        break;
        //    case ControllerType.Mouse:
        //        Cursor.visible = true;
        //        break;
        //}
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

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.GetContact(0).normal);
    }

    public bool CheckForJumpInput()
    {
        return Input.GetButtonDown("Jump");
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

        m_Velocity = transform.right * x * m_MoveSpeed + transform.forward * z * m_MoveSpeed;

        HandleJump();

        ApplyGravity();

        m_Velocity += m_Momentum;

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
        if (_controller.isGrounded)
        {
            m_YVelocity = 0f;

            if (CheckForJumpInput())
            {
                m_YVelocity = m_JumpHeight;
            }
        }
        else if (!_controller.isGrounded)
        {
            if (_controller.collisionFlags.ToString() == "Above")
            {
                m_YVelocity = -2.0f;
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


}
