using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : GenericObstacles
{

    //movement variables
    public float m_MovementSpeed;
    public float m_MovementTimer;       //timer set by user - doesn't change
    float m_Timer;
    public bool m_MovementRadiusActiavates;
    public bool m_MovementTimerActivates;
    public GameObject[] patrolPoints;
    int currentPatrolIndex;

    // Start is called before the first frame update
    void Start()
    {
        m_Timer = m_MovementTimer;
    }

    // Update is called once per frame
    void Update()
    {
        //if the object moves
        if (m_MovementRadiusActiavates)
        {
            //if the object moves based on distance from the player
            MoveObjectBasedOnPlayerDistance();
        }

        else if (m_MovementTimerActivates)
        {
            //if the object moves based on a timer
            MoveObjectBasedOnTimer();
        }

        else
        {
            //if the object moves all the time and isn't based on time or distance from player
            MoveObject();
        }
    }

    void MoveObject()
    {
        //move the object based on speed only
        float step = m_MovementSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPatrolIndex].transform.position, step);

        if (transform.position == patrolPoints[currentPatrolIndex].transform.position)
        {
            //if reaches current target, switch targets
            if (currentPatrolIndex == patrolPoints.Length - 1)
            {
                currentPatrolIndex = 0;
            }
            else
            {
                currentPatrolIndex++;
            }
        }
    }

    void MoveObjectBasedOnTimer()
    {
        //move the object based on taking time from one point to another
        Vector3 startPos = patrolPoints[currentPatrolIndex].transform.position;
        Vector3 targetPos;
        if (currentPatrolIndex == patrolPoints.Length - 1)
        {
            targetPos = patrolPoints[0].transform.position;
        }
        else
        {
            targetPos = patrolPoints[currentPatrolIndex + 1].transform.position;
        }

        Vector3 difference = targetPos - startPos;

        //if timer is still going
        if (m_Timer <= m_MovementTimer && m_Timer > 0.0f)
        {
            m_Timer -= Time.deltaTime;
            float percent = m_Timer / m_MovementTimer;
            transform.position = startPos + difference * percent;
        }
        //if timer is up - should be at end point
        else if (m_Timer <= 0.0f)
        {
            m_Timer = m_MovementTimer;
            transform.position = targetPos;
            if (currentPatrolIndex == patrolPoints.Length - 1)
            {
                currentPatrolIndex = 0;
            }
            else
            {
                currentPatrolIndex++;
            }
        }
    }

    void MoveObjectBasedOnPlayerDistance()
    {
        //move object based on players distance
    }
}
