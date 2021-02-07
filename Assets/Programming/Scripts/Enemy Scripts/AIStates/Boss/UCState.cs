using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UCState : BossState
{
    BossState[] _randomState;
    public UCState(GameObject boss)
    {
        Debug.Log("UC State Started");
        _currentEnemy = boss;
        //_randomState = new BossState[2];
        //_randomState[0] = new BossHomingAttack(boss);
        //_randomState[1] = new BossProjAttack(boss);
    }

    public override void Enter()
    {
        base.Enter();
       // int randStateInt = Random.Range(0, _randomState.Length);
        
    }

    public override void Update()
    {
        base.Update();
        _nextState = new BossHomingAttack(_currentEnemy );
        Debug.Log(_nextState);
        _stage = EVENT.EXIT;
    }

    public override void Exit()
    {
        base.Exit();

    }
}
