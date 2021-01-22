using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_UpgradePickup : MonoBehaviour
{
    public WeaponScalars[] Upgrades;

    public int item_index = 0;
    public bool HasAction = false;
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
                foreach(WeaponScalars up in Upgrades)
                {
                    weapon.AddUpgrade(up);
                }
                weapon.SetHasAction(HasAction);
                Destroy(gameObject);
            }
        }
    }
}
