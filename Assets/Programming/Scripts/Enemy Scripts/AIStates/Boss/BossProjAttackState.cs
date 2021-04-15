using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjAttack : BossState
{
    GameObject _drone;
    Transform _droneSpawnPoint;
    float _droneTimer;
    float _spawnTimer = 2f;

    public BossProjAttack(GameObject boss) : base(boss)
    {
        _bossStateName = BOSSSTATENAME.DRONESPAWNING;
        Debug.Log("Spawning Drones..");
    }

    public override void Enter()
    {
        base.Enter();
        _currentEnemy.GetComponent<BossAI>().SetDroneAndHomingSpawnAnimation();
        _droneSpawnPoint = _currentEnemy.GetComponent<BossAI>()._spawnPoint;
        _droneTimer = 10.0f;
        _spawnTimer = 2.0f;
    }
    

    public override void Update()
    {
        base.Update();
        //Debug.Log("Currently in Projectile State");
        _droneTimer -= Time.deltaTime;
        _spawnTimer -= Time.deltaTime;

        if(_spawnTimer <= 0)
        {
            _spawnTimer = 10f;
            //_drone = GameObject.Instantiate(Resources.Load("Prefabs/Enemies/Drone/DroneEnemy") as GameObject, _currentEnemy.transform.position, Quaternion.identity);
            _drone = GameObject.Instantiate(Resources.Load("Prefabs/Enemies/Drone/DroneEnemy") as GameObject, _droneSpawnPoint.transform.position, Quaternion.identity);
            _drone.GetComponent<DroneAI>().SetCurrentDroneState(new DroneBossAttack(_drone));            
        }

        if (_droneTimer <= 0)
        {
            _nextState = new UCState(_currentEnemy);
            _stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        _nextState = new UCState(_currentEnemy);
        base.Exit();

    }
}
