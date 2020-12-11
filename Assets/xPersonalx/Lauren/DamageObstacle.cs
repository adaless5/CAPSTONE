using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObstacle : GenericObstacles
{
    //damage variables
    public int m_DamagePerTick;
    public bool m_DamageRadiusActivated;
    public bool m_DamageTimerActivated;

    Vector3 m_StartPos;
    Vector3 m_TargetPos;
    public float m_HeightDistance;  //creates the targetposition based on start position

    //based on timer variables
    public float m_ActivationTimer;       //timer set by user - doesn't change
    float m_Timer;                         //timer that counts down

    //based on radius variables
    public float m_ActivationSpeed;

    public bool m_IsColliding;
    public GameObject m_ActivationRadius;

    // Start is called before the first frame update
    void Start()
    {
        //Set the starting position and the target position of trap
        m_StartPos = transform.position;    //start position is where you place it in the editor
        m_TargetPos = new Vector3(m_StartPos.x, m_StartPos.y + m_HeightDistance, m_StartPos.z);

        //If timer activated, set timer activated variables
        if (m_DamageTimerActivated)
            m_Timer = m_ActivationTimer;

        //if radius activated, set radius activated variables
        if (m_DamageRadiusActivated && m_ActivationRadius)
        {
            //need to set position of m_ActivationRadius in the editor
            //that way the "trigger" (i.e. collider) isn't tied to the trap
            //for now I'm making the position equal to the trap position
            //creates a basic trap that activates when player gets closer to it
            //when changing this in the future to be more flexible, ALSO NEED TO CHANGE IN UPDATE()
            m_ActivationRadius.transform.position = transform.position;
            m_ActivationRadius.transform.localScale = transform.localScale * 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if activated using a radius
        if (m_DamageRadiusActivated)
        {
            m_ActivationRadius.transform.position = transform.position;
            ActivateDamageTrapBasedOnRadius();

        }

        //if activated using a timer
        else if (m_DamageTimerActivated)
        {
            ActivateDamageTrapBasedOnTimer();
        }
    }

    void ActivateDamageTrapBasedOnRadius()
    {
        if (m_IsColliding)
        {
            //move the object based on speed only
            float step = m_ActivationSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, m_TargetPos, step);

            if (transform.position == m_TargetPos)
            {
                //change direction
                ChangeDirection();
            }
        }
    }

    void ActivateDamageTrapBasedOnTimer()
    {

        //if timer is done
        if (m_Timer <= 0.0f)
        {
            //reset timer
            m_Timer = m_ActivationTimer;
            transform.position = m_TargetPos;

            //change direction
            ChangeDirection();
        }
        else
        {
            //move the trap
            m_Timer -= Time.deltaTime;

            //move the object based on taking time from one point to another
            Vector3 difference = m_TargetPos - m_StartPos;

            float percent = m_Timer / m_ActivationTimer;
            transform.position = m_StartPos + difference * percent;
        }
    }

    void ChangeDirection()
    {
        Vector3 temp = m_StartPos;
        m_StartPos = m_TargetPos;
        m_TargetPos = temp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_IsColliding = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_IsColliding = false;
        }
    }
}
