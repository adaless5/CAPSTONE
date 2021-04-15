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
    protected int m_ammoCap = 0;

    Pickup m_ammoPickup;

    Weapon m_baseWeapon;
    Weapon m_creatureWeapon;
    Weapon m_grenadeWeapon;

    public Compass m_compass;
    public CompassMarkers m_marker;
    public AmmoController m_ammoController;

    bool isPickedUp;
    bool isMarkerCreated;

    // Start is called before the first frame update
    void Start()
    {
        EventBroker.OnPlayerSpawned += PlayerSpawned;

        isPickedUp = false;
        isMarkerCreated = false;
        m_ammoController = null;
    }

    void PlayerSpawned(GameObject playerReference)
    {
        try
        {
            m_ammoPickup = GetComponent<Pickup>();
            m_compass = FindObjectOfType<Compass>();
            m_marker = GetComponent<CompassMarkers>();
            m_ammoController = FindObjectOfType<AmmoController>();
        }
        catch { }
        if (m_marker != null && isMarkerCreated == false)
        {
            m_compass.AddMarker(m_marker);
            isMarkerCreated = true;
        }

    }

    void Awake()
    {
        switch (ammoType)
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

        m_ammoController = FindObjectOfType<AmmoController>();

        if (other.tag == "Player" && isPickedUp == false && m_ammoController.IsAmmoFull(ammoType) == false)
        {
            isPickedUp = true;
            EventBroker.CallOnAmmoPickup(ammoType, m_amountOfClipsInPickup, m_ammoCap);
            gameObject.SetActive(false);

            if (m_marker != null)
                m_compass.RemoveMarker(m_marker);
        }
        else if (other.tag == "Player" && isPickedUp == false && m_ammoController.IsAmmoFull(ammoType) == true)
        {
            EventBroker.CallOnAmmoPickupAttempt();
        }
    }
}
