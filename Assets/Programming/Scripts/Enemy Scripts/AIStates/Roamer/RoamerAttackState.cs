using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamerAttackState : RoamerState
{
    float _Damage = 5.0f;
    float _AttackTimer = 0.0f;
    bool bCanAttack = true;

    public RoamerAttackState(GameObject enemy, Transform[] pp, Transform playerposition, NavMeshAgent nav) : base(enemy, pp, playerposition, nav)
    {
        _stateName = STATENAME.ATTACK;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemy Attacking");
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (Vector3.Distance(_currentEnemy.transform.position, _playerPos.transform.position) > 2.5f)
        {
            _navMeshAgent.ResetPath();
            _nextState = new RoamerPursueState(_currentEnemy, _patrolPoints, _playerPos, _navMeshAgent);
            _stage = EVENT.EXIT;
        }

        Attack();
       
    }

    void Attack()
    {
        if (bCanAttack)
        {
            if (_playerPos != null)
            {
                _playerPos.GetComponent<ALTPlayerController>().CallOnTakeDamage(_Damage);
            }

            bCanAttack = false;
            _AttackTimer = 0.0f;
        }
        else
        {
            LookAt(_playerPos.transform);
            _AttackTimer += Time.deltaTime;

            if (_AttackTimer >= 0.935f)
            {
                bCanAttack = true;
            }
        }
    }
}
