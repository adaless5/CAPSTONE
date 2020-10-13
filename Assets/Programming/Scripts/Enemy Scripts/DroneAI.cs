using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAI : MonoBehaviour
{
    private State _currentState;
    public Transform[] _patrolPoints;
    // Start is called before the first frame update
    void Start()
    {
        EventBroker.OnPlayerSpawned += EventStart;
    }

    private void EventStart(GameObject player)
    {
        _currentState = new Patrol(gameObject, _patrolPoints, player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentState != null)
            _currentState = _currentState.Process();
    }
}
