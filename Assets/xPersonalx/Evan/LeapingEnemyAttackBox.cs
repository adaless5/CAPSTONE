using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapingEnemyAttackBox : MonoBehaviour
{
    // Start is called before the first frame update
    LeapingEnemy _owningEnemy;
    // Start is called before the first frame update
    void Start()
    {
        _owningEnemy = transform.parent.gameObject.GetComponent<LeapingEnemy>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _owningEnemy._behaviourState = LeapingEnemy.BehaviourState.Attack;
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            _owningEnemy._behaviourState = LeapingEnemy.BehaviourState.Follow;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (_owningEnemy == null)
        {
            _owningEnemy = transform.parent.gameObject.GetComponent<LeapingEnemy>();
        }
    }
}
