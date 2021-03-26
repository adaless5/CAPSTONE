using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportingEnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update

    public ParticleSystem _chargeParticles;     // the particles played when the enemy begins to charge its attack
    public ParticleSystem _attackParticles;     // the particles played when the enemy fires its attack

    public float _attackDamage;                 // the damage that is done to the player when the attack hits

    public float _attackTime;                   // the amount of time that the attack lasts for
    float _currentAttackTime;
    public float _chargeTime;                   // the amount of time that the enemy charges its attack
    float _currentChargeTime;                   
    public bool _isAttacking;                   // used to trigger the entire attack sequence
    public bool _isFiring;                             // flags when the attack is fired
    bool _isCharging;                           // flags when the attack is charging, stays flagged while the attack is fired

    void Start()
    {
        _isAttacking = false;
        _isFiring = false;
        _isCharging = false;
    }
    private void Awake()
    {
        _chargeParticles.Stop();
        _attackParticles.Stop();
        GetComponent<Collider>().enabled = false;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.isTrigger)
        {
            if (collider.gameObject.GetComponentInParent<ALTPlayerController>() != null || collider.gameObject.GetComponentInParent<Tool>() != null)
            {
                FindObjectOfType<ALTPlayerController>().CallOnTakeDamage(_attackDamage);
                GetComponent<Collider>().enabled = false;
            }
        }
    }
    public void Attack()
    {
        _isAttacking = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (_isAttacking)
        {
            Charge();
        }
        else
        {
            _chargeParticles.Stop();
            _attackParticles.Stop();
        }

    }

    void Fire()
    {
        if (!_isFiring)
        {
            _attackParticles.Play();
            GetComponent<Collider>().enabled = true;
            _isFiring = true;
        }
            _currentAttackTime -= Time.deltaTime;
        if (_currentAttackTime < 0.0f)
        {
            GetComponent<Collider>().enabled = false;
            _isAttacking = false;
            _isCharging = false;
            _isFiring = false;
            _currentAttackTime = _attackTime;
            _currentChargeTime = _chargeTime;
            _attackParticles.Stop();
            _chargeParticles.Stop();
        }
    }
    void Charge()
    {
        if (!_isCharging)
        {
            _chargeParticles.Play();
            _isCharging = true;
        }
        if (_currentChargeTime < 0.0f)
        {
            Fire();
        }
        else
        {
            _currentChargeTime -= Time.deltaTime;
        }
    }

}
