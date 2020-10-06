using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int amountToSpawn = 1;
    private void Awake()
    {
        EventBroker.SpawnEnemy += SpawnEnemy;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SpawnEnemy(GameObject enemyToSpawn)
    {
        Debug.Log("Spawning Enemy...");
        for (int i = 0; i < amountToSpawn; i++)
        {
            GameObject newEnemy = Instantiate(enemyToSpawn, transform);
            newEnemy.GetComponent<Rigidbody>().AddForce(new Vector3(0, UnityEngine.Random.Range(0, 50),0), ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
