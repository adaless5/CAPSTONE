using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (GameObject))]
public class EnemyTrigger : MonoBehaviour
{
    [Header("Set Enemy Spawning")]
    [SerializeField]
    GameObject Enemy;
    public Transform spawnLocation;

    Spawner spawner;
    // Start is called before the first frame update
    void Start()
    {
        spawner = GetComponent<Spawner>();
        //spawnLocation = GetComponentInChildren<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //EventBroker.CallSpawnEnemy(Enemy);
            spawner.Spawn(spawnLocation.position, spawnLocation.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
