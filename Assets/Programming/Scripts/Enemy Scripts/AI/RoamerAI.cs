using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamerAI : MonoBehaviour
{
    private State _currentState;
    public Transform[] _patrolPoints;
    private NavMeshAgent _navMeshAgent;
    GameObject _playerReference;
    public bool bShouldWanderRandomly = false;

    private void Awake()
    {
        EventBroker.OnPlayerSpawned += EventStart;
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
     
    }

    private void EventStart(GameObject player)
    {
        if (this != null)
        {
            if(bShouldWanderRandomly)
                _currentState = new RoamerWanderState(gameObject, _patrolPoints, player.transform, _navMeshAgent);

            else if(!bShouldWanderRandomly)
                _currentState = new RoamerPatrolState(gameObject, _patrolPoints, player.transform, _navMeshAgent);

            _playerReference = player;
        }
    }

    void Update()
    {
        if (_currentState != null)
            _currentState = _currentState.Process();
    }

    public void Stun()
    {
        State savedState = _currentState;
        _currentState = new Stun(3.0f, savedState);
    }
}
