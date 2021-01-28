using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeAltar : MonoBehaviour
{
    [Header("Deafault Upgrades")]
    [SerializeField]
    WeaponScalars DeafaultUpgrade1;
    [SerializeField]
    WeaponScalars DeafaultUpgrade2;
    [SerializeField]
    WeaponScalars DeafaultUpgrade3;

    [Header("Explosive Upgrades")]
    [SerializeField]
    WeaponScalars ExplosiveUpgrade1;
    [SerializeField]
    WeaponScalars ExplosiveUpgrade2;
    [SerializeField]
    WeaponScalars ExplosiveUpgrade3;

    [Header("Creature Upgrades")]
    [SerializeField]
    WeaponScalars CreatureUpgrade1;
    [SerializeField]
    WeaponScalars CreatureUpgrade2;
    [SerializeField]
    WeaponScalars CreatureUpgrade3;

    //dont need variable for action, its just a bool

    List<WeaponScalars> _upgrades;
    private void Start()
    {
        _upgrades = new List<WeaponScalars>();
        _upgrades.Add(DeafaultUpgrade1);
        _upgrades.Add(DeafaultUpgrade2);
        _upgrades.Add(DeafaultUpgrade3);
        _upgrades.Add(ExplosiveUpgrade1);
        _upgrades.Add(ExplosiveUpgrade2);
        _upgrades.Add(ExplosiveUpgrade3);
        _upgrades.Add(CreatureUpgrade1);
        _upgrades.Add(CreatureUpgrade2);
        _upgrades.Add(CreatureUpgrade3);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponentInChildren<UpgradeMenuUI>())
        {
            other.gameObject.GetComponentInChildren<UpgradeMenuUI>().Activate();
            other.gameObject.GetComponentInChildren<UpgradeMenuUI>().InitUpgradeMenu(_upgrades);
        }
    }
}
