using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHomingAttack : BossState
{
    float _haTimeLimit = 10.0f;
    float _haTimer = 0;
    float _spawnTimer = 2f;
    Transform _homingSpawnPoint;

    public BossHomingAttack(GameObject boss) : base(boss)
    {
        _bossStateName = BOSSSTATENAME.HOMINGATTACK;
        Debug.Log("Homing State Initiated");
        _currentEnemy = boss;
        _haTimer = _haTimeLimit;
    }

    public override void Enter()
    {
        base.Enter();
        _currentEnemy.GetComponent<BossAI>().SetDroneAndHomingSpawnAnimation();
        _homingSpawnPoint = _currentEnemy.GetComponent<BossAI>()._spawnPoint;
    }

    public override void Update()
    {
        base.Update();
        _haTimer -= Time.deltaTime;
        _spawnTimer -= Time.deltaTime;

        if(_spawnTimer <= 0)
        {
            _spawnTimer = 10f;
            HomingAttackManager.instance.SpawnHomingAttack(_homingSpawnPoint.transform.position);
            //HomingAttackManager.instance.SpawnHomingAttack(_currentEnemy.transform.position);
        }
        if (_haTimer <= 0)
        {
            _haTimer = _haTimeLimit;
            _stage = EVENT.EXIT;
            _nextState = new UCState(_currentEnemy);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
