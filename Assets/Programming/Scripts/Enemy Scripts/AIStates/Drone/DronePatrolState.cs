using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DronePatrol : DroneState
{
    int _currentPatrolIndex = 0;
    int _nextPatrolIndex = 1;
    Vector3 destination = Vector3.zero;
    public DronePatrol(GameObject enemy, Transform[] pp, Transform playerposition, NavMeshAgent nav) : base(enemy, pp, playerposition, nav)
    {
        _stateName = STATENAME.PATROL;

    }

    public override void Enter()
    {
        base.Enter();
        destination = _patrolPoints[_currentPatrolIndex].transform.position;
        //Debug.Log("Destination: " + destination);
    }

    public override void Update()
    {
        base.Update();

        if (CanSeePlayer())
        {
            //_navMeshAgent.ResetPath();
            _nextState = new DroneAttack(_currentEnemy, _patrolPoints, _playerPos, _navMeshAgent);
            _stage = EVENT.EXIT;
        }

        //_currentEnemy.transform.position = Vector3.MoveTowards(_currentEnemy.transform.position, destination, _enemySpeed * Time.deltaTime);
        // LookAt(_patrolPoints[_currentPatrolIndex]);
        _currentEnemy.transform.position += _currentEnemy.transform.forward * _enemySpeed * Time.deltaTime;
        LookTowards(_currentEnemy.transform, _patrolPoints[_currentPatrolIndex].position, 2.0f);
        if (Vector3.Distance(_currentEnemy.transform.position, destination) < 2)
        {
            MoveToNextPoint();
        }
    }

    void MoveToNextPoint()
    {
        if(_patrolPoints.Length == 0)
        {
            Debug.LogError("No Patrol Points Set!");
            return;
        }

        _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Length;

        destination = _patrolPoints[_currentPatrolIndex].transform.position;
        
        //_navMeshAgent.destination = _patrolPoints[_currentPatrolIndex].transform.position;

    }


    public override void Exit()
    {
        base.Exit();
    }
}
