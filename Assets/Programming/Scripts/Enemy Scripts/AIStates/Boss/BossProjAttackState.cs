using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjAttack : BossState
{
    GameObject _drone;
    float _droneTimer;
    public BossProjAttack(GameObject boss) : base(boss)
    {
        Debug.Log("Projectile State");
    }

    public override void Enter()
    {
        base.Enter();
        _drone = GameObject.Instantiate(Resources.Load("Prefabs/Enemies/Drone/DroneEnemy") as GameObject, _currentEnemy.transform.position, Quaternion.identity);
        _drone.GetComponent<DroneAI>().SetCurrentDroneState(new DroneBossAttack(_drone));
        _droneTimer = 7.0f;
    }

    public override void Update()
    {
        base.Update();
        //Debug.Log("Currently in Projectile State");
        _droneTimer -= Time.deltaTime;

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
