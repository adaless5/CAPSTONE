using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBelt : Belt
{
    // Start is called before the first frame update
    private void Start()
    {
        base.Start();
        EventBroker.OnPickupWeapon += WeaponBeltPickup;
    }

    public void WeaponBeltPickup(int weaponIndex)
    {
        Debug.Log("Weapon Obtained, weapon size " + _items.Length);
        ObtainEquipmentAtIndex(weaponIndex);
    }

}
