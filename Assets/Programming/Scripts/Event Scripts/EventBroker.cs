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
    public static event Action<WeaponType, int, int> OnAmmoPickup;
    public static event Action<float> OnHealthPickup;
    public static event Action OnAmmoPickupAttempt;
    public static event Action<bool> OnHealthPickupAttempt;
    public static event Action OnGameEnd;
    public static event Action OnWeaponSwap;
    public static event Action OnWeaponSwapIn;
    public static event Action<SaveSystem.RespawnInfo_Data> OnLoadingScreenFinished;
    public static void CallSpawnEnemy(GameObject enemyToSpawn)
    {
        SpawnEnemy?.Invoke(enemyToSpawn);
    }

    public static void CallOnGameEnd()
    {
        OnGameEnd?.Invoke();
    }

    public static void CallOnPlayerSpawned(GameObject player)
    {
        //Debug.Log("Player has spawned");
        OnPlayerSpawned?.Invoke(player);
    }

    public static void CallOnPlayerSpawned(ref GameObject player)
    {
        //Debug.Log("Player has spawned");
        OnPlayerSpawned?.Invoke(player);
    }

    public static void CallOnLoadingScreenFinished(SaveSystem.RespawnInfo_Data respawninfo)
    {
        Debug.Log("Loading Screen Finished");
        OnLoadingScreenFinished?.Invoke(respawninfo);
    }

    public static void CallOnPickupWeapon(int weaponIndex)
    {
        OnPickupWeapon?.Invoke(weaponIndex);
    }

    public static void CallOnAmmoPickup(WeaponType type, int clipAmount, int ammoCap)
    {
        OnAmmoPickup?.Invoke(type, clipAmount, ammoCap);
    }

    public static void CallOnHealthPickup(float healthValue)
    {
        OnHealthPickup?.Invoke(healthValue);
    }

    public static void CallOnAmmoPickupAttempt()
    {
        OnAmmoPickupAttempt?.Invoke();
    }

    public static void CallOnHealthPickupAttempt(bool healthFull)
    {
        OnHealthPickupAttempt?.Invoke(healthFull);
    }

    public static void CallOnPlayerDeath()
    {
        OnPlayerDeath?.Invoke();
    }

    public static void CallOnWeaponSwapOut()
    {
        OnWeaponSwap?.Invoke();
    }

    public static void CallOnWeaponSwapIn()
    {
        OnWeaponSwapIn?.Invoke();
    }

}
