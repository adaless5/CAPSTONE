using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamerLungeState : RoamerState
{
    bool bCanLunge = true;

    public RoamerLungeState(GameObject enemy, Transform[] pp, Transform playerposition, NavMeshAgent nav) : base(enemy, pp, playerposition, nav)
    {
        _stateName = STATENAME.LUNGE;
    }
    public override void Enter()
    {
        base.Enter();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if(bCanLunge)
        {
            Vector3 jumpDir = _currentEnemy.gameObject.transform.forward * 5.0f + _currentEnemy.gameObject.transform.up * 5.0f;
            _currentEnemy.gameObject.GetComponent<Rigidbody>().AddForce(jumpDir, ForceMode.Impulse);

            bCanLunge = false;
        }
        else if(!bCanLunge)
        {
            _nextState = new RoamerPursueState(_currentEnemy, _patrolPoints, _playerPos, _navMeshAgent);
            _stage = EVENT.EXIT;
        }
    }
}
