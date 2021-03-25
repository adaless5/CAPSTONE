using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamerPursueState : RoamerState
{
    GameObject _player;
    bool bCanLunge = true;
    int _LungeChance = 0;
    bool bIsFacingPlayer = false;

    public RoamerPursueState(GameObject enemy, Transform[] pp, Transform playerposition, NavMeshAgent nav) : base(enemy, pp, playerposition, nav)
    {
        _stateName = STATENAME.FOLLOW;
    }

    public override void Enter()
    {
        base.Enter();

        _LungeChance = Random.Range(0, 4);
        bCanLunge = true;
    }

    public override void Update()
    {
        base.Update();

        MoveToPlayer();

        Debug.DrawRay(_currentEnemy.transform.position + new Vector3(0.0f, 1.0f, 0.0f), _currentEnemy.transform.forward);
        RaycastHit hit;
        Physics.Raycast(_currentEnemy.transform.position + new Vector3(0.0f, 1.0f, 0.0f), _currentEnemy.transform.forward, out hit, Mathf.Infinity);
        if(hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                bIsFacingPlayer = true;
            }
            else
            {
                bIsFacingPlayer = false;
            }
        }

        if (Vector3.Distance(_currentEnemy.gameObject.transform.position, _playerPos.position) <= 2.0f)
        {
            _navMeshAgent.ResetPath();
            _nextState = new RoamerAttackState(_currentEnemy, _patrolPoints, _playerPos, _navMeshAgent);
            _stage = EVENT.EXIT;
        }
        else if (_navMeshAgent.remainingDistance < 10.0f && bCanLunge && _LungeChance == 3 && bIsFacingPlayer)
        {
            //TODO: Figure out why enemy won't leave the ground despite adding an up vector with forward vector when applying impulse -AD
            _nextState = new RoamerLungeState(_currentEnemy, _patrolPoints, _playerPos, _navMeshAgent);
            _stage = EVENT.EXIT;

        }
        else if (_navMeshAgent.remainingDistance >= 15.0f)
        {
            _navMeshAgent.ResetPath();
            _nextState = new RoamerIdleState(_currentEnemy, _patrolPoints, _playerPos, _navMeshAgent);
            _stage = EVENT.EXIT;
        }

        Debug.Log("Enemy Pursuing");
    }

    void MoveToPlayer()
    {
        _navMeshAgent.destination = _playerPos.transform.position;
    }
}
