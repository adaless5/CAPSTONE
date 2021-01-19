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
    protected int m_weaponClipSize = 6;
    protected int m_currentAmmoCount;
    protected int m_overallAmmoCount;

    [SerializeField]
    protected float m_reloadTime = 2.0f;
    [SerializeField]
    protected float m_fireRate = 10.0f;
    [SerializeField]
    protected float m_hitImpact = 50.0f;
    [SerializeField]
    protected float m_weaponRange = 50.0f;
    [SerializeField]
    protected float m_projectileLifeTime = 3.0f;
    [SerializeField]
    protected float m_blastradius = 10.0f;
    [SerializeField]
    protected float m_blastforce = 2000.0f; //might not be needed, can use hit impact
    [SerializeField]
    protected float m_projectileforce = 800.0f; //force of a lauched projectile
    [SerializeField]
    protected float m_maxDamageTime;
    [Space]

    protected WeaponScalars m_scalars;


    protected ALTPlayerController _playerController;
    protected float m_fireStart = 0.0f;
    protected bool bIsReloading = false;

    protected void Awake()
    {
        EventBroker.OnPlayerSpawned += InitializePlayer;
        m_scalars.SetToDefault();
    }

    protected void InitializePlayer(GameObject player)
    {
        _playerController = player.GetComponent<ALTPlayerController>();
        //Debug.Log(_playerController);
    }

    public abstract void AddUpgrade(WeaponScalars scalars);
    //public abstract void RemoveUpgrade(WeaponScalars scalars);
}

[System.Serializable]
public struct WeaponScalars
{
    public float Damage;
    public int ClipSize;
    public float AmmoReserveSize;
    public float ReloadTime;
    public float FireRate;
    public float ImpactForce;
    public float Range;
    public float FuzeTime;
    public float BlastRadius;
    public float ProjectileForce;

    public void SetToDefault()
    {
        Damage = 1;
        ClipSize = 1;
        AmmoReserveSize = 1;
        ReloadTime = 1;
        FireRate = 1;
        ImpactForce = 1;
        Range = 1;
        FuzeTime = 1;
        BlastRadius = 1;
        ProjectileForce = 1;
    }

    public void SetToZero()
    {
        Damage = 0;
        ClipSize = 0;
        AmmoReserveSize = 0;
        ReloadTime = 0;
        FireRate = 0;
        ImpactForce = 0;
        Range = 0;
        FuzeTime = 0;
        BlastRadius = 0;
        ProjectileForce = 0;
    }

    public static WeaponScalars operator +(WeaponScalars a, WeaponScalars b)
    {
        a.Damage += b.Damage;
        a.ClipSize += b.ClipSize;
        a.AmmoReserveSize += b.AmmoReserveSize;
        a.ReloadTime += b.ReloadTime;
        a.FireRate += b.FireRate;
        a.ImpactForce += b.ImpactForce;
        a.Range += b.Range;
        a.FuzeTime += b.FuzeTime;
        a.BlastRadius += b.BlastRadius;
        a.ProjectileForce += b.ProjectileForce;

        return a;
    }

    public static WeaponScalars operator -(WeaponScalars a, WeaponScalars b)
    {
        a.Damage -= b.Damage;
        a.ClipSize -= b.ClipSize;
        a.AmmoReserveSize -= b.AmmoReserveSize;
        a.ReloadTime -= b.ReloadTime;
        a.FireRate -= b.FireRate;
        a.ImpactForce -= b.ImpactForce;
        a.Range -= b.Range;
        a.FuzeTime -= b.FuzeTime;
        a.BlastRadius -= b.BlastRadius;
        a.ProjectileForce -= b.ProjectileForce;

        return a;
    }
}

