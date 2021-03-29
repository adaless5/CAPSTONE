using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamerDeath : RoamerState
{

    float _deathTimer = 0;
    public RoamerDeath(GameObject enemy, Transform[] pp, Transform playerposition, NavMeshAgent nav) : base(enemy, pp, playerposition, nav)
    {
        _stateName = STATENAME.DEAD;
        _deathTimer = 8.5f;
    }

    public override void Enter()
    {
        base.Enter();        
        _navMeshAgent.isStopped = true;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        _deathTimer -= Time.deltaTime;
        if(_deathTimer <= 0)
        {
            _navMeshAgent.isStopped = false;
            _currentEnemy.SetActive(false);
            for (int i = 0; i < _currentEnemy.transform.childCount; ++i)
            {
                _currentEnemy.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

  
}
