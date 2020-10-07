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
        _currentState = new Patrol(gameObject, _patrolPoints, GameObject.FindGameObjectWithTag("Player").transform);
    }

    // Update is called once per frame
    void Update()
    {
        _currentState = _currentState.Process();
    }
}
