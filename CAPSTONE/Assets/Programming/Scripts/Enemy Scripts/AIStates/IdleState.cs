using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    public Idle(GameObject enemy, Transform[] pp, Transform playerposition) : base(enemy, pp, playerposition)
    {
        _stateName = STATE.IDLE;

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
