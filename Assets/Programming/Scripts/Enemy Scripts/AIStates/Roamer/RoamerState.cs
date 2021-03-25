using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamerState : State
{
    protected float _roamerSpeed = 5.0f;
    protected Transform[] _patrolPoints;
    protected NavMeshAgent _navMeshAgent;

    public RoamerState(GameObject enemy, Transform[] pp, Transform playerposition, NavMeshAgent nav)
    {
        _currentEnemy = enemy;
        _patrolPoints = pp;
        _playerPos = playerposition;
        _navMeshAgent = nav;
        _stage = EVENT.ENTER; 
    }

}
