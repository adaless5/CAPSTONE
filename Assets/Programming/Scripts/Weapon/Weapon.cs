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
public enum EUpgradeType
{
    Upgrade_1 = 0,
    Upgrade_2,
    Upgrade_3,
    Action,
    NumUpgrades
}
public abstract class Weapon : Tool
{


    [Header("Weapon Selected")]
    [SerializeField]
    public WeaponType _weapon;

    [Header("Damage Settings")]
    [SerializeField]
    protected float m_damageAmount = 10.0f;

    [Header("Weapon Settings")]
    [SerializeField]
    protected int m_weaponClipSize = 6;    
    protected int m_startingOverstockAmmo = 0;
    [SerializeField]
    protected int m_ammoCapAmount = 20;

    [SerializeField]
    protected float m_reloadTime = 2.0f;
    [SerializeField]
    protected float m_fireRate = 10.0f;
    [SerializeField]
    protected float m_hitImpact = 50.0f;
    [SerializeField]
    protected float m_weaponRange = 50.0f;
    [Header("Explosive Weapon Specific")]
    [SerializeField]
    protected float m_projectileLifeTime = 3.0f;
    [SerializeField]
    protected float m_blastradius = 10.0f;
    [SerializeField]
    protected float m_blastforce = 2000.0f; //might not be needed, can use hit impact
    [SerializeField]
    protected float m_projectileforce = 800.0f; //force of a lauched projectile
    [Header("Creture Weapon Specific")]
    [SerializeField]
    protected float m_maxDamageTime;
    [Space]

    protected WeaponUpgrade m_upgradestats;
    public List<EUpgradeType> m_currentupgrades;

    protected ALTPlayerController _playerController;
    [SerializeField]
    protected AmmoController _ammoController;
    protected float m_fireStart = 0.0f;
    protected bool bIsReloading = false;

    protected bool m_bHasActionUpgrade = false;

    protected void Awake()
    {
        LoadDataOnSceneEnter();
        EventBroker.OnPlayerSpawned += InitializePlayer;
        m_currentupgrades = new List<EUpgradeType>();
        m_upgradestats.SetToDefault();
    }

    protected void InitializePlayer(GameObject player)
    {
        _playerController = player.GetComponent<ALTPlayerController>();
        //Debug.Log(_playerController);
    }

    public abstract void AddUpgrade(WeaponUpgrade upgrade);
    //public abstract void RemoveUpgrade(WeaponScalars scalars);
}

[System.Serializable]
public struct WeaponUpgrade
{
    public float Damage;
    public float ClipSize;
    public float AmmoReserveSize;
    public float ReloadTime;
    public float FireRate;
    public float ImpactForce;
    public float Range;
    public float FuzeTime;
    public float BlastRadius;
    public float ProjectileForce;
    public float DamageTime;
    public bool HasAction;
    public EUpgradeType Type;
    public string Title;
    public string Discription;
    public int UpgradeWorth;

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
        DamageTime = 1;
        UpgradeWorth = 0;
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
        DamageTime = 0;
    }

    public static WeaponUpgrade operator +(WeaponUpgrade a, WeaponUpgrade b)
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
        a.DamageTime += b.DamageTime;

        return a;
    }

    public static WeaponUpgrade operator -(WeaponUpgrade a, WeaponUpgrade b)
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
        a.DamageTime -= b.DamageTime;


        return a;
    }
    
}

