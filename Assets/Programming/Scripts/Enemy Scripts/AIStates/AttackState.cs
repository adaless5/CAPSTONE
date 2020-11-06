using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Attack : State
{
    float _shootTimer = 0.5f;
    public float _enemyDamage = 20.0f;
    public Attack(GameObject enemy, Transform[] pp, Transform playerposition, NavMeshAgent nav) : base(enemy, pp, playerposition, nav)
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
            _currentEnemy.transform.LookAt(_playerPos.position);
            if (Vector3.Distance(_currentEnemy.transform.position, _playerPos.position) < _shootDistance)
            {
                ShootPlayer();
            }
            else
            {
                _currentEnemy.transform.position = Vector3.MoveTowards(_currentEnemy.transform.position, _playerPos.position, _enemySpeed * Time.deltaTime);
            }
        }
        else
        {
            _nextState = new Patrol(_currentEnemy, _patrolPoints, _playerPos, _navMeshAgent);
            _stage = EVENT.EXIT;
        }
    }

    public void ShootPlayer()
    {
        _shootTimer -= Time.deltaTime;
        Vector3 bulletDeviation = Random.insideUnitCircle * _maxDeviation;
        Quaternion rot = Quaternion.LookRotation(Vector3.forward * _bulletRange + bulletDeviation);
        Vector3 finalFowardVector = _currentEnemy.transform.rotation * rot * Vector3.forward;
        finalFowardVector += _currentEnemy.transform.position;
        RaycastHit hit;

        Debug.DrawRay(finalFowardVector, _currentEnemy.transform.forward * _shootDistance, Color.green);
        if (_shootTimer <= 0)
        {
            if (Physics.Raycast(finalFowardVector, _currentEnemy.transform.forward * _shootDistance, out hit, _shootDistance))
            {
                if (hit.transform.tag == "Player")
                {
                    Debug.Log("Player hit");
                    _playerPos.gameObject.GetComponent<ALTPlayerController>().CallOnTakeDamage(_enemyDamage);
                }
                else
                {
                    Debug.Log("Player not hit");
                }
            }
            _shootTimer = 0.5f;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
