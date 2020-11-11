using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureProjectile : MonoBehaviour
{
    Rigidbody _rigidBody;
    Health _targetHealth;
    private float _damageTimer;
    public float _maxDamageTime;
    private float _lifeTime;
    // Start is called before the first frame update

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _damageTimer = _maxDamageTime;
        _lifeTime = 3.0f;
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
                    _targetHealth.TakeDamage(5.0f);
                }
            }
        }
        else
        {
            _lifeTime = 3.0f;
            ObjectPool.Instance.ReturnToPool("Creature", this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");
        _targetHealth = collision.gameObject.GetComponent<Health>();
        if (_targetHealth != null)
        {
            Debug.Log("Sticking");
            transform.parent = collision.transform;
            Stick();
        }
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
    }
}
