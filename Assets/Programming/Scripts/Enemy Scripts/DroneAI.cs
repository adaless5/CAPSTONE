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

    bool hasAttacked = false;

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

        InitializingEnemyState();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void EventStart(GameObject player)
    {
        InitializingEnemyState();
    }

    // Update is called once per frame
    void Update()
    {
        //InitializingEnemyState();

        if (_currentState != null)
            _currentState = _currentState.Process();

        if (_currentState._stateName == DroneState.STATENAME.ATTACK)
        {
            try
            {
                AudioManager_Drone audioManager = GetComponent<AudioManager_Drone>();
                if (!audioManager.isPlaying() && !hasAttacked)
                {
                    audioManager.TriggerShot();
                }
            }
            catch { }
        }
        
    }

    public void SetCurrentDroneState(DroneState state)
    {
        _currentState = state;
    }

    public void Stun()
    {
        State savedState = _currentState;
        _currentState = new Stun(3.0f, savedState);
    }

    void InitializingEnemyState()
    {
        if (_playerReference == null)
        {
            try
            {
                _playerReference = ALTPlayerController.instance.gameObject;
            }
            catch { }

        }

        if (_playerReference != null)
            _currentState = new DronePatrol(gameObject, _patrolPoints, _playerReference.transform, _navMeshAgent);
    }
}
