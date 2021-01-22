using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjAttack : BossState
{
    public BossProjAttack(GameObject boss)
    {
        Debug.Log("Projectile State");
        _currentEnemy = boss;
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Update()
    {
        base.Update();
        _stage = EVENT.EXIT;
    }

    public override void Exit()
    {
        _nextState = new UCState(_currentEnemy);
        base.Exit();

    }
}
