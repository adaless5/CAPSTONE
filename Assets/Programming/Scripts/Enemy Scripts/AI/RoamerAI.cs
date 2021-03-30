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
    public GameObject _wanderSphere;
    public float _wanderRadius;
    private void Awake()
    {
        EventBroker.OnPlayerSpawned += EventStart;
       
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _roamerAnimator = GetComponentInChildren<Animator>();
        _roamerHealth = GetComponent<Health>();
        _roamerHealth.OnTakeDamage += TakingDamage;
        //_roamerHealth.OnDeath += Death;
        
    }

    void Start()
    {
     
    }

    private void EventStart(GameObject player)
    {
        if (this != null)
        {
            if (bShouldWanderRandomly)
                _currentState = new RoamerWanderState(gameObject, _patrolPoints, player.transform, _navMeshAgent, _wanderRadius);

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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _wanderRadius);
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
        if(_roamerHealth.isDead)
        {
            _currentState.Exit();
            _currentState = new RoamerDeath(gameObject, _patrolPoints, _playerReference.transform, _navMeshAgent);
            int deathRandomize = Random.Range(0, 2);
            _roamerAnimator.SetInteger("RandomDeath", deathRandomize);
            _roamerAnimator.SetTrigger("IsDying");
        }
    }

  
   

    public void CheckAnimationState()
    {
        switch (_currentState._stateName)
        {
            case State.STATENAME.IDLE:
                _roamerAnimator.SetTrigger("IsIdle");
                break;
            case State.STATENAME.PATROL:
                _roamerAnimator.SetTrigger("IsPatrolling");
                break;
            case State.STATENAME.FOLLOW:
                _roamerAnimator.SetTrigger("IsChasing");
                break;
            case State.STATENAME.ATTACK:
                _roamerAnimator.SetTrigger("IsAttacking");
                break;
            case State.STATENAME.WANDER:
                _roamerAnimator.SetTrigger("IsPatrolling");
                break;
        }     
    }    
}
