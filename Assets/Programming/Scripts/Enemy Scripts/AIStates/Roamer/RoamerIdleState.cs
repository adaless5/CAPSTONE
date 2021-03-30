using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamerIdleState : RoamerState
{
    float _IdleTimer = 0.0f;
    State _previousState;
    public RoamerIdleState(GameObject enemy, Transform[] pp, Transform playerposition, NavMeshAgent nav) : base(enemy, pp, playerposition, nav)
    {
        _stateName = STATENAME.IDLE;
        _IdleTimer = Random.Range(7.0f, 10.0f);
        //_navMeshAgent.speed = 0f;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemy Idle");
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        _IdleTimer -= Time.deltaTime;

        if (CanSeePlayer())
        {
            _nextState = new RoamerPursueState(_currentEnemy, _patrolPoints, _playerPos, _navMeshAgent);
            _stage = EVENT.EXIT;
        }
        else if (_IdleTimer <= 0.0f)
        {
            //_navMeshAgent.ResetPath();
            //if(Vector3.Distance(_currentEnemy.transform.position, _playerPos.position) >= 15.0f)
            //{
            //    _nextState = new RoamerPatrolState(_currentEnemy, _patrolPoints, _playerPos, _navMeshAgent);
            //    _stage = EVENT.EXIT;
            //}
            //_navMeshAgent.speed = 2.0f;
            _nextState = new RoamerPatrolState(_currentEnemy, _patrolPoints, _playerPos, _navMeshAgent);
            _stage = EVENT.EXIT;
        }

    }
}
