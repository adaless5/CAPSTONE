using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapingEnemySightBox : MonoBehaviour
{
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

            _owningEnemy._playerDistance = LeapingEnemyAI.PlayerDistance.follow;
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {

            _owningEnemy._playerDistance = LeapingEnemyAI.PlayerDistance.far;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(_owningEnemy == null)
        {
            _owningEnemy = transform.parent.gameObject.GetComponent<LeapingEnemyAI>();
        }
    }
}