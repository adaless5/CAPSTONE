using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportingEnemyAI : MonoBehaviour
{
    // Start is called before the first frame update

    public ParticleSystem _localTeleportParticles;      // the teleport particles used when this enemy disappears
    public ParticleSystem _targetTeleportParticles;     // The teleport particles used when this enemy appears at its target
    public TeleportingEnemyAnimation _enemyAnimation;   // the TeleportingEnemyAnimation component used by this enemy
    public State _currentState;                         // the current state that the enemy is in
    public Vector3 _teleportTarget;                     // the location that theenemy will attempt to teleport to
    public GameObject _enemyModelObject;                // the object that contains the model components for the enemy
    public TeleportingEnemyAttack _attack;

    public float _followDistance;                       // the distance the player must be within for the enemy to be in follow state
    public float _attackDistance;                       // the distance the player must be within for the enemy to be in attack state

    public Vector2 _attackTimeRange;                    // the range of time that the enemy may take to attack
    public bool _hasDisappeared;                        // is turned on when the enemy has disappeared and not yet reappeared
    public Vector2 _attackTeleportTimeRange;            // the range of time the enemy may take between teleports during the attack state
    public float _followTeleportTime;                   // the length of time the enemy will take between teleports when in follow state
    public float _teleportRange;                        // the distance the enemy is able to travel with a teleport (with additional random range of + or - _teleportDistance)
    public float _lookSpeed;                            // the speed at which the enemy will turn to face its target
    public float _reappearTime;                         // the ammount of time it takes for the enemy to reappear after teleporting away (may take longer if the enemy has trouble finding a valid position to teleport to)
    public float _teleportDistance;                     // the distance from the target the enemy may teleport within (allows the enemy to avoid being unable to find a place to spawn)
    float _hoverTime;
    float _currentReappearTime;
    private void Awake()
    {
        _enemyAnimation = GetComponentInChildren<TeleportingEnemyAnimation>();
        EventBroker.OnPlayerSpawned += EventStart;
    }
    private void EventStart(GameObject player)
    {
        try
        {
            _currentState = new TeleportingEnemyIdleState(gameObject.GetComponent<TeleportingEnemyAI>(), player.transform, _enemyAnimation, _attack);
        }
        catch { }

    }

    public void Teleport(Vector3 target)
    {
        try 
        {
            Debug.Log("TELEPORT");
            AudioManager_Teleporter audioManager = GetComponent<AudioManager_Teleporter>();
            if (!audioManager.teleportIsPlaying()) audioManager.TriggerTeleport();
        }
        catch { }

        if (!_hasDisappeared)
        {
            Disappear();
        }
        if (_currentReappearTime < 0.0f)
        {
            RaycastHit hit;

            _teleportTarget = target + (Random.insideUnitSphere * _teleportDistance);
            _teleportTarget.y = target.y + Random.Range(1.0f, _teleportDistance / 2);

            if (Physics.SphereCast(_teleportTarget, 1.5f, Vector3.down, out hit, 1.5f))
            {
                Reappear(_teleportTarget);
            }
        }
        else
        {
            _currentReappearTime -= Time.deltaTime;
        }

    }

    public void Disappear()
    {
        _localTeleportParticles.transform.position = transform.position;
        _localTeleportParticles.Play();
        _enemyModelObject.SetActive(false);
        _hasDisappeared = true;
    }

    public void Reappear(Vector3 target)
    {
        _currentReappearTime = _reappearTime;
        _hasDisappeared = false;
        transform.position = target;
        _targetTeleportParticles.transform.position = transform.position;
        _targetTeleportParticles.Play();
        _enemyModelObject.SetActive(true);

    }

    public void LookTowards(Transform thisTransform, Vector3 target, float turnspeed)
    {

        Quaternion targetRotation = Quaternion.LookRotation(target - thisTransform.position);
        float str;
        str = Mathf.Min(turnspeed * Time.deltaTime, 1);
        thisTransform.rotation = Quaternion.Lerp(thisTransform.rotation, targetRotation, str);

    }


    public void Attack()
    {
            if (!_attack._isAttacking)
            {
                _attack.Attack();
                
                try { GetComponent<AudioManager_Telenemy>().TriggerAttack(); }
                catch { }
            }
        
    }
    // Update is called once per frame
    private void Update()
    {
        if (_currentState != null)
        {
            _currentState = _currentState.Process();
        }
        else
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _currentState = new TeleportingEnemyIdleState(gameObject.GetComponent<TeleportingEnemyAI>(), player.transform, _enemyAnimation,_attack);
            }
        }
    }
}
