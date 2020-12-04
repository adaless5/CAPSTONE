using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stun : State
{
    float _stunTime = 3.0f;
    Vector3 _spinningVector = new Vector3(0, 180.0f, 0);
    public Stun(GameObject enemy, Transform[] pp, Transform playerposition, NavMeshAgent nav, float stuntime) : base(enemy, pp, playerposition, nav)
    {
        _stateName = STATENAME.IDLE;
        _stunTime = stuntime;
        Debug.Log("Stunned");
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        _stunTime -= Time.deltaTime;
        _currentEnemy.transform.Rotate(_spinningVector * Time.deltaTime);
        if (_stunTime <= 0)
        {
            _nextState = new Idle(_currentEnemy, _patrolPoints, _playerPos, _navMeshAgent);
            _stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
