using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Idle : State
{
    public Idle(GameObject enemy, Transform[] pp, Transform playerposition, NavMeshAgent nav) : base(enemy, pp, playerposition, nav)
    {
        _stateName = STATENAME.IDLE;

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (CanSeePlayer())
        {
            _nextState = new Attack(_currentEnemy, _patrolPoints, _playerPos, _navMeshAgent);
            _stage = EVENT.EXIT;
        }
        else
        {
            _nextState = new Patrol(_currentEnemy, _patrolPoints, _playerPos, _navMeshAgent);
            _stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
