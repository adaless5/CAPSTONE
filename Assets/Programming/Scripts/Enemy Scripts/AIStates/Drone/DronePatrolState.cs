using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DronePatrol : DroneState
{
    int _currentPatrolIndex = 0;
    Vector3 destination = Vector3.zero;
    public DronePatrol(GameObject enemy, Transform[] pp, Transform playerposition, NavMeshAgent nav) : base(enemy, pp, playerposition, nav)
    {
        _stateName = STATENAME.PATROL;

    }

    public override void Enter()
    {
        base.Enter();
        destination = _patrolPoints[_currentPatrolIndex].transform.position;
    }

    public override void Update()
    {
        base.Update();
        //if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < 0.5f)

        if (Vector3.Distance(_currentEnemy.transform.position, destination) < 2)
            MoveToNextPoint();

        if (CanSeePlayer())
        {
            //_navMeshAgent.ResetPath();
            _nextState = new DroneAttack(_currentEnemy, _patrolPoints, _playerPos, _navMeshAgent);
            _stage = EVENT.EXIT;
        }
        
        _currentEnemy.transform.position = Vector3.MoveTowards(_currentEnemy.transform.position, destination, _enemySpeed * Time.deltaTime);
        LookAt(_patrolPoints[_currentPatrolIndex]);

    }

    void MoveToNextPoint()
    {
        if(_patrolPoints.Length == 0)
        {
            Debug.LogError("No Patrol Points Set!");
            return;
        }

        destination = _patrolPoints[_currentPatrolIndex].transform.position;

        //_navMeshAgent.destination = _patrolPoints[_currentPatrolIndex].transform.position;

        _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Length;
    }


    public override void Exit()
    {
        base.Exit();
    }
}
