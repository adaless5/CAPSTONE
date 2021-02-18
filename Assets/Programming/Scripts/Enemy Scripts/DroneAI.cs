using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    private State _currentState;
    public Transform[] _patrolPoints;
    private NavMeshAgent _navMeshAgent;
    GameObject _playerReference;

    private void Awake()
    {
        EventBroker.OnPlayerSpawned += EventStart;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (_navMeshAgent != null)
                _navMeshAgent.baseOffset += hit.distance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {



    }

    private void EventStart(GameObject player)
    {
        if (this != null)
        {
            _currentState = new DronePatrol(gameObject, _patrolPoints, player.transform, _navMeshAgent);
            _playerReference = player;
        }


    }

    // Update is called once per frame
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
