using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBroker
{
    public static event Action<GameObject> SpawnEnemy;
    public static event Action<GameObject> OnPlayerSpawned;
    public static event Action<int> OnPickupWeapon;


    public static void CallSpawnEnemy(GameObject enemyToSpawn)
    {
        SpawnEnemy?.Invoke(enemyToSpawn);
    }
    public static void CallOnPlayerSpawned(GameObject player)
    {
        OnPlayerSpawned?.Invoke(player);
    }

    public static void CallOnPickupWeapon(int weaponIndex)
    {
        OnPickupWeapon?.Invoke(weaponIndex);
    }

}
