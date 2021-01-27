using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetsOnSpeed : MovingObstaclesBase
{
    public float m_MovementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = patrolPoints[0].transform.position;
    }

    // Update is called once per frame
    void Update()
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
                //if reach the end of the patrol list, send the object back to the start of the patrol
                transform.position = patrolPoints[currentPatrolIndex].transform.position;
            }
            else
            {
                currentPatrolIndex++;
            }
        }
    }
}
