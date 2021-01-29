using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class TEMPORARY_DroneAI : MonoBehaviour
{
    public MeshRenderer m_MeshRenderComponent;


    ALTPlayerController m_Player;

    public int _currentPatrolIndex;
    public Transform[] _patrolPoints;

    public Vector3 m_TargetLocation;
    public Vector3 direction;

    public float _patrolSpeed;
    public float m_FollowRange;
    public float m_AttackEscapeRange;
    public float m_AttackRange;
    public float m_AttackTime;
    public float m_AttackDelay;
    bool b_isAttacking;

    public enum DRONESTATE
    {
        IDLE, PATROL, FOLLOW, ATTACK
    }

    public DRONESTATE m_CurrentState;



    // Start is called before the first frame update
    void Start()
    {
        m_MeshRenderComponent = GetComponent<MeshRenderer>();

        m_FollowRange = 20.0f;
        m_AttackEscapeRange = 5.0f;
        m_AttackRange = 10.0f;
        m_AttackTime = 0.5f;
        m_AttackDelay = 3.0f;
        b_isAttacking = false;

        _patrolSpeed = 5.0f;
        _currentPatrolIndex = 0;

        m_CurrentState = DRONESTATE.PATROL;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_Player == null) 
        {
            m_Player = Object.FindObjectOfType<ALTPlayerController>();
            m_TargetLocation = m_Player.transform.position;
        }

        STATES_BEHAVIOR();
    }

    void STATES_BEHAVIOR()
    {

        if (m_CurrentState == DRONESTATE.IDLE)
        {
            IDLE();

        }
        if (m_CurrentState == DRONESTATE.PATROL)
        {
            PATROL();
            if (Vector3.Distance(transform.position, m_Player.transform.position) < m_FollowRange)
            {
                m_CurrentState = DRONESTATE.FOLLOW;
            }
        }
        if (m_CurrentState == DRONESTATE.FOLLOW)
        {
            FOLLOW();
            if (Vector3.Distance(transform.position, m_Player.transform.position) < m_AttackRange - m_AttackEscapeRange)
            {
                m_CurrentState = DRONESTATE.ATTACK;
            }
            if (Vector3.Distance(transform.position, m_Player.transform.position) > m_FollowRange + m_AttackEscapeRange)
            {
                m_CurrentState = DRONESTATE.PATROL;
            }
        }
        if (m_CurrentState == DRONESTATE.ATTACK)
        {
            ATTACK();
            if (Vector3.Distance(transform.position, m_Player.transform.position) > m_AttackRange)
            {
                m_CurrentState = DRONESTATE.FOLLOW;
            }
        }
    }

    public void LookTowards(Transform target, float turnspeed)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        float str;
        str = Mathf.Min(turnspeed * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);
    }

    void FOLLOW()
    {
        m_TargetLocation = m_Player.transform.position;
        direction = Vector3.Normalize(m_TargetLocation - transform.position);
        transform.position += (transform.forward
                            * Time.fixedDeltaTime)
                            * (Vector3.Distance(transform.position, m_Player.transform.position) * 0.5f);
        LookTowards(m_Player.transform,4);
    }

    void PATROL()
    {

        if (_currentPatrolIndex != _patrolPoints.Length)
        {
            if (Vector3.Distance(transform.position, _patrolPoints[_currentPatrolIndex].transform.position) > 2)
            {

                LookTowards(_patrolPoints[_currentPatrolIndex].transform, 4.0f);
                transform.position = Vector3.MoveTowards(transform.position, _patrolPoints[_currentPatrolIndex].transform.position, _patrolSpeed * Time.deltaTime);
            }
            else
            {
                //Debug.Log("Patrol point reached");
                _currentPatrolIndex++;
            }
        }
        else
        {
            _currentPatrolIndex = 0;
        }
    }

    void IDLE()
    {

    }

    void ATTACK()
    {
        if (b_isAttacking == false)
        {
            LookTowards(m_Player.transform, 6);
            m_AttackTime = 0.5f;
            m_AttackDelay -= Time.fixedDeltaTime;
            if (m_AttackDelay < 0.0f)
            {
                b_isAttacking = true;
            }
        }
        else
        {
            m_AttackDelay = 3.0f;
            m_AttackTime -= Time.fixedDeltaTime;
            if (m_AttackTime < 0.0f)
            {
                RaycastHit hit;
                if (Physics.CapsuleCast(transform.position, transform.position * 20, 1.0f, transform.forward, out hit, 10))
                {
                    //vvvvvvvv  ADD attack logic here
                    m_Player.transform.position += new Vector3(transform.forward.x * 2.0f, 0.0f, transform.forward.z * 2.0f);
                    //^^^^^^^^
                }
                b_isAttacking = false;
            }
        }
    }



}