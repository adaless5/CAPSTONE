using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class EventBroker
{
    public static event Action<GameObject> SpawnEnemy;
    public static event Action<GameObject> OnPlayerSpawned;
    public static event Action<int> OnPickupWeapon;
    public static event Action OnPlayerDeath;
    public static event Action<WeaponType, int> OnAmmoPickup;

    public static void CallSpawnEnemy(GameObject enemyToSpawn)
    {
        SpawnEnemy?.Invoke(enemyToSpawn);
    }

    public static void CallOnPlayerSpawned(GameObject player)
    {
        Debug.Log("Player has spawned");
        OnPlayerSpawned?.Invoke(player);
    }

    public static void CallOnPlayerSpawned(ref GameObject player)
    {
        Debug.Log("Player has spawned");
        OnPlayerSpawned?.Invoke(player);
    }

    public static void CallOnPickupWeapon(int weaponIndex)
    {
        OnPickupWeapon?.Invoke(weaponIndex);
    }

    public static void CallOnAmmoPickup(WeaponType type, int clipAmount)
    {
        OnAmmoPickup?.Invoke(type, clipAmount);
    }

    public static void CallOnPlayerDeath()
    {
        OnPlayerDeath?.Invoke();
    }

}
