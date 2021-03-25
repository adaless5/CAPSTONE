using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : Equipment, ISaveable
{
    public Transform m_GrappleHookTransform;
    public Transform m_PlayerPosition;
    public float m_GrappleHookSpeedMultiplier = 5.0f;
    float m_GrappleHookLength;
    public float m_AutoReleaseGrappleDistance = 2.0f;
    public float m_MinGrappleSpeed = 10.0f;
    public float m_MaxGrappleSpeed = 40.0f;
    private Vector3 m_GrappleTarget;
    public Vector3 m_Momentum = Vector3.zero;
    public float m_GrappleDeploySpeed = 70.0f;
    float m_GrappleMomentumMultiplier = 100.0f;
    float m_GrappleJumpMultiplier = 60.0f;
    const float MAX_GRAPPLE_DIST = 100.0f;
    const float MIN_GRAPPLE_DIST = 5f;

    public ALTPlayerController m_PlayerController;
    public GameObject m_GrappleMarker;
    public GameObject m_GrappleGun;
    public GameObject m_GrappleCable;
    MeshRenderer m_SpriteRenderer;

    bool bCanGrapple = true;

    void Awake()
    {
        LoadDataOnSceneEnter();

        m_SpriteRenderer = m_GrappleMarker.GetComponentInChildren<MeshRenderer>();
        m_GrappleMarker.SetActive(false);
        m_GrappleGun.SetActive(false);
        m_GrappleCable.SetActive(false);
    
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (bIsActive && bIsObtained)
        {
            m_GrappleGun.SetActive(true);
            switch (m_PlayerController.m_PlayerState)
            {
                case ALTPlayerController.PlayerState.Idle:
                    UseTool();
                    HandleGrappleMarker();
                    break;

                case ALTPlayerController.PlayerState.Grappling:
                    HandleGrappleHookMovement();
                    break;

                case ALTPlayerController.PlayerState.GrappleDeployed:
                    HandleGrappleHookDeployed();
                    break;
            }
        }

        if (!m_PlayerController.CheckForUseEquipmentInput() && m_PlayerController.m_PlayerState == ALTPlayerController.PlayerState.Idle && !bCanGrapple)
        {
            bCanGrapple = true;
        }
        
        
    }

    public override void UseTool()
    {
        if (bCanGrapple)
        {
            if (m_PlayerController.CheckForUseEquipmentInput())
            {
                m_GrappleMarker.SetActive(false);

                Vector3 camPos = m_PlayerController._camera.transform.position;
                Vector3 camForwardVec = m_PlayerController._camera.transform.forward;

                if (Physics.Raycast(camPos, camForwardVec, out RaycastHit raycastHit, MAX_GRAPPLE_DIST))
                {
                    if (Vector3.Distance(raycastHit.point, camPos) >= MIN_GRAPPLE_DIST)
                    {
                        m_GrappleCable.SetActive(true);
                        m_GrappleTarget = raycastHit.point;
                        m_GrappleHookLength = 0.0f;

                        if (raycastHit.collider.gameObject.tag == "Enemy")
                        {
                            DroneAI AItemp = raycastHit.collider.gameObject.GetComponent<DroneAI>();

                            if (AItemp != null)
                            {
                                AItemp.Stun();
                            }
                        }

                        m_PlayerController.ChangePlayerState(ALTPlayerController.PlayerState.GrappleDeployed);
                        //Grapple Deployed Audio Triggers
                        GetComponent<AudioManager_Grapple>().SetGrappleHookPointAndHitType
                            (raycastHit.transform, raycastHit.collider.gameObject.layer);
                        GetComponent<AudioManager_Grapple>().TriggerShot();

                        bCanGrapple = false;
                        m_PlayerController.bWasGrappling = true;
                    }
                }
            }
        }
    }

    void HandleGrappleHookDeployed()
    {

        m_GrappleMarker.SetActive(false);
        m_GrappleCable.SetActive(true);

        m_GrappleHookTransform.LookAt(m_GrappleTarget);

        m_GrappleHookLength += m_GrappleDeploySpeed * Time.deltaTime;

        m_GrappleCable.transform.localScale = new Vector3(1, m_GrappleHookLength * 0.5f, 1);

        if (m_GrappleHookLength >= Vector3.Distance(m_PlayerPosition.position, m_GrappleTarget))
        {
            m_PlayerController.ChangePlayerState(ALTPlayerController.PlayerState.Grappling);

            //Grapple Impact Audio Triggers
            GetComponent<AudioManager_Grapple>().TriggerHit();
            //GetComponent<AudioManager_Grapple>().StopShot();
            GetComponent<AudioManager_Grapple>().TriggerRetract();
            //
        }
    }

    void HandleGrappleHookMovement()
    {
        m_GrappleMarker.SetActive(false);
        m_GrappleHookTransform.LookAt(m_GrappleTarget);

        float distFromGrappleTarget = Vector3.Distance(m_PlayerPosition.position, m_GrappleTarget);
        Vector3 grappleDirection = m_GrappleTarget - m_PlayerPosition.position;
        grappleDirection.Normalize();

        float speed = 30.0f;

        m_PlayerController._controller.Move(grappleDirection * m_GrappleHookSpeedMultiplier * speed * Time.deltaTime);

        if (distFromGrappleTarget < m_AutoReleaseGrappleDistance)
        {
            DeactivateGrappleHook();
        }

        if (m_PlayerController.CheckForJumpInput())
        {
            m_PlayerController.ApplyMomentum(grappleDirection, speed, m_GrappleMomentumMultiplier, m_GrappleJumpMultiplier);
            DeactivateGrappleHook();
        }
    }

    void HandleGrappleMarker()
    {
        Vector3 camPos = m_PlayerController._camera.transform.position;
        Vector3 camForwardVec = m_PlayerController._camera.transform.forward;
        if (bIsActive && bIsObtained)
        {

            if (Physics.Raycast(camPos, camForwardVec, out RaycastHit raycastHit, MAX_GRAPPLE_DIST))
            {
                if (Vector3.Distance(raycastHit.point, camPos) >= MIN_GRAPPLE_DIST)
                {
                    if (!m_GrappleMarker.activeInHierarchy)
                    {
                        m_GrappleMarker.SetActive(true);
                        Transform tran = m_SpriteRenderer.gameObject.transform;
                        tran.position = raycastHit.point;
                    }
                    else
                    {
                        Transform tran = m_SpriteRenderer.gameObject.transform;
                        tran.position = Vector3.Lerp(tran.position, raycastHit.point, Time.deltaTime * 20.0f);
                    }
                }
                else
                {
                    m_SpriteRenderer.gameObject.transform.position = Vector3.zero;
                    m_GrappleMarker.SetActive(false);
                    //print(m_GrappleMarker.activeSelf);
                }
            }
            else
            {
                m_SpriteRenderer.gameObject.transform.position = Vector3.zero;
                m_GrappleMarker.SetActive(false);
            }
        }
    }

    void DeactivateGrappleHook()
    {
        //Grapple Done Audio Triggers
        GetComponent<AudioManager_Grapple>().StopRetract();
        GetComponent<AudioManager_Grapple>().TriggerClick();
        //

        m_GrappleHookTransform.localEulerAngles = Vector3.zero;
        m_PlayerController.ResetGravity();
        m_PlayerController.ChangePlayerState(ALTPlayerController.PlayerState.Idle);
        m_GrappleHookTransform.localEulerAngles = Vector3.zero;
        m_GrappleCable.SetActive(false);
        m_GrappleCable.transform.localScale = new Vector3(1, 1, 1);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        m_GrappleGun.SetActive(false);
        m_GrappleCable.SetActive(false);
        m_GrappleMarker.SetActive(false);
        DeactivateGrappleHook();
        //gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    public void LoadDataOnSceneEnter()
    {
        bIsObtained = SaveSystem.LoadBool(gameObject.name, "bIsObtained", "Equipment");
    }

}
