using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoPickup : MonoBehaviour
{
    public WeaponType ammoType;
    private int weaponIndex;
   
    public int m_amountOfClipsInPickup = 2;
    public int m_clipSize = 6;
    Pickup m_ammoPickup;

    Weapon m_baseWeapon;
    Weapon m_creatureWeapon;
    Weapon m_grenadeWeapon;

    public Compass m_compass;
    public CompassMarkers m_marker;

    bool isPickedUp;

    // Start is called before the first frame update
    void Start()
    {
        EventBroker.OnPlayerSpawned += PlayerSpawned;

        m_ammoPickup = GetComponent<Pickup>();

        //if (ammoType == WeaponType.BaseWeapon)
        //{
        //    m_marker.m_markerImage = Resources.Load<Image>("Sprites/Icons/Ammo/AMMO_DEFAULT");
        //}
        //else if (ammoType == WeaponType.CreatureWeapon)
        //{
        //    m_marker.m_markerImage = Resources.Load<Image>("Sprites/Icons/Ammo/AMMO_CREATURE");
        //}
        //else
        //{
        //    m_marker.m_markerImage = Resources.Load<Image>("Sprites/Icons/Ammo/AMMO_GRENADE");
        //}

       // m_marker.m_markerImage = FindObjectOfType<Sprite>();

        isPickedUp = false;
    }

    void PlayerSpawned(GameObject playerReference)
    {
        m_compass = FindObjectOfType<Compass>();
        m_marker = GetComponent<CompassMarkers>();
        if(m_marker != null)
        m_compass.AddMarker(m_marker);
    }

    void Awake()
    {
        switch(ammoType)
        {
            case WeaponType.BaseWeapon:
                weaponIndex = 0;
                break;

            case WeaponType.GrenadeWeapon:
                weaponIndex = 1;
                break;

            case WeaponType.CreatureWeapon:
                weaponIndex = 2;
                break;
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && isPickedUp == false)
        {   
            isPickedUp = true;
            EventBroker.CallOnAmmoPickup(ammoType, m_amountOfClipsInPickup);
            //Destroy(gameObject); // EVAN COMMENTED THIS OUT AND ADDED THE LINE BELOW, IF THERE IS A PROBLEM YELL AT HIM
            gameObject.SetActive(false);

            if(m_marker != null)
            m_compass.RemoveMarker(m_marker);            
        }
    }
}
