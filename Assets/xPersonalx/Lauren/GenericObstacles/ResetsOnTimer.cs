using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetsOnTimer : MovingObstaclesBase
{
    public float m_MovementTimer;       //timer set by user - doesn't change
    float m_Timer;

    // Start is called before the first frame update
    void Start()
    {
        m_Timer = m_MovementTimer;
        transform.position = patrolPoints[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //move the object based on taking time from one point to another
        Vector3 startPos = patrolPoints[currentPatrolIndex].transform.position;
        Vector3 targetPos;
        if (currentPatrolIndex == patrolPoints.Length - 1)
        {
            //if you reach the end of the patrol points that you are moving between, 
            //reset the target to the 2nd control point because position will be
            //reset to the first position in the patrol points
            targetPos = patrolPoints[1].transform.position;
            startPos = patrolPoints[0].transform.position;
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
                transform.position = patrolPoints[currentPatrolIndex].transform.position;
            }
            else
            {
                currentPatrolIndex++;
            }
        }
    }
}
