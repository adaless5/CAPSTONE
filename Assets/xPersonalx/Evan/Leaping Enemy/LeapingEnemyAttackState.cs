using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeapingEnemyAttackState : LeapingEnemyState
{
    protected float _currentFollowJumpTime;
    public LeapingEnemyAttackState(GameObject enemy, LeapingEnemyAI thisLeapingEnemy, Transform playerposition) : base(enemy, thisLeapingEnemy, playerposition)
    {
        _stateName = STATENAME.ATTACK;
        _currentFollowJumpTime = Random.Range(_thisLeapingEnemy._fullFollowJumpTimeRange.x, _thisLeapingEnemy._fullFollowJumpTimeRange.y);
    }
    public override void Enter()
    {
        base.Enter();

    }

    public override void Update()
    {
        base.Update();
        if(GameObject.FindGameObjectWithTag("Player") != null)
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
        if (_thisLeapingEnemy._LeapingEnemyProjectile.gameObject.activeSelf == false)
        {
            _thisLeapingEnemy._LeapingEnemyProjectile.gameObject.SetActive(true);
            _thisLeapingEnemy._LeapingEnemyProjectile.Fire();
            _thisLeapingEnemy._LeapingEnemyProjectile.transform.position = _thisLeapingEnemy.transform.position + (_thisLeapingEnemy.transform.forward * 2.0f);
            _thisLeapingEnemy._LeapingEnemyProjectile._rigidBody.velocity = _thisLeapingEnemy._ProjectileBaseSpeed * (_thisLeapingEnemy._jumpTarget.transform.position - _thisLeapingEnemy.transform.position + _thisLeapingEnemy._ProjectileSpawnPoint.transform.forward).normalized;

        }
        if (_thisLeapingEnemy._playerDistance == LeapingEnemyAI.PlayerDistance.far)
        {
            _nextState = new LeapingEnemyWanderState(_currentEnemy, _thisLeapingEnemy, _playerPos);
            _stage = EVENT.EXIT;
        }
        if (_thisLeapingEnemy._playerDistance == LeapingEnemyAI.PlayerDistance.follow)
        {
            _nextState = new LeapingEnemyFollowState(_currentEnemy, _thisLeapingEnemy, _playerPos);
            _stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
