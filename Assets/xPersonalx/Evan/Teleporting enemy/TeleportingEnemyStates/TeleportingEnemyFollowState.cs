using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportingEnemyFollowState : TeleportingEnemyState
{
    // Start is called before the first frame update
    public TeleportingEnemyFollowState(TeleportingEnemyAI thisEnemy, Transform playerPosition, TeleportingEnemyAnimation enemyAnimation, TeleportingEnemyAttack enemyAttack) : base(thisEnemy, playerPosition, enemyAnimation, enemyAttack)
    {
        _stateName = STATENAME.FOLLOW;
        _enemyAnimation._PlayerSpotted = true;
        _teleportTime = _thisEnemy._followTeleportTime;
    }
    public override void Enter()
    {
        base.Enter();
    }
    void CheckStates()
    {
        if (Vector3.Distance(_playerPosition.position, _currentEnemy.transform.position) > _thisEnemy._followDistance)
        {
            _nextState = new TeleportingEnemyIdleState(_thisEnemy, _playerPosition, _enemyAnimation, _attack);
            _stage = EVENT.EXIT;
        }
        else if (Vector3.Distance(_playerPosition.position, _thisEnemy.transform.position) < _thisEnemy._attackDistance)
        {
            _nextState = new TeleportingEnemyAttackState(_thisEnemy, _playerPosition, _enemyAnimation, _attack);
            _stage = EVENT.EXIT;
        }
    }
    public void Follow()
    {
        if (_teleportTime < 0.0f)
        {
            _thisEnemy.Teleport(_thisEnemy.transform.position + (_thisEnemy.transform.forward * _thisEnemy._teleportRange));
            if (_thisEnemy._hasDisappeared == false)
            {
                _teleportTime = _thisEnemy._followTeleportTime;
            }
        }
        else
        {
            _teleportTime -= Time.deltaTime;
        }
    }
    public override void Update()
    {
        base.Update();

        if (!_thisEnemy._hasDisappeared)
            CheckStates();
     
        _thisEnemy.LookTowards(_thisEnemy.transform, _playerPosition.position, _thisEnemy._lookSpeed);
        Follow();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
