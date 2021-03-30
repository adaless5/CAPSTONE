using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stun : State
{
    float _stunTime = 3.0f;
    //Vector3 _spinningVector = new Vector3(0, 180.0f, 0);
    State _resumeState;
    public Stun(float stuntime, State resumestate) 
    {
        _currentEnemy = resumestate.GetCurrentEnemy();
        _stateName = STATENAME.STUN;
        _stunTime = stuntime;
        _resumeState = resumestate;
        Debug.Log("Stunned");
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        _stunTime -= Time.deltaTime;
        //_currentEnemy.transform.Rotate(_spinningVector * Time.deltaTime);
        if (_stunTime <= 0)
        {
            _stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        _currentEnemy.GetComponent<NavMeshAgent>().isStopped = false;
        _nextState = _resumeState;
        base.Exit();
    }
}
