using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamerPatrolState : RoamerState
{
    int _patrolIndex = 0;
    bool bFirstPatrol;
    bool bShouldPatrol;

    public RoamerPatrolState(GameObject enemy, Transform[] pp, Transform playerposition, NavMeshAgent nav) : base(enemy, pp, playerposition, nav)
    {
        _stateName = STATENAME.PATROL;       
    }

    public override void Enter()
    {
        base.Enter();

        bFirstPatrol = true;
        MoveToNextPoint();
        Debug.Log("Enemy Patrolling");
        //_navMeshAgent.speed = 2.0f;
    }

    public override void Update()
    {
        base.Update();
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < 0.9f)
        {
            //MoveToNextPoint();
            _nextState = new RoamerIdleState(_currentEnemy, _patrolPoints, _playerPos, _navMeshAgent);
            _stage = EVENT.EXIT;
        }

        if (Vector3.Distance(_currentEnemy.transform.position, _playerPos.transform.position) < 15.0f)
        {
            _navMeshAgent.ResetPath();
            //_navMeshAgent.speed = 4.0f;
            _nextState = new RoamerPursueState(_currentEnemy, _patrolPoints, _playerPos, _navMeshAgent);
            _stage = EVENT.EXIT;
        }
    }

    void MoveToNextPoint()
    {
        if (bFirstPatrol)
        {
            //_patrolIndex = Random.Range(0, _patrolPoints.Length);
            for(int i =0; i < _patrolPoints.Length; i++)
            {
                if(Vector3.Distance(_currentEnemy.transform.position, _patrolPoints[i].position) > 2f)
                {
                    _patrolIndex = i;
                }
            }
            bFirstPatrol = false;
        }
        if (_patrolPoints.Length == 0)
        {
            Debug.LogError(this + ": Roaming Enemy Patrol Points Not Set!");
            return;
        }
        _navMeshAgent.SetDestination(_patrolPoints[_patrolIndex].transform.position);
        _patrolIndex++;

        if (_patrolIndex > _patrolPoints.Length - 1)
            _patrolIndex = 0;        
    }
}
