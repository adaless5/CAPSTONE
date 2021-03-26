using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportingEnemyIdleState : TeleportingEnemyState
{
    // Start is called before the first frame update
    public TeleportingEnemyIdleState(TeleportingEnemyAI thisEnemy, Transform playerPosition, TeleportingEnemyAnimation enemyAnimation, TeleportingEnemyAttack enemyAttack) : base(thisEnemy, playerPosition,enemyAnimation, enemyAttack)
    {
        _stateName = STATENAME.IDLE;
    }
    public override void Enter()
    {
        base.Enter();
        _enemyAnimation._PlayerSpotted = false;

    }

    public override void Update()
    {
        base.Update(); 
        if(!_thisEnemy._hasDisappeared)
        CheckStates();
        
    }
    void CheckStates()
    {
        if(Vector3.Distance(_playerPosition.position,_currentEnemy.transform.position)<_thisEnemy._followDistance)
        {
            _nextState = new TeleportingEnemyFollowState(_thisEnemy, _playerPosition, _enemyAnimation, _attack);
            _stage = EVENT.EXIT;
        }
        else if(Vector3.Distance(_playerPosition.position, _currentEnemy.transform.position) < _thisEnemy._attackDistance)
        {
            _nextState = new TeleportingEnemyAttackState(_thisEnemy, _playerPosition, _enemyAnimation, _attack);
            _stage = EVENT.EXIT;
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}