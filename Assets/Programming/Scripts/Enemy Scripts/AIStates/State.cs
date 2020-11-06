using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        IDLE, PATROL, FOLLOW, ATTACK
    };

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    public STATE _stateName;
    protected State _nextState;
    protected EVENT _stage;
    protected float _enemySpeed = 2f;
    protected Transform[] _patrolPoints;
    protected GameObject _currentEnemy;
    protected Transform _playerPos;
    protected NavMeshAgent _navMeshAgent;

    protected float _visualDistance = 10.0f;
    protected float _visualAngle = 180.0f;
    protected float _shootDistance = 7.0f;
    protected float _bulletRange = 300.0f;
    protected float _maxDeviation = 350.0f;
    public State(GameObject enemy, Transform[] pp, Transform playerposition, NavMeshAgent nav)
    {
        _currentEnemy = enemy;
        _patrolPoints = pp;
        _playerPos = playerposition;
        _navMeshAgent = nav;
        _stage = EVENT.ENTER;
    }

    public virtual void Enter() { _stage = EVENT.UPDATE; }
    public virtual void Update() { _stage = EVENT.UPDATE; }
    public virtual void Exit() { _stage = EVENT.EXIT; }

    public State Process()
    {
        switch (_stage)
        {
            case EVENT.ENTER:
                Enter();
                break;
            case EVENT.UPDATE:
                Update();
                break;
            case EVENT.EXIT:
                Exit();
                return _nextState;
        }
        return this;
    }
    public bool CanSeePlayer()
    {
        Vector3 direction = _playerPos.position - _currentEnemy.transform.position;
        float angle = Vector3.Angle(direction, _currentEnemy.transform.forward);

        NavMeshHit hit;
        if (!_navMeshAgent.Raycast(_playerPos.position, out hit))
        {
            //Debug.Log("Player not hiding");
            if (direction.magnitude < _visualDistance && angle < _visualAngle)
            {
                //Debug.Log("Player found");
                return true;
            }

        }
        //Debug.Log("Player hiding");
        return false;
    }

}
