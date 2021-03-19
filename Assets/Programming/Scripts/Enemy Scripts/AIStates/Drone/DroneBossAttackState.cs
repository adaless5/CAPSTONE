using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBossAttack : DroneState
{
    GameObject _droneProjectile;
    public DroneBossAttack(GameObject enemy, Transform[] pp, Transform playerposition, UnityEngine.AI.NavMeshAgent nav) : base(enemy, pp, playerposition, nav)
    {

    }

    public DroneBossAttack(GameObject enemy) : base(enemy, null, ALTPlayerController.instance.transform, null)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _droneProjectile = (GameObject)Resources.Load("Prefabs/Weapon/Drone Projectile");
    }

    public override void Update()
    {
        base.Update();

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

    public void ShootPlayer()
    {
        _shootTimer -= Time.deltaTime;
        Vector3 playerDir = _playerPos.position - _currentEnemy.transform.position;

        if (_shootTimer <= 0)
        {
            //if (Physics.Raycast(finalFowardVector, playerDir * _shootDistance, out hit, _shootDistance))
            //{
            //    if (hit.transform.tag == "Player")
            //    {
            //        Debug.Log("Player hit");
            //        _playerPos.gameObject.GetComponent<ALTPlayerController>().CallOnTakeDamage(_enemyDamage);
            //    }
            //    else
            //    {
            //        Debug.Log("Player not hit");
            //    }
            //}

            GameObject tempbullet = GameObject.Instantiate(_droneProjectile, playerDir, Quaternion.identity, _currentEnemy.transform);
            tempbullet.GetComponent<Rigidbody>().AddForce(playerDir * _shootDistance, ForceMode.Impulse);
            _shootTimer = 0.5f;
        }
    }


    public override void Exit()
    {
        base.Exit();
    }
}
