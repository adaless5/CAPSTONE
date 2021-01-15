using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_UpgradePickup : MonoBehaviour
{
    public WeaponScalars Upgrade;

    private void OnTriggerEnter(Collider other)
    {
        ALTPlayerController player = other.GetComponent<ALTPlayerController>();
        if(player)
        {
            Tool item = player._weaponBelt._items[0];
            if(item.GetComponent<WeaponBase>())
            {
                WeaponBase weapon = item.GetComponent<WeaponBase>();
                weapon.AddUpgrade(Upgrade);
                Destroy(gameObject);
            }
        }
    }
}
