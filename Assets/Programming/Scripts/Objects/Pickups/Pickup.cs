using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Pickup : MonoBehaviour
{
    [SerializeField]
    protected float m_verticalBounceSpeed = 2f;
    [SerializeField]
    protected float m_bounceAmount = 0.25f;
    [SerializeField]
    protected float m_rotationSpeed = 10f;
    
    public Rigidbody m_pickupBody { get; private set; }

    Collider m_Collider;
    protected Vector3 m_StartPosition;

    // Start is called before the first frame update
    void Start()
    {
        m_pickupBody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();

       // m_pickupBody.isKinematic = true;
        m_Collider.isTrigger = true;

        m_StartPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Animation of pickup
        float bobbingAnimationPhase = ((Mathf.Sin(Time.time * m_verticalBounceSpeed) * 0.5f) + 0.5f) * m_bounceAmount;
        transform.position = m_StartPosition + Vector3.up * bobbingAnimationPhase;
        transform.Rotate(Vector3.up, m_rotationSpeed * Time.deltaTime, Space.Self);
    }
}
