using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapingEnemy : MonoBehaviour
{
    // Start is called before the first frame update

    public enum BehaviourState { Wander, Follow, Attack, GoHome };
    public BehaviourState _behaviourState;

    public GameObject[] _eyes;
    public GameObject _jumpTarget;
    public GameObject _homeObject;
    public GameObject _jumpDirectionObject;
    public GameObject _ProjectileSpawnPoint;
    public Collider _body;
    public LeapingEnemyProjectile _LeapingEnemyProjectile;

    Rigidbody _rigidBody;

    public float _TouchDamage;

    public float _AttackJumpSpeed;
    public float _WanderJumpSpeed;
    public float _ProjectileBaseSpeed;


    public Vector2 _fullIdleTimeRange;
    float _currentIdleTime;

    public Vector2 _fullFollowJumpTimeRange;
    float _currentFollowJumpTime;

    public Vector2 _fullGoHomeJumpTimeRange;
    float _currentGoHomeJumpTime;


    void Behavior()
    {
        if (_behaviourState == BehaviourState.Wander)
        {
            Wander();
        }
        else if (_behaviourState == BehaviourState.Attack)
        {
            SpitAttack();
            FollowJump();
        }
        else if (_behaviourState == BehaviourState.Follow)
        {
            FollowJump();
        }
        else if (_behaviourState == BehaviourState.GoHome)
        {
            GoHomeJump();
        }
    }
    void GoHomeJump()
    {
        _jumpTarget.transform.position = _homeObject.transform.position;

        if (_currentGoHomeJumpTime > 0.0f)
        {
            _currentGoHomeJumpTime -= Time.deltaTime;
        }
        else
        {
            if (Vector3.Distance(transform.position, _homeObject.transform.position) < _homeObject.transform.localScale.x)
            {
                _behaviourState = BehaviourState.Wander;
            }
            else if (IsGrounded())
            {
                Leap(_WanderJumpSpeed);
                _currentGoHomeJumpTime = Random.Range(_fullGoHomeJumpTimeRange.x, _fullGoHomeJumpTimeRange.y);
            }
        }
    }
    void FollowJump()
    {
        _jumpTarget.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;

        if (_currentFollowJumpTime > 0.0f)
        {
            _currentFollowJumpTime -= Time.deltaTime;
        }
        else if (IsGrounded())
        {

            Leap(_AttackJumpSpeed);

            _currentFollowJumpTime = Random.Range(_fullFollowJumpTimeRange.x, _fullFollowJumpTimeRange.y);
        }
    }
    void SpitAttack()
    {
        if (_LeapingEnemyProjectile.gameObject.activeSelf == false)
        {
            _LeapingEnemyProjectile.gameObject.SetActive(true);
            _LeapingEnemyProjectile.Fire();
            _LeapingEnemyProjectile.transform.position = transform.position + (transform.forward * 2.0f);
            _LeapingEnemyProjectile._rigidBody.velocity = _ProjectileBaseSpeed * (_jumpTarget.transform.position-transform.position + _ProjectileSpawnPoint.transform.forward  ).normalized;

        }
    }
    void Wander()
    {
        if (_currentIdleTime > 0.0f)
        {
            _currentIdleTime -= Time.deltaTime;
        }
        else if (IsGrounded())
        {
            if (Vector3.Distance(transform.position, _homeObject.transform.position) > _homeObject.transform.localScale.x)
            {
                _behaviourState = BehaviourState.GoHome;
            }
            else
            {
                _jumpTarget.transform.position = _homeObject.transform.position + (Random.insideUnitSphere * _homeObject.transform.localScale.x);
                Leap(_WanderJumpSpeed);
            }
            _currentIdleTime = Random.Range(_fullIdleTimeRange.x, _fullIdleTimeRange.y);
        }
    }
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _currentIdleTime = Random.Range(_fullIdleTimeRange.x, _fullIdleTimeRange.y);
        _LeapingEnemyProjectile.gameObject.SetActive(false);
        _currentFollowJumpTime = Random.Range(_fullFollowJumpTimeRange.x, _fullFollowJumpTimeRange.y);
        _currentGoHomeJumpTime = Random.Range(_fullGoHomeJumpTimeRange.x, _fullGoHomeJumpTimeRange.y);
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.transform.parent != null)
        {
            if (other.gameObject.transform.parent.tag == "Player")
            {
                _rigidBody.velocity = other.GetContact(0).normal * 10.0f;//other.GetContact(0).normal * 50.0f;

                other.gameObject.transform.parent.GetComponent<ALTPlayerController>().CallOnTakeDamage(_TouchDamage);

            }
        }

    }
    bool IsGrounded()
    {
        //Debug.DrawRay(transform.position, -Vector3.up * 1.1f);
        return Physics.Raycast(transform.position, -Vector3.up, 1.0f);
    
    }
    void Leap(float baseSpeed)
    {
        _rigidBody.velocity = (_jumpDirectionObject.transform.forward) * (baseSpeed);// * Vector3.Distance(transform.position, _jumpTarget.transform.position) );
    }
    void LookTowards(Transform thisTransform, Vector3 target, float turnspeed)
    {

        Quaternion targetRotation = Quaternion.LookRotation(target - thisTransform.position);
        float str;
        str = Mathf.Min(turnspeed * Time.deltaTime, 1);
        thisTransform.rotation = Quaternion.Lerp(thisTransform.rotation, targetRotation, str);

    }
    // Update is called once per frame
    void Update()
    {

        LookTowards(transform, _jumpTarget.transform.position, 5.0f);
        foreach (GameObject eye in _eyes)
        {
            LookTowards(eye.transform, _jumpTarget.transform.position, 10.0f);
        }
        Behavior();
        _rigidBody.angularVelocity = new Vector3();
    }
}
