using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeapingEnemyWanderState : LeapingEnemyState
{
    protected float _currentIdleTime;
    public LeapingEnemyWanderState(GameObject enemy, LeapingEnemyAI thisLeapingEnemy, Transform playerposition) : base(enemy,thisLeapingEnemy,playerposition)
    {
        _stateName = STATENAME.IDLE;
        _currentIdleTime = Random.Range(_thisLeapingEnemy._fullIdleTimeRange.x, _thisLeapingEnemy._fullIdleTimeRange.y); 
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        _thisLeapingEnemy.LookTowards(_thisLeapingEnemy.transform, _thisLeapingEnemy._jumpTarget.transform.position, 4.0f);
        if (_currentIdleTime > 0.0f)
        {
            _currentIdleTime -= Time.deltaTime;
        }
        else if (_thisLeapingEnemy.IsGrounded())
        {
            if (Vector3.Distance(_thisLeapingEnemy.transform.position, _thisLeapingEnemy._homeObject.transform.position) > _thisLeapingEnemy._homeObject.transform.localScale.x)
            {
                _nextState = new LeapingEnemyReturnHomeState(_currentEnemy, _thisLeapingEnemy, _playerPos);
                _stage = EVENT.EXIT;
            }
            else
            {                
                _thisLeapingEnemy.Leap(_thisLeapingEnemy._WanderJumpSpeed);
                _thisLeapingEnemy._jumpTarget.transform.position = _thisLeapingEnemy._homeObject.transform.position + (Random.insideUnitSphere * _thisLeapingEnemy._homeObject.transform.localScale.x);
                _thisLeapingEnemy._jumpTarget.transform.position =new Vector3(_thisLeapingEnemy._jumpTarget.transform.position.x, _thisLeapingEnemy._homeObject.transform.position.y, _thisLeapingEnemy._jumpTarget.transform.position.z);
            }
            _currentIdleTime = Random.Range(_thisLeapingEnemy._fullIdleTimeRange.x, _thisLeapingEnemy._fullIdleTimeRange.y);

        }
        if (_thisLeapingEnemy._playerDistance == LeapingEnemyAI.PlayerDistance.follow)
            {
                _nextState = new LeapingEnemyFollowState(_currentEnemy, _thisLeapingEnemy, _playerPos);
                _stage = EVENT.EXIT;
            }
            else if (_thisLeapingEnemy._playerDistance == LeapingEnemyAI.PlayerDistance.attack)
            {
                _nextState = new LeapingEnemyAttackState(_currentEnemy, _thisLeapingEnemy, _playerPos);
                _stage = EVENT.EXIT;
            }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
