using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    BaseWeapon,
    GrenadeWeapon,
    CreatureWeapon,
    NumberOfWeapons
}
public abstract class Weapon : Tool
{

    [Header("Weapon Selected")]
    [SerializeField]
    private WeaponType _weapon;

    [Header("Damage Settings")]
    [SerializeField]
    protected float m_damageAmount = 10.0f;

    [Header("Weapon Settings")]
    [SerializeField]
    protected int m_ammoAmount = 6; //not being used currently
    [SerializeField]
    protected float m_fireRate = 10.0f;
    [SerializeField]
    protected float m_hitImpact = 50.0f;
    [SerializeField]
    protected float m_weaponRange = 50.0f;
    [Space]

    protected float m_fireStart = 0.0f;
}
