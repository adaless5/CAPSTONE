using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportingEnemyState : State
{
    // Start is called before the first frame update
    protected TeleportingEnemyAI _thisEnemy;
    protected Transform _playerPosition;
    protected TeleportingEnemyAnimation _enemyAnimation;
    protected float _teleportTime;
    protected TeleportingEnemyAttack _attack;
public TeleportingEnemyState(TeleportingEnemyAI thisEnemy, Transform playerPosition, TeleportingEnemyAnimation enemyAnimation, TeleportingEnemyAttack enemyAttack)
    {
        _thisEnemy = thisEnemy;
        _playerPosition = playerPosition;
        _enemyAnimation = enemyAnimation;
        _currentEnemy = thisEnemy.gameObject;
        _visualDistance = 75.0f;
        _stage = EVENT.ENTER;
        _attack = enemyAttack;
    }
}
