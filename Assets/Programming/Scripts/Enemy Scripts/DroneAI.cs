using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    private State _currentState;
    public Transform[] _patrolPoints;
    private NavMeshAgent _navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        EventBroker.OnPlayerSpawned += EventStart;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
    }

    private void EventStart(GameObject player)
    {
        _currentState = new Patrol(gameObject, _patrolPoints, player.transform, _navMeshAgent);
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentState != null)
            _currentState = _currentState.Process();
    }
}
