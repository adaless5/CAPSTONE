using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DamageHandler(int amount);

//this class will act as an event manager for all UI scripts - VR
public class HUD : MonoBehaviour
{
    public event DamageHandler TakeDamage;

    public HealthBarUI m_healthUI;
    public ArmorBarUI m_armorUI;
    public AmmoUI m_ammoUI;

    private void Awake()
    {
        m_healthUI = GetComponentInChildren<HealthBarUI>();
        m_armorUI = GetComponentInChildren<ArmorBarUI>();
        m_ammoUI = GetComponentInChildren<AmmoUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
