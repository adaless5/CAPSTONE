using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        [Tooltip("This should be turned on if this pool will be considered for random item spawning.")]
        public bool bIsItemPool; 
    }

    public static ObjectPool Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<Pool> _pools;
    public Dictionary<string, Queue<GameObject>> _poolDictionary;

    void Start()
    {
        _poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in _pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            _poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if(!_poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " does not exist exist.");
            return null;
        }

        GameObject objectToSpawn = _poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        _poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if(_poolDictionary.ContainsKey(tag))
        {
            _poolDictionary[tag].Enqueue(objectToReturn);
            objectToReturn.SetActive(false);
        }    
    }
}
