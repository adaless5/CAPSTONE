using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class DroneAttack : DroneState
{
    GameObject _droneProjectile;

    public DroneAttack(GameObject enemy, Transform[] pp, Transform playerposition, NavMeshAgent nav) : base(enemy, pp, playerposition, nav)
    {
        _stateName = STATENAME.ATTACK;
        _droneProjectile = (GameObject)Resources.Load("Prefabs/Weapon/Drone Projectile");
    }
    
    public DroneAttack(GameObject enemy) : base (enemy, null, ALTPlayerController.instance.transform, null)
    {

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

            LookAt(_playerPos);

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
            _nextState = new DronePatrol(_currentEnemy, _patrolPoints, _playerPos, _navMeshAgent);
            _stage = EVENT.EXIT;
        }
    }

    public void ShootPlayer()
    {
        _shootTimer -= Time.deltaTime;
        Vector3 bulletDeviation = Random.insideUnitCircle * _maxDeviation;
        Quaternion rot = Quaternion.LookRotation(Vector3.forward * _bulletRange + bulletDeviation);
        Vector3 finalFowardVector = _currentEnemy.transform.rotation * Vector3.forward;
        //Vector3 finalFowardVector = _currentEnemy.transform.rotation * Vector3.forward;
        finalFowardVector += _currentEnemy.transform.position;
        RaycastHit hit;
        Vector3 playerDir = _playerPos.position - _currentEnemy.transform.position;

        Debug.DrawRay(_currentEnemy.transform.forward, playerDir * _shootDistance, Color.green);
        if (_shootTimer <= 0)
        {
            if (Physics.Raycast(finalFowardVector, playerDir * _shootDistance, out hit, _shootDistance))
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
            //Debug.Log("Spawn Bullet");
            //GameObject tempbullet = GameObject.Instantiate(_droneProjectile, finalFowardVector, Quaternion.identity, _currentEnemy.transform);
            //tempbullet.GetComponent<Rigidbody>().AddForce(playerDir * _shootDistance, ForceMode.Impulse);
            _shootTimer = 0.5f;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
