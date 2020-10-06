using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    float _shootTimer = 1f;
    public Attack(GameObject enemy, Transform[] pp, Transform playerposition) : base(enemy, pp, playerposition)
    {
        _stateName = STATE.ATTACK;

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (CanSeePlayer())
        {
            if (Vector3.Distance(_currentEnemy.transform.position, _playerPos.position) < _shootDistance)
            {
                Debug.Log("Attack Code");
                ShootPlayer();
            }
            else
            {
                _currentEnemy.transform.LookAt(_playerPos.position);
                _currentEnemy.transform.position = Vector3.MoveTowards(_currentEnemy.transform.position, _playerPos.position, _enemySpeed * Time.deltaTime);
            }
        }
        else
        {
            _nextState = new Patrol(_currentEnemy, _patrolPoints, _playerPos);
            _stage = EVENT.EXIT;
        }
    }

    public void ShootPlayer()
    {
        _shootTimer -= Time.deltaTime;

        if (_shootTimer <= 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(_currentEnemy.transform.position, _currentEnemy.transform.forward, out hit, _shootDistance))
            {
                Debug.Log("Player hit");
            }
            _shootTimer = 1f;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
