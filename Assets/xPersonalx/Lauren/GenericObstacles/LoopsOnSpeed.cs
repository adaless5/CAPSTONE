using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopsOnSpeed : MovingObstaclesBase
{
    public float m_MovementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
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
            }
            else
            {
                currentPatrolIndex++;
            }
        }
    }
}
