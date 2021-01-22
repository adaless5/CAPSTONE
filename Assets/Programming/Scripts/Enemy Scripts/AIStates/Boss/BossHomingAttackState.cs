using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHomingAttack : BossState
{
    GameObject _haRef;
    GameObject _homingAttack;
    public BossHomingAttack(GameObject boss)
    {
        Debug.Log("Homing State");
        _currentEnemy = boss;
        _haRef = (GameObject)Resources.Load("Prefabs/Enemies/Boss/Homing Attack");
    }

    public override void Enter()
    {
        base.Enter();
        _homingAttack = GameObject.Instantiate(_haRef, _currentEnemy.transform);
    }

    public override void Update()
    {
        base.Update();

        if (_homingAttack == null)
        {
            Debug.Log("Next state...");
            _stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
        _nextState = new UCState(_currentEnemy);
        Debug.Log("Homing State Complete");
    }
}
