using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureProjectile : MonoBehaviour
{
    Rigidbody _rigidBody;
    Health _targetHealth;
    GameObject _target;
    private float _damageTimer;
    public float _maxDamageTime;
    public float _lifeTime;
    Transform _transformOrigin;

    AudioManager_CreatureWeapon audioManager;

    bool m_bHasAction = false;
    float _damage;
    float _targetDefaultSpeed;
    // Start is called before the first frame update

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _transformOrigin = ObjectPool.Instance.transform;
        _damageTimer = _maxDamageTime;
       // _lifeTime = 6.0f;
    }

    public void InitCreatureProjectile(float maxdamagetime, float lifetime, float damage, bool hasaction)
    {
        //_maxDamageTime = maxdamagetime;
        _lifeTime = lifetime;
        _damage = damage;
        m_bHasAction = hasaction;
        _rigidBody.velocity = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        _lifeTime -= Time.deltaTime;

        if (_lifeTime >= 0)
        {
            if (_targetHealth != null)
            {
                _damageTimer -= Time.deltaTime;
                if (_damageTimer <= 0)
                {
                    _damageTimer = _maxDamageTime;
                    _targetHealth.TakeDamage(_damage);
                }
            }
        }
        else
        {
            _lifeTime = 3.0f;
            transform.parent = _transformOrigin;
            DeStick();
            ObjectPool.Instance.ReturnToPool("Creature", gameObject);
        }

        if (gameObject.activeSelf == false)
        {
            transform.parent = _transformOrigin;
            DeStick();
        }

    }
    private void OnEnable()
    {
        _lifeTime = 3.0f;
    }

    private void OnDisable()
    {
        _lifeTime = 3.0f;
        //transform.parent = _transformOrigin;
        if (m_bHasAction)
        {
            if (_target.GetComponent<TEMP_Roamer>())
            {
                _target.GetComponent<TEMP_Roamer>()._FollowSpeed = _targetDefaultSpeed;
            }
        }

        DeStick();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Hit");
        if (m_bHasAction)
        {
            _target = collision.gameObject;
            if (_target.GetComponent<TEMP_Roamer>())
            {
                _targetDefaultSpeed = _target.GetComponent<TEMP_Roamer>()._FollowSpeed;
                _target.GetComponent<TEMP_Roamer>()._FollowSpeed *= 0.8f;
            }
        }

        _targetHealth = collision.gameObject.GetComponent<Health>();
        if (_targetHealth != null)
        {
            Debug.Log("Sticking");
            transform.parent = collision.transform;
            Stick();
        }
        DestructibleObject wall = collision.transform.GetComponentInParent<DestructibleObject>();
        if (wall)
        {
            wall.Break(gameObject.tag);
        }
        /// Evan's Item container call vvv
        ItemContainer container = collision.transform.GetComponentInParent<ItemContainer>();
        if (container)
        {
            container.Break(gameObject.tag);
        }
        /// Evan's Item container call ^^^
    }
    void DeStick()
    {
        _rigidBody.isKinematic = false;
        _rigidBody.detectCollisions = true;

    }
    void Stick()
    {
        _rigidBody.isKinematic = true;
        _rigidBody.detectCollisions = false;

        audioManager.TriggerStickCreatureWeapon(GetComponentInChildren<AudioSource>());
    }

    public void LinkAudioManager(AudioManager_CreatureWeapon amc)
    {
        audioManager = amc;
    }
}
