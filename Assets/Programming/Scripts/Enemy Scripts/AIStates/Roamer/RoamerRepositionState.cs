using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamerRepositionState : RoamerState
{
    public RoamerRepositionState(GameObject enemy, Transform[] pp, Transform playerposition, NavMeshAgent nav) : base(enemy, pp, playerposition, nav)
    {
        _stateName = STATENAME.REPOSITION;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
