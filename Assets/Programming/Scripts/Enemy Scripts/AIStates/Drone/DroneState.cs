using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
//using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class DroneState : State
{


    protected float _enemySpeed = 2f;
    protected Transform[] _patrolPoints;
    protected NavMeshAgent _navMeshAgent;


    private Quaternion _desiredRot;

    public DroneState(GameObject enemy, Transform[] pp, Transform playerposition, NavMeshAgent nav)
    {
        _currentEnemy = enemy;
        _patrolPoints = pp;
        _playerPos = playerposition;
        _navMeshAgent = nav;
        _stage = EVENT.ENTER;
    }
}
