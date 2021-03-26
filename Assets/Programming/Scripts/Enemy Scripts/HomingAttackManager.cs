using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingAttackManager : MonoBehaviour
{
    public static HomingAttackManager instance;
    GameObject _haRef;

    private void Awake()
    {
        instance = this;
        _haRef = (GameObject)Resources.Load("Prefabs/Enemies/Boss/HA");
    }

    public void SpawnHomingAttack(Vector3 spawnPos)
    {

        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.SpawnFromPool("Homing Attack", _haRef, spawnPos, Quaternion.identity);
            Debug.Log("HA spawned from pool");

        }
        else
            Debug.LogError("ObjectPool not Initialized! Please Drop a pool in the current scene (can be found at Assets/Design/Resources/Prefabs/Object Pool/Object Pool.prefab)");

    }
}
