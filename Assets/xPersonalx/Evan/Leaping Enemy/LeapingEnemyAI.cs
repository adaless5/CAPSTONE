using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeapingEnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    public State _currentState;                             // current state of this enemy

    GameObject _playerReference;                            //reference to the player as a GameObject
    public GameObject[] _eyes;                              // the two eyes attacked to the leaper, as GameObjects
    public GameObject _jumpTarget;                          // the target that the leaper will jump towards, as a GameObject
    public GameObject _homeObject;                          // the area that the leaper will stay within when not chasing the player, and will return to if it loses the player
    public GameObject _jumpDirectionObject;                 //The object that the leaper orients and checks when leaping to determine the direction to jump
    public GameObject _ProjectileSpawnPoint;                // the position that the leaper's projectiles will be spawned in at.
    public Collider _body;                                  // the main collidere attached to this enemy that is used as its body/hit detection
    public Collider _landingCollider;                       // collider used for landing animation to detect when to start animation
    public Animator _leaperAnimator;                        // animator controller for leaping enemy animations

    public LeapingEnemyProjectile _LeapingEnemyProjectile;  // the projectile that the leaper will fire at the player when in attack state

    Rigidbody _rigidBody;                                   // the rigid body used by the enemy for body physics stuff

    public float _TouchDamage;                              // the amount of damage the leaper should deal to the player when the player makes contact with it
    public float _JumpTimer = 3f;
    private bool _bHasJumped = false;
    public float _AttackJumpSpeed;                          // the force used for the leaper's jump when the leaper is in its attack state
    public float _WanderJumpSpeed;                          // the force used for the leaper's jump when the leaper is in its wander state or go home state
    public float _ProjectileBaseSpeed;                      // the force that the leaper's projectile is spawned in with

    public float _jumpVelocity;

    public enum PlayerDistance { far, follow, attack };         // possible distance levels the leaper can consider for the player, used by the detection objects attached to the leaping enemy prefab to determine whether to change leaper state.
    public PlayerDistance _playerDistance;                  // local variable to store the current distance level the player is at, used by the detection objects attached to the leaping enemy prefab to determine whether to change leaper state.

    public Vector2 _fullIdleTimeRange;                      // the range of time that the leaper might take between jumps when in its idle state
    public Vector2 _fullFollowJumpTimeRange;                // the range of time that the leaper might take between jumps when in its follow state
    public Vector2 _fullGoHomeJumpTimeRange;                // the range of time that the leaper might take between jumps when in its go home state

    private void Awake()
    {
        EventBroker.OnPlayerSpawned += EventStart;
    }

    private void Start()
    {
        _LeapingEnemyProjectile.gameObject.SetActive(false);
        _rigidBody = GetComponent<Rigidbody>();
        _leaperAnimator = GetComponentInChildren<Animator>();
        //_landingCollider = GetComponent<CapsuleCollider>();
        //_landingCollider.enabled = false;
        
    }

    private void EventStart(GameObject player)
    {
        try
        {
            _currentState = new LeapingEnemyWanderState(gameObject, gameObject.GetComponent<LeapingEnemyAI>(), player.transform);
            _playerReference = player;

        }
        catch { }
    }

    void Update()
    {
        CheckAnimationState();
        if (_currentState != null)
        {
            _currentState = _currentState.Process();
        }
        else
        {
            _playerReference = GameObject.FindGameObjectWithTag("Player");
            if (_playerReference)
                _currentState = new LeapingEnemyWanderState(gameObject, gameObject.GetComponent<LeapingEnemyAI>(), _playerReference.transform);
        }
        MoveEyes();
        //if (_bHasJumped)
        //{
        //    _JumpTimer -= Time.deltaTime;
        //    if (_JumpTimer <= 0)
        //    {
        //        _landingCollider.enabled = true;
        //        _bHasJumped = false;
        //    }
        //}
    }

    void MoveEyes()
    {
        foreach (GameObject eye in _eyes)
        {
            LookTowards(eye.transform, _jumpTarget.transform.position, 10.0f);
        }
    }

    public void LookTowards(Transform thisTransform, Vector3 target, float turnspeed)
    {

        Quaternion targetRotation = Quaternion.LookRotation(target - thisTransform.position);
        float str;
        str = Mathf.Min(turnspeed * Time.deltaTime, 1);
        thisTransform.rotation = Quaternion.Lerp(thisTransform.rotation, targetRotation, str);

    }
    public void SetCurrentLeapingEnemyState(DroneState state)
    {
        _currentState = state;
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, 1.5f);
    }

    public IEnumerator LeapAnimation(float baseSpeed)
    {
        _leaperAnimator.SetBool("IsJumping", true);
        yield return new WaitForSeconds(0.25f);
        _rigidBody.velocity = (_jumpDirectionObject.transform.forward) * (baseSpeed);// * Vector3.Distance(transform.position, _jumpTarget.transform.position) );

    }
    public void Leap(float baseSpeed)
    {
        StartCoroutine(LeapAnimation(baseSpeed));
        //_leaperAnimator.SetTrigger("IsLeaping");
        //_landingCollider.enabled = false;
        //_bHasJumped = true;
        //_JumpTimer = 3f;
    }

    public bool IsLanding()
    {
        return Physics.Raycast(transform.position, Vector3.down, 2.3f);
    }

    public void CheckAnimationState()
    {
        _jumpVelocity = _rigidBody.velocity.y;
        _leaperAnimator.SetFloat("JumpHeight", _jumpVelocity);

        if(_jumpVelocity < -1)
        {
            if(IsLanding())
            {
                _leaperAnimator.SetBool("IsJumping", false);
            }
        }            

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Collided with " + other.name);
    //    if(other.tag != "LeaperExtras")
    //    {
    //    //Debug.Log("COLLIDER HAS TRIGGERED");
    //    //_landingCollider.enabled = false;
    //    _leaperAnimator.SetBool("IsJumping", false);
    //    //_leaperAnimator.SetTrigger("IsLanding");

    //    }
        
    //}
}
