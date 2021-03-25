using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapingEnemyAttackBox : MonoBehaviour
{
    // Start is called before the first frame update
    LeapingEnemyAI _owningEnemy;
    // Start is called before the first frame update
    void Start()
    {
        _owningEnemy = transform.parent.gameObject.GetComponent<LeapingEnemyAI>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _owningEnemy._playerDistance = LeapingEnemyAI.PlayerDistance.attack;
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            _owningEnemy._playerDistance = LeapingEnemyAI.PlayerDistance.follow;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (_owningEnemy == null)
        {
            _owningEnemy = transform.parent.gameObject.GetComponent<LeapingEnemyAI>();
        }
    }
}
