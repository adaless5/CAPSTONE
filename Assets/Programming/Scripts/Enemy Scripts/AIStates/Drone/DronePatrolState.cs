using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DronePatrol : DroneState
{
    int _currentPatrolIndex = 0;
    public DronePatrol(GameObject enemy, Transform[] pp, Transform playerposition, NavMeshAgent nav) : base(enemy, pp, playerposition, nav)
    {
        _stateName = STATENAME.PATROL;

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        //LookAt(_patrolPoints[_currentPatrolIndex]);
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < 0.5f)
            MoveToNextPoint();

        if (CanSeePlayer())
        {
            _navMeshAgent.ResetPath();
            _nextState = new DroneAttack(_currentEnemy, _patrolPoints, _playerPos, _navMeshAgent);
            _stage = EVENT.EXIT;
        }

    }

    void MoveToNextPoint()
    {
        if(_patrolPoints.Length == 0)
        {
            Debug.LogError("No Patrol Points Set!");
            return;
        }

        _navMeshAgent.destination = _patrolPoints[_currentPatrolIndex].transform.position;

        _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Length;
    }


    public override void Exit()
    {
        base.Exit();
    }
}
