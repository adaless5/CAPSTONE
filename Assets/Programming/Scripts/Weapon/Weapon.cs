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
    [Header("Weapon Recoil")]
    [SerializeField]
    protected float m_recoilForce = 1.0f;
    [SerializeField, Tooltip("How quickly the weapon recoils")]
    protected float m_recoilInitialSpeed = 25f;
    [SerializeField, Tooltip("How far the weapon can recoil")]
    protected float m_maxRecoilDistance = 0.5f;
    [SerializeField, Tooltip("How quickly the weapon adjusts back to normal position")]
    protected float m_recoilReadjustSpeed = 10f;
    [Space]

   

    protected ALTPlayerController _playerController;
    protected float m_fireStart = 0.0f;
    protected bool bIsReloading = false;

    protected void Awake()
    {
        EventBroker.OnPlayerSpawned += InitializePlayer;
    }

    protected void InitializePlayer(GameObject player)
    {
        _playerController = player.GetComponent<ALTPlayerController>();
        //Debug.Log(_playerController);
    }

}

