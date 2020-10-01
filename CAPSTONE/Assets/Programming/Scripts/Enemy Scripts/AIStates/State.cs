using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.SceneManagement;
using UnityEngine;

public class State : MonoBehaviour
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

    protected float _visualDistance = 10.0f;
    protected float _visualAngle = 30.0f;
    protected float _shootDistance = 7.0f;
    public State(GameObject enemy, Transform[] pp, Transform playerposition)
    {
        _currentEnemy = enemy;
        _patrolPoints = pp;
        _playerPos = playerposition;
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

        //TODO: Raycast between Enemy and Player to avoid see-through wall

        if (direction.magnitude < _visualDistance && angle < _visualAngle)
        {
            return true;
        }
        return false;
    }

    public void LookTowards(Transform target, float turnspeed)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target.position - _currentEnemy.transform.position);
        float str;
        str = Mathf.Min(turnspeed * Time.deltaTime, 1);
        _currentEnemy.transform.rotation = Quaternion.Lerp(_currentEnemy.transform.rotation, targetRotation, str);
    }

}
