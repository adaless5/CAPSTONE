using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportingEnemyIdleState : TeleportingEnemyState
{
    // Start is called before the first frame update
    public TeleportingEnemyIdleState(TeleportingEnemyAI thisEnemy, Transform playerPosition, TeleportingEnemyAnimation enemyAnimation, TeleportingEnemyAttack enemyAttack) : base(thisEnemy, playerPosition, enemyAnimation, enemyAttack)
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
        if (!_thisEnemy._hasDisappeared)
            CheckStates();

    }
    void CheckStates()
    {
        if (Vector3.Distance(_playerPosition.position, _currentEnemy.transform.position) < _thisEnemy._followDistance)
        {
            RaycastHit hitt;
            LayerMask enemyLayer = new LayerMask();
            enemyLayer.value = 14;
            ALTPlayerController player = GameObject.FindObjectOfType<ALTPlayerController>();
            _thisEnemy.LookTowards(_thisEnemy.transform,player.transform.position,_thisEnemy._lookSpeed);
            if (Physics.Raycast(_currentEnemy.transform.position, _currentEnemy.transform.forward, out hitt,100.0f,~enemyLayer))
            {
                if (hitt.collider.gameObject.GetComponent<ALTPlayerController>())
                {
                    _nextState = new TeleportingEnemyFollowState(_thisEnemy, _playerPosition, _enemyAnimation, _attack);
                    _stage = EVENT.EXIT;
                }
                    Debug.DrawRay(_currentEnemy.transform.position, _currentEnemy.transform.position + _currentEnemy.transform.forward * 10, new Color(1,1,1),1);
            }
        }

    }
    public override void Exit()
    {
        base.Exit();
    }
}