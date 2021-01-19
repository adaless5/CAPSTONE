using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_UpgradePickup : MonoBehaviour
{
    public WeaponScalars Upgrade;
    public int item_index = 0;
    private void OnTriggerEnter(Collider other)
    {
        ALTPlayerController player = other.GetComponent<ALTPlayerController>();
        if(player)
        {
            Tool item = player._weaponBelt._items[item_index];
            Debug.Log(item);
            if(item.GetComponent<Weapon>())
            {
                Weapon weapon = item.GetComponent<Weapon>();
                weapon.AddUpgrade(Upgrade);
                Destroy(gameObject);
            }
        }
    }
}
