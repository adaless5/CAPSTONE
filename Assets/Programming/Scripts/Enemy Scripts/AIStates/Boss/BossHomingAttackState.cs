using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHomingAttack : BossState
{
    float _haTimeLimit = 4.0f;
    float _haTimer = 0;
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
        HomingAttackManager.instance.SpawnHomingAttack(_currentEnemy.transform.position);
    }

    public override void Update()
    {
        base.Update();
        _haTimer -= Time.deltaTime;
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
