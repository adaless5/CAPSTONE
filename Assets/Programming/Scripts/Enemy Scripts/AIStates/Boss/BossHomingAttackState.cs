using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHomingAttack : BossState
{
    GameObject _haRef;
    GameObject _homingAttack;
    public BossHomingAttack(GameObject boss) : base(boss)
    {
        Debug.Log("Homing State Initiated");
        _currentEnemy = boss;
        _haRef = (GameObject)Resources.Load("Prefabs/Enemies/Boss/HA");
        _homingAttack = new GameObject();
    }

    public override void Enter()
    {
        base.Enter();
        _homingAttack = GameObject.Instantiate(_haRef, _currentEnemy.transform.position + Vector3.up, Quaternion.identity);
    }

    public override void Update()
    {
        base.Update();

        if (_homingAttack == null)
        {
            _stage = EVENT.EXIT;
            _nextState = new UCState(_currentEnemy);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
