using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public WeaponType ammoType;
    private int weaponIndex;

    public int m_amountOfClipsInPickup = 2;
    public int m_clipSize = 6;
    Pickup m_ammoPickup;

    bool isPickedUp;

    // Start is called before the first frame update
    void Start()
    {
        m_ammoPickup = GetComponent<Pickup>();
        isPickedUp = false;
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
        // WeaponBase playerAmmo = (WeaponBase)other.gameObject.GetComponent<WeaponBelt>().GetToolAtIndex(weaponIndex);
        if(isPickedUp == false)
        {
            isPickedUp = true;
            EventBroker.CallOnAmmoPickup(ammoType, m_amountOfClipsInPickup);
            Destroy(gameObject);
        }
    }
}
