using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamerWanderState : RoamerState
{
    public RoamerWanderState(GameObject enemy, Transform[] pp, Transform playerposition, NavMeshAgent nav) : base(enemy, pp, playerposition, nav)
    {
        _stateName = STATENAME.WANDER;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
