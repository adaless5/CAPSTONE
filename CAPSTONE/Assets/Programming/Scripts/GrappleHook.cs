using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Grappling,
        GrappleDeployed,
    }

    public PlayerState m_PlayerState;

    public Transform m_GrappleHookTransform;
    public float m_GrappleHookSpeedMultiplier = 5.0f;
    float m_GrappleHookLength;
    public float m_AutoReleaseGrappleDistance = 2.0f;
    public float m_MinGrappleSpeed = 10.0f;
    public float m_MaxGrappleSpeed = 40.0f;
    private Vector3 m_GrappleTarget;
    public Vector3 m_Momentum = Vector3.zero;
    public float m_GrappleDeploySpeed = 100.0f;
    public float m_GrappleMomentumMultiplier = 7.0f;
    public float m_GrappleJumpMultiplier = 60.0f;

    public ALTPlayerController m_PlayerController;

    void Awake()
    {
        m_GrappleHookTransform.gameObject.SetActive(false);
    }

    void Start()
    {

    }

    void Update()
    {
        switch (m_PlayerState)
        {
            case PlayerState.Idle:
                m_PlayerController.PlayerRotation();
                m_PlayerController.PlayerMovement();
                HandleGrappleHook();
                break;

            case PlayerState.Grappling:
                HandleGrappleHookMovement();
                m_PlayerController.PlayerRotation();
                break;

            case PlayerState.GrappleDeployed:
                HandleGrappleHookDeployed();
                m_PlayerController.PlayerRotation();
                m_PlayerController.PlayerMovement();
                break;
        }
    }

    void HandleGrappleHook()
    {
        if (m_PlayerController.CheckForGrappleInput())
        {
            Vector3 camPos = m_PlayerController._camera.transform.position;
            Vector3 camForwardVec = m_PlayerController._camera.transform.forward;

            if (Physics.Raycast(camPos, camForwardVec, out RaycastHit raycastHit))
            {
                m_GrappleTarget = raycastHit.point;
                m_GrappleHookLength = 0.0f;
                m_GrappleHookTransform.gameObject.SetActive(true);
                m_GrappleHookTransform.localScale = Vector3.zero;
                m_PlayerState = PlayerState.GrappleDeployed;
            }
        }
    }

    void HandleGrappleHookDeployed()
    {
        m_GrappleHookTransform.gameObject.SetActive(true);

        m_GrappleHookTransform.LookAt(m_GrappleTarget);

        m_GrappleHookLength += m_GrappleDeploySpeed * Time.deltaTime;

        m_GrappleHookTransform.localScale = new Vector3(1, 1, m_GrappleHookLength);

        if (m_GrappleHookLength >= Vector3.Distance(transform.position, m_GrappleTarget))
        {
            m_PlayerState = PlayerState.Grappling;
        }
    }

    void HandleGrappleHookMovement()
    {
        m_GrappleHookTransform.LookAt(m_GrappleTarget);

        float distFromGrappleTarget = Vector3.Distance(transform.position, m_GrappleTarget);
        Vector3 grappleDirection = m_GrappleTarget - transform.position;
        grappleDirection.Normalize();

        float speed = Mathf.Clamp(distFromGrappleTarget, m_MinGrappleSpeed, m_MaxGrappleSpeed);

        m_PlayerController._controller.Move(grappleDirection * m_GrappleHookSpeedMultiplier * speed * Time.deltaTime);

        if (distFromGrappleTarget < m_AutoReleaseGrappleDistance)
        {
            DeactivateGrappleHook();
        }

        if (m_PlayerController.CheckForGrappleInput())
        {
            DeactivateGrappleHook();
        }

        if (m_PlayerController.CheckForJumpInput())
        {
            m_PlayerController.ApplyMomentum(grappleDirection, speed, m_GrappleMomentumMultiplier, m_GrappleJumpMultiplier);
            DeactivateGrappleHook();
        }
    }

    void DeactivateGrappleHook()
    {
        //m_PlayerController.ResetGravity();
        m_PlayerState = PlayerState.Idle;
        m_GrappleHookTransform.gameObject.SetActive(false);
    }
}
