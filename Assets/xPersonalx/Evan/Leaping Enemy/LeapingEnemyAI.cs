using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeapingEnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    public State _currentState;
    GameObject _playerReference;
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

    public enum PlayerDistance {far,follow,attack};
    public PlayerDistance _playerDistance;
    public Vector2 _fullIdleTimeRange;
    public Vector2 _fullFollowJumpTimeRange;
    public Vector2 _fullGoHomeJumpTimeRange;

    private void Awake()
    {
        EventBroker.OnPlayerSpawned += EventStart;
    }
    private void Start()
    {
        _LeapingEnemyProjectile.gameObject.SetActive(false);
        _rigidBody = GetComponent<Rigidbody>();
    }
    private void EventStart(GameObject player)
    {
        try
        {
            _currentState = new LeapingEnemyWanderState(gameObject,gameObject.GetComponent<LeapingEnemyAI>() ,player.transform);
            _playerReference = player;

        }
        catch { }
    }

        void Update()
    {
        if (_currentState != null)
        {
            _currentState = _currentState.Process();
        }
        else
        {
            _playerReference = GameObject.FindGameObjectWithTag("Player");
            if(_playerReference)
            _currentState = new LeapingEnemyWanderState(gameObject, gameObject.GetComponent<LeapingEnemyAI>(), _playerReference.transform);
        }
        MoveEyes();
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
    public void Leap(float baseSpeed)
    {
        _rigidBody.velocity = (_jumpDirectionObject.transform.forward) * (baseSpeed);// * Vector3.Distance(transform.position, _jumpTarget.transform.position) );
    }
}
