using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeAltar : MonoBehaviour
{
    [Header("Altar Settings")]
    [SerializeField]
    int NumberOfPermittedUpgrades = 99;

    [Header("Deafault Upgrades")]
    [SerializeField]
    WeaponUpgrade DeafaultUpgrade1;
    [SerializeField]
    WeaponUpgrade DeafaultUpgrade2;
    [SerializeField]
    WeaponUpgrade DeafaultUpgrade3;
    [SerializeField]
    WeaponUpgrade DeafaultUpgrade4;

    [Header("Explosive Upgrades")]
    [SerializeField]
    WeaponUpgrade ExplosiveUpgrade1;
    [SerializeField]
    WeaponUpgrade ExplosiveUpgrade2;
    [SerializeField]
    WeaponUpgrade ExplosiveUpgrade3;
    [SerializeField]
    WeaponUpgrade ExplosiveUpgrade4;

    [Header("Creature Upgrades")]
    [SerializeField]
    WeaponUpgrade CreatureUpgrade1;
    [SerializeField]
    WeaponUpgrade CreatureUpgrade2;
    [SerializeField]
    WeaponUpgrade CreatureUpgrade3;
    [SerializeField]
    WeaponUpgrade CreatureUpgrade4;

    //dont need variable for action, its just a bool

    List<WeaponUpgrade> _upgrades;

    private void Start()
    {
        _upgrades = new List<WeaponUpgrade>();
        _upgrades.Add(DeafaultUpgrade1);
        _upgrades.Add(DeafaultUpgrade2);
        _upgrades.Add(DeafaultUpgrade3);
        _upgrades.Add(DeafaultUpgrade4);
        _upgrades.Add(ExplosiveUpgrade1);
        _upgrades.Add(ExplosiveUpgrade2);
        _upgrades.Add(ExplosiveUpgrade3);
        _upgrades.Add(ExplosiveUpgrade4);
        _upgrades.Add(CreatureUpgrade1);
        _upgrades.Add(CreatureUpgrade2);
        _upgrades.Add(CreatureUpgrade3);
        _upgrades.Add(CreatureUpgrade4);

    }

    private void OnTriggerStay(Collider other)
    {

            if (other.gameObject.GetComponentInChildren<UpgradeMenuUI>() && other.GetComponentInChildren<ALTPlayerController>().CheckForInteract())
            {
                other.gameObject.GetComponentInChildren<UpgradeMenuUI>()._numUpgradesAllowed = NumberOfPermittedUpgrades;
                other.gameObject.GetComponentInChildren<UpgradeMenuUI>().Activate();
                other.gameObject.GetComponentInChildren<UpgradeMenuUI>().InitUpgradeMenu(_upgrades);
            }
    
    }
}
