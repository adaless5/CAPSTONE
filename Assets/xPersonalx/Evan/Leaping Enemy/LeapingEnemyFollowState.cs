using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeapingEnemyFollowState : LeapingEnemyState
{
    // Start is called before the first frame update
    protected float _currentFollowJumpTime;
    public LeapingEnemyFollowState(GameObject enemy, LeapingEnemyAI thisLeapingEnemy, Transform playerposition) : base(enemy, thisLeapingEnemy, playerposition)
    {
        _stateName = STATENAME.FOLLOW;
        _currentFollowJumpTime = Random.Range(_thisLeapingEnemy._fullFollowJumpTimeRange.x, _thisLeapingEnemy._fullFollowJumpTimeRange.y);
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        _thisLeapingEnemy._jumpTarget.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        _thisLeapingEnemy.LookTowards(_thisLeapingEnemy.transform, _thisLeapingEnemy._jumpTarget.transform.position, 8.0f);

        if (_currentFollowJumpTime > 0.0f)
        {
            _currentFollowJumpTime -= Time.deltaTime;
        }
        else if (_thisLeapingEnemy.IsGrounded())
        {

            _thisLeapingEnemy.Leap(_thisLeapingEnemy._AttackJumpSpeed);

            _currentFollowJumpTime = Random.Range(_thisLeapingEnemy._fullFollowJumpTimeRange.x, _thisLeapingEnemy._fullFollowJumpTimeRange.y);
        }
        if(_thisLeapingEnemy._playerDistance ==LeapingEnemyAI.PlayerDistance.far)
        {
            _nextState = new LeapingEnemyWanderState(_currentEnemy, _thisLeapingEnemy, _playerPos);
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
