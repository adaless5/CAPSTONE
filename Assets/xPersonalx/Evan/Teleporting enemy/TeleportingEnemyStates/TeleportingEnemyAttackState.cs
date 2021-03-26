using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportingEnemyAttackState : TeleportingEnemyState
{
    // Start is called before the first frame update
    float _attackTime;
    bool _isAttacking;
    public TeleportingEnemyAttackState(TeleportingEnemyAI thisEnemy, Transform playerPosition, TeleportingEnemyAnimation enemyAnimation, TeleportingEnemyAttack enemyAttack) : base(thisEnemy, playerPosition, enemyAnimation, enemyAttack)
    {
        _stateName = STATENAME.ATTACK;
        _teleportTime = Random.Range(_thisEnemy._attackTeleportTimeRange.x, _thisEnemy._attackTeleportTimeRange.y);
        _enemyAnimation._PlayerSpotted = true;
        _attackTime = Random.Range(_thisEnemy._attackTimeRange.x, _thisEnemy._attackTimeRange.y);
        _attack = enemyAttack;
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        Attack();
        if(!_attack._isFiring)
        {
            _thisEnemy.LookTowards(_thisEnemy.transform, _playerPosition.position, _thisEnemy._lookSpeed);
        }
        if (!_attack._isAttacking)
        {
            if (!_thisEnemy._hasDisappeared)
            {
                CheckStates();
            }
        }
    }
    void Maneuver()
    {
        if (_teleportTime < 0.0f)
        {
            _thisEnemy.Teleport(_playerPosition.position + (Random.insideUnitSphere.normalized * 10.0f));
            if (_thisEnemy._hasDisappeared == false)
            {
                _teleportTime = Random.Range(_thisEnemy._attackTeleportTimeRange.x, _thisEnemy._attackTeleportTimeRange.y);
            }
        }
        else
        {
            _teleportTime -= Time.deltaTime;
        }
    }
    void CheckStates()
    {
        if (Vector3.Distance(_playerPosition.position, _currentEnemy.transform.position) > _thisEnemy._followDistance)
        {
            _nextState = new TeleportingEnemyIdleState(_thisEnemy, _playerPosition, _enemyAnimation, _attack);
            _stage = EVENT.EXIT;
        }
        else if (Vector3.Distance(_playerPosition.position, _currentEnemy.transform.position) > _thisEnemy._attackDistance
                && Vector3.Distance(_playerPosition.position, _currentEnemy.transform.position) < _thisEnemy._followDistance)
        {
            _nextState = new TeleportingEnemyFollowState(_thisEnemy, _playerPosition, _enemyAnimation, _attack);
            _stage = EVENT.EXIT;
        }
    }
    public void Attack()
    {
        if (_attackTime < 0.0f)
        {
            if (!_isAttacking)
            {
                _thisEnemy.Attack();
                _isAttacking = true;
            }

            if (!_attack._isAttacking)
            {
                _attackTime = Random.Range(_thisEnemy._attackTimeRange.x, _thisEnemy._attackTimeRange.y);
                _isAttacking = false;
            }

        }
        else
        {
            _attackTime -= Time.deltaTime;
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
