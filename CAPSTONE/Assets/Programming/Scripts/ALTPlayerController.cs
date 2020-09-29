using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALTPlayerController : MonoBehaviour
{
    public Camera _camera;
    public float m_LookSensitivity = 100.0f;
    private float m_XRotation = 0f;

    public CharacterController _controller;
    Vector3 m_Velocity;
    float m_YVelocity;
    public float m_MoveSpeed = 20.0f;
    public float m_Gravity = -60.0f;
    public float m_JumpHeight = 30.0f;
    public Vector3 m_Momentum { get; private set; } = Vector3.zero;

    void Start()
    {
        Application.targetFrameRate = 120;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

    }

    public bool CheckForJumpInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public bool CheckForGrappleInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public void PlayerRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * m_LookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * m_LookSensitivity;

        m_XRotation += mouseY;
        m_XRotation = Mathf.Clamp(m_XRotation, -90.0f, 90.0f);

        _camera.transform.localRotation = Quaternion.Euler(-m_XRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
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
}
