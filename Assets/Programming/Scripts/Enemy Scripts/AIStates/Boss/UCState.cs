using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UCState : BossState
{
    BossState[] _randomState;
    int _bossStateNum;
    public UCState(GameObject boss) : base (boss)
    {
        _randomState = new BossState[2];
        Debug.Log("UC State Started");
    }

    public override void Enter()
    {
        base.Enter();
        _randomState[0] = new BossHomingAttack(_currentEnemy);
        _randomState[1] = new BossProjAttack(_currentEnemy);
        _bossStateNum = Random.Range(0, _randomState.Length);
        Debug.Log("Random State chosen is " + _bossStateNum);
    }

    public override void Update()
    {
        base.Update();
        _nextState = _randomState[_bossStateNum];
        _stage = EVENT.EXIT;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
