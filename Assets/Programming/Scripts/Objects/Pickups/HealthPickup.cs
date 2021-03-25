using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPickup : MonoBehaviour
{
    public float m_healAmount = 20f;

    public Compass m_compass;
    public CompassMarkers m_marker;

    


    // Start is called before the first frame update
    void Start()
    {
        EventBroker.OnPlayerSpawned += PlayerSpawned;     
       
       

    }

    void PlayerSpawned(GameObject playerRef)
    {
        try
        {
           // m_healthPickup = GetComponent<Pickup>();
            m_compass = FindObjectOfType<Compass>();
            m_marker = GetComponent<CompassMarkers>();
            if (m_marker != null)
                m_compass.AddMarker(m_marker);
        }
        catch
        {

        }     
    }

    private void OnTriggerEnter(Collider other)
    {
        Health playerHP = other.GetComponent<Health>();
        if (playerHP)
        {
            if (!playerHP.IsAtFullHealth())
            {
                EventBroker.CallOnHealthPickup(m_healAmount);
                playerHP.Heal(m_healAmount);
                Destroy(gameObject);
                if (m_marker != null)
                {
                    m_compass.RemoveMarker(m_marker);
                }
            }
            else
            {
                EventBroker.CallOnHealthPickupAttempt(playerHP.IsAtFullHealth());
            }
        }
    }
}
