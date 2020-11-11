using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum SpawnerType
    {
        SpawnOnce,
        SpawnIndefinitely,
        RandomItemSpawn,
    }

    public SpawnerType _spawnerType;
    public string[] _ObjectPoolTags;
    ObjectPool _ObjectPool;

    Vector3 _Position;
    Quaternion _Rotation;

    [SerializeField] bool bSpawnOnAwake = true;
    bool bBeginSpawning;
    public float _IntervalBetweenSpawn = 3.0f;
    float _Timer = 0.0f;

    void Start()
    {
        _ObjectPool = ObjectPool.Instance;

        if (bSpawnOnAwake) { bBeginSpawning = true; }
        else if (!bSpawnOnAwake) { bBeginSpawning = false; }

        _Position = transform.position;
        _Rotation = transform.rotation;
    }

    void Update()
    {
        if (bBeginSpawning)
        {
            _Timer += Time.deltaTime;

            if (_Timer >= _IntervalBetweenSpawn)
            {
                SpawnFromTags();
                _Timer = 0.0f;
            }
        }
    }

    public void Spawn()
    {
        switch(_spawnerType)
        {
            case SpawnerType.SpawnOnce:
                SpawnFromTags();
                break;

            case SpawnerType.SpawnIndefinitely:
                bBeginSpawning = true;
                break;

            case SpawnerType.RandomItemSpawn:
                SpawnRandomItem();
                break;
        }
    }

    public void Spawn(Vector3 position, Quaternion rotation)
    {
        _Position = position;
        _Rotation = rotation;

        switch (_spawnerType)
        {
            case SpawnerType.SpawnOnce:
                SpawnFromTags();
                break;

            case SpawnerType.SpawnIndefinitely:
                bBeginSpawning = true;
                break;

            case SpawnerType.RandomItemSpawn:
                SpawnRandomItem();
                break;

        }
    }

    void SpawnFromTags()
    {
        foreach (string tag in _ObjectPoolTags)
        {
            _ObjectPool.SpawnFromPool(tag, _Position, _Rotation);
        }
    }

    void SpawnRandomItem()
    {
        int RandNumMax = _ObjectPool._pools.Count;
        int RandNum = 0;

        do
        {
            RandNum = Random.Range(0, RandNumMax);
        }
        while (_ObjectPool._pools[RandNum].bIsItemPool != true);

        _ObjectPool.SpawnFromPool(_ObjectPool._pools[RandNum].tag, _Position, _Rotation); 
    }
}
