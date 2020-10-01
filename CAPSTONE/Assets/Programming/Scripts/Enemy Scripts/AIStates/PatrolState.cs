using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : State
{
    int _currentPatrolIndex = 0;
    public Patrol(GameObject enemy, Transform[] pp, Transform playerposition) : base(enemy, pp, playerposition)
    {
        _stateName = STATE.PATROL;

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (_currentPatrolIndex != _patrolPoints.Length)
        {
            if (Vector3.Distance(_currentEnemy.transform.position, _patrolPoints[_currentPatrolIndex].transform.position) > 2)
            {

                //  _currentEnemy.transform.rotation = Quaternion.RotateTowards(_currentEnemy.transform.rotation, _patrolPoints[_currentPatrolIndex].transform.rotation, 2f);
                LookTowards(_patrolPoints[_currentPatrolIndex].transform, 4.0f);
                _currentEnemy.transform.position = Vector3.MoveTowards(_currentEnemy.transform.position, _patrolPoints[_currentPatrolIndex].transform.position, _enemySpeed * Time.deltaTime);
            }
            else
            {
                Debug.Log("Patrol point reached");
                _currentPatrolIndex++;
            }
        }
        else
        {
            _currentPatrolIndex = 0;
        }

        if (CanSeePlayer())
        {
            _nextState = new Attack(_currentEnemy, _patrolPoints, _playerPos);
            _stage = EVENT.EXIT;
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
