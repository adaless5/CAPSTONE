using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamerWanderState : RoamerState
{       
    Vector3 _WanderAreaScale;             
    Vector3 _WanderAreaPosition;         
    public float _wanderSpeed = 1.4f;
    float _wanderRadius;
    Vector3 finalPos;
    int _ChangeToIdleChance;

    public RoamerWanderState(GameObject enemy, Transform[] pp, Transform playerposition, NavMeshAgent nav, float wanderrad) : base(enemy, pp, playerposition, nav)
    {
        _stateName = STATENAME.WANDER;
        _wanderRadius = wanderrad;
        Debug.Log("Enter Wander State");
    }

    public override void Enter()
    {
        base.Enter();
        _navMeshAgent.isStopped = false;
        SetRandomWanderPatrolPoint();
    }

    public override void Update()
    {
        if (_navMeshAgent.remainingDistance < 0.5f)
        {  
            if(_ChangeToIdleChance == 0)
            {
                _navMeshAgent.isStopped = true;
                _nextState = new RoamerIdleState(_currentEnemy, _patrolPoints, _playerPos, _navMeshAgent);
                _stage = EVENT.EXIT;
            }
            SetRandomWanderPatrolPoint();
        }
        else
        {
            LookTowards(finalPos, _wanderSpeed);
        }

        if (Vector3.Distance(_currentEnemy.transform.position, _playerPos.transform.position) < 5.0f || CanSeePlayer())
        {
            _navMeshAgent.ResetPath();
            _nextState = new RoamerPursueState(_currentEnemy, _patrolPoints, _playerPos, _navMeshAgent);
            _stage = EVENT.EXIT;
        }
    }

    void SetRandomWanderPatrolPoint()
    {
        Vector3 randDir = Random.insideUnitSphere * _wanderRadius;
        randDir += _currentEnemy.transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randDir, out hit, _wanderRadius, 1);
        finalPos = hit.position;       
        Debug.Log("Point set to" + finalPos);
        _navMeshAgent.SetDestination(finalPos);
        _ChangeToIdleChance = Random.Range(0, 3);

    }
    public void LookTowards(Vector3 target, float turnspeed)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - _currentEnemy.transform.position);
        float str;
        str = Mathf.Min(turnspeed * Time.deltaTime, 1);
        _currentEnemy.transform.rotation = Quaternion.Lerp(_currentEnemy.transform.rotation, targetRotation, str);
    }
}
