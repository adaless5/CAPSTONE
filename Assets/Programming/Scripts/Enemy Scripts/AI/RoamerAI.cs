using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamerAI : MonoBehaviour
{
    private State _currentState;
    public Transform[] _patrolPoints;
    private NavMeshAgent _navMeshAgent;
    GameObject _playerReference;
    public Animator _roamerAnimator;
    public Health _roamerHealth;
    public bool bShouldWanderRandomly = false;
   

    private void Awake()
    {
        EventBroker.OnPlayerSpawned += EventStart;
       
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _roamerAnimator = GetComponentInChildren<Animator>();
        _roamerHealth = GetComponent<Health>();
        _roamerHealth.OnTakeDamage += TakingDamage;
        _roamerHealth.OnDeath += Death;
        
    }

    void Start()
    {
     
    }

    private void EventStart(GameObject player)
    {
        if (this != null)
        {
            if (bShouldWanderRandomly)
                _currentState = new RoamerWanderState(gameObject, _patrolPoints, player.transform, _navMeshAgent);

            else if (!bShouldWanderRandomly)
                _currentState = new RoamerIdleState(gameObject, _patrolPoints, player.transform, _navMeshAgent); 
                //_currentState = new RoamerPatrolState(gameObject, _patrolPoints, player.transform, _navMeshAgent);

            _playerReference = player;
        }
    }

    void Update()
    {
        if (_currentState != null)
            _currentState = _currentState.Process();
        CheckAnimationState();
        //_roamerAnimator.SetFloat("IsWalking", _navMeshAgent.speed);

        

    }

    public void Stun()
    {
       // if(gameObject.)
        {
            _navMeshAgent.isStopped = true;
        }
        State savedState = _currentState;        
        _currentState = new Stun(0.7f, savedState);
    }

    public void TakingDamage()
    {
        //If AI is attacked in idle, switch to chasing state even when not in line of sight
        if(_currentState._stateName == State.STATENAME.IDLE)
        {
            _currentState = new RoamerPursueState(gameObject, _patrolPoints, _playerReference.transform, _navMeshAgent);
        }
        _roamerAnimator.SetTrigger("IsHit");
        Stun();

    }

    //private void OnAnimatorMove()
    //{
    //    _navMeshAgent.speed = (_roamerAnimator.deltaPosition / Time.deltaTime).magnitude;
    //}

    public void Death()
    {
        Debug.Log("HE DEAD");
        _roamerAnimator.SetTrigger("IsDead");
    }

    public void CheckAnimationState()
    {
        if (_currentState._stateName == State.STATENAME.IDLE)
        {
            _roamerAnimator.SetTrigger("IsIdle");
        }
        else if (_currentState._stateName == State.STATENAME.PATROL)
        {

            _roamerAnimator.SetTrigger("IsPatrolling");
        }
        else if (_currentState._stateName == State.STATENAME.FOLLOW)
        {
            _roamerAnimator.SetTrigger("IsChasing");
        }
        else if (_currentState._stateName == State.STATENAME.ATTACK)
        {
            _roamerAnimator.SetTrigger("IsAttacking");
        }
    }
}
