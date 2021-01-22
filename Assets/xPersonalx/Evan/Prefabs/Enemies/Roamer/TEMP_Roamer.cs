using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_Roamer : MonoBehaviour
{
    public enum RoamerState
    {
        Patrol, Wander, Follow, Attack
    }

    MeshRenderer _MeshRenderComponent;
    Vector3      _StartPosition;
    GameObject   _Player;
    public bool         _MakeThisRoamerWanderRandomly;   // When enabled the roamer will wander randomly withing the area of the _WanderAreaSphereObject. When disabled the roamer will move towards its patrol points.


           bool         _IsIdling;                      // Bool to make the roamer enter Idle state.
           float        _IdleForTime;                   // Counts down the time that the roamer will stay idle for.
           float        _UntilNextIdleTime;             // Count down until the next time a roamer will go idle.

    public GameObject   _WanderAreaSphereObject;        // This is used in level design to refine the area in which a wandering roamer (not patroling) will wander.
           Vector3      _WanderAreaScale;               // Used internally to determine the size of the area of _WanderAreaSphereObject. Should always be the result of WanderAreaSphereObject.transform.position but has its own variable to avoid startup bugs. Just roll with it.
           Vector3      _WanderAreaPosition;            // Used internally to determine the location of area of _WanderAreaSphereObject. Should always be the result of WanderAreaSphereObject.transform.position but has its own variable to avoid startup bugs. Just roll with it.
    public Vector3      _WanderPoint;                   // The target position a wandering roamer is currently moving towards.

    public float        _LookSpeed;
    public float        _PatrolOrWanderSpeed;           // The speed at which the roamer will move during its patrol or wander state.
    public float        _AcceptableDistanceFromTarget;  // The distance at which the roamer will accept that it has reached its current wander target or patrol point.

    public Transform[]  _PatrolPoints;                  // Patrol points that a Patroling roamer (not wandering) will move to and cycle through. 
    public int          _CurrentPatrolIndex;        // The index of the patrol point that the roamer is currently approaching.

    public float        _FollowRange;                   // The distance from the player at which the roamer becomes altered.
    public float        _FollowEscapeRange;             // The extra distance on top of the follow range that the player must escape in order to escape the roamer's awareness.
    public float        _FollowSpeed;                   // The speed at which a roamer which is following the player moves.

           bool         _IsAttacking;                   // Determines whether or not the player is currently attacking
    public float        _AttackRange;                   // The distance from the player at which the roamer will begin to attack.
    public float        _AttackEscapeRange;             // The extra distance on top of the follow range that the player must escape in order to escape the roamer's attack range.
    public float        _AttackTime;                    // Counts down the next attack.
    public float        _HitRange;
    public float        _HitDamage;

           RoamerState  _CurrentState;                  // The current state of the roamer.

    // Start is called before the first frame update
    void Start()
    {
        _StartPosition = transform.position;
        _WanderPoint = _StartPosition;
        _FollowSpeed = 0.5f;
        _MeshRenderComponent = GetComponent<MeshRenderer>();

        _AttackTime = 0.5f;
        _IsAttacking = false;

        _CurrentPatrolIndex = 0;

        if (_MakeThisRoamerWanderRandomly)
        {
            _CurrentState = RoamerState.Wander;
        }
        else
        {

            _CurrentState = RoamerState.Patrol;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {   
        if (_WanderAreaSphereObject != null)
        {      
            if (_MakeThisRoamerWanderRandomly)
            {
           
                _WanderAreaScale = _WanderAreaSphereObject.transform.localScale;
                _WanderAreaPosition = _WanderAreaSphereObject.transform.position;
                _WanderPoint = new Vector3(_WanderPoint.x, transform.position.y, _WanderPoint.z);
            }
        }
        if (_Player==null)
        {
            _Player = GameObject.FindWithTag("Player");
        }
        else
        {

            StateChange();
            MakeIdle();
        }

        
            transform.eulerAngles = new Vector3(0.0f, transform.localEulerAngles.y, 0.0f);
        
    }

    void Idle()     
    { 
    }
    void Patrol()   
    {
        if (_CurrentPatrolIndex != _PatrolPoints.Length)
        {
            if (Vector3.Distance(transform.position, _PatrolPoints[_CurrentPatrolIndex].transform.position) > _AcceptableDistanceFromTarget)
            {

                LookTowards(_PatrolPoints[_CurrentPatrolIndex].transform.position, _LookSpeed);
                transform.position = Vector3.MoveTowards(transform.position, _PatrolPoints[_CurrentPatrolIndex].transform.position, _PatrolOrWanderSpeed * Time.deltaTime);
            }
            else
            {
                _CurrentPatrolIndex++;
            }
        }
        else
        {
            _CurrentPatrolIndex = 0;
        }
    }
    void Wander()   
    {
        if (Vector3.Distance(transform.position, _WanderPoint) > _AcceptableDistanceFromTarget)
        {
           
            LookTowards(_WanderPoint, _LookSpeed);
            transform.position = Vector3.MoveTowards(transform.position, _WanderPoint, _PatrolOrWanderSpeed * Time.deltaTime);
        }
        else
        {
            SetRandomWanderPatrolPoint();
        }
    }
    void Attack()
    {
        if (_IsAttacking == true)
        {
            _IsAttacking = false;
            _AttackTime = 0.5f;

            if (_Player != null)
            {
               _Player.transform.GetComponent<ALTPlayerController>().CallOnTakeDamage(_HitDamage);
            }
             
            
            }
            else
            {
                LookTowards(_Player.transform.position, 6);
                _AttackTime -= Time.fixedDeltaTime;

                if (_AttackTime < 0.0f)
                {

                    _IsAttacking = true;
                }
            }

    }
    void Follow()
    {
        Vector3 direction = Vector3.Normalize(_Player.transform.position - transform.position);
        transform.position += (transform.forward
                            * Time.fixedDeltaTime)
                            * (Vector3.Distance(transform.position, _Player.transform.position) * _FollowSpeed);
        LookTowards(_Player.transform.position, _LookSpeed);
    }
    void StateChange()
    {

        if (_IsIdling)
        {
            Idle();

        }
        else if (_CurrentState == RoamerState.Wander)
        {
            Wander();
            if (FromPatrolToFollow())
            {
                _CurrentState = RoamerState.Follow;
            }
        }
        else if (_CurrentState == RoamerState.Patrol)
        {
            Patrol();
            if (FromPatrolToFollow())
            {
                _CurrentState = RoamerState.Follow;
            }
        }
        else  if (_CurrentState == RoamerState.Follow)
        {
            Follow();
            if (FromFollowToAttack())
            {
                _CurrentState = RoamerState.Attack;
            }
            if (FromFollowToPatrol())
            {
                if(_MakeThisRoamerWanderRandomly)
                {
                    _CurrentState = RoamerState.Wander;
                }
                else 
                {
                    _CurrentState = RoamerState.Patrol; 
                }
                _UntilNextIdleTime = 0.0f;
                _IdleForTime = Random.Range(3.0f, 7.0f);
            }
        }
        else if (_CurrentState == RoamerState.Attack)
        {
            Attack();
            if (FromAttackToFollow())
            {
                _CurrentState = RoamerState.Follow;
            }
        }

    }

    void SetRandomWanderPatrolPoint()
    {
       
            _WanderPoint = new Vector3
            (
            _WanderAreaPosition.x + Random.Range(-0.5f * _WanderAreaScale.x, 0.5f * _WanderAreaScale.x),
            _WanderAreaPosition.y + Random.Range(-0.5f * _WanderAreaScale.y, 0.5f * _WanderAreaScale.y),
            _WanderAreaPosition.z + Random.Range(-0.5f * _WanderAreaScale.z, 0.5f * _WanderAreaScale.z)
            );
       
    }

    bool FromAttackToFollow()
    {
        return Vector3.Distance(transform.position, _Player.transform.position) > _AttackRange + _AttackEscapeRange;
    }
    bool FromFollowToPatrol()
    {
        return Vector3.Distance(transform.position, _Player.transform.position) > _FollowRange + _FollowEscapeRange;
    }
    bool FromFollowToAttack()
    {
        return Vector3.Distance(transform.position, _Player.transform.position) < _AttackRange;
    }
    bool FromPatrolToFollow()
    {
        return Vector3.Distance(transform.position, _Player.transform.position) < _FollowRange;
    }

    void MakeIdle()
    {
        if (_CurrentState == RoamerState.Wander)
        {
            if (_UntilNextIdleTime >= 0.0f)
            {
                _UntilNextIdleTime -= Time.fixedDeltaTime;
                _IdleForTime = Random.Range(1.0f,5.0f);
                _IsIdling = false;
            }
            else if (_IdleForTime >= 0.0f)
            {
                _IdleForTime -= Time.fixedDeltaTime;
                _IsIdling = true;
            }
            else
            {
                _IdleForTime = Random.Range(1.0f, 5.0f);
                _UntilNextIdleTime = Random.Range(5.0f, 20.0f);
                SetRandomWanderPatrolPoint();
            }

        }
    }
    public void LookTowards(Vector3 target, float turnspeed)
    {
        
            Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
            float str;
            str = Mathf.Min(turnspeed * Time.deltaTime, 1);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);
        
    }
}
