using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float m_healAmount = 20f;
    Pickup m_healthPickup;

    // Start is called before the first frame update
    void Start()
    {
        m_healthPickup = GetComponent<Pickup>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Health playerHP = other.GetComponent<Health>();
        if (playerHP)
        {
            if (!playerHP.IsAtFullHealth())
            {
                playerHP.Heal(m_healAmount);
                Destroy(gameObject);
            }
        }
    }
}
