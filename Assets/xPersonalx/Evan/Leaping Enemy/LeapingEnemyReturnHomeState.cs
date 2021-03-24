using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeapingEnemyReturnHomeState : LeapingEnemyState
{
    // Start is called before the first frame update
    float _currentGoHomeJumpTime;
    public LeapingEnemyReturnHomeState(GameObject enemy, LeapingEnemyAI thisLeapingEnemy, Transform playerposition) : base(enemy, thisLeapingEnemy, playerposition)
    {
        _stateName = STATENAME.PATROL;
        _currentGoHomeJumpTime = Random.Range(thisLeapingEnemy._fullGoHomeJumpTimeRange.x, thisLeapingEnemy._fullGoHomeJumpTimeRange.y);
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        _thisLeapingEnemy._jumpTarget.transform.position = _thisLeapingEnemy._homeObject.transform.position;
        _thisLeapingEnemy.LookTowards(_thisLeapingEnemy.transform, _thisLeapingEnemy._jumpTarget.transform.position,5.0f);
        if (_currentGoHomeJumpTime > 0.0f)
        {
            _currentGoHomeJumpTime -= Time.deltaTime;
        }
        else
        {
            if (Vector3.Distance(_thisLeapingEnemy.transform.position, _thisLeapingEnemy._homeObject.transform.position) < _thisLeapingEnemy._homeObject.transform.localScale.x)
            {
                _nextState = new LeapingEnemyWanderState(_currentEnemy, _thisLeapingEnemy, _playerPos);
                _stage = EVENT.EXIT;
            }
            else if (_thisLeapingEnemy.IsGrounded())
            {
                _thisLeapingEnemy.Leap(_thisLeapingEnemy._WanderJumpSpeed);
                _currentGoHomeJumpTime = Random.Range(_thisLeapingEnemy._fullGoHomeJumpTimeRange.x, _thisLeapingEnemy._fullGoHomeJumpTimeRange.y);
            }

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
