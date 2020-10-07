using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (GameObject))]
public class EnemyTrigger : MonoBehaviour
{
    [Header("Set Enemy Spawning")]
    [SerializeField]
    GameObject Enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            EventBroker.CallSpawnEnemy(Enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
