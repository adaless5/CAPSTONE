using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float m_healAmount = 20f;
    Pickup m_healthPickup;

    public Compass m_compass;
    public CompassMarkers m_marker;

    // Start is called before the first frame update
    void Start()
    {
        EventBroker.OnPlayerSpawned += PlayerSpawned;
     
        m_marker = GetComponent<CompassMarkers>();
        m_healthPickup = GetComponent<Pickup>();

    }

    void PlayerSpawned(GameObject playerRef)
    {
        m_compass = FindObjectOfType<Compass>();
        if (m_marker != null)
        m_compass.AddMarker(m_marker);
    }

    private void OnTriggerEnter(Collider other)
    {
        Health playerHP = other.GetComponent<Health>();
        if (playerHP && m_healthPickup != null)
        {
            if (!playerHP.IsAtFullHealth())
            {
                playerHP.Heal(m_healAmount);
                Destroy(gameObject);
                if (m_marker != null)
                {
                    m_compass.RemoveMarker(m_marker);
                }
            }
        }
    }
}
