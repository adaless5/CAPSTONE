using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapingEnemyProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    public float _ProjectileDamage;
    public ParticleSystem _splatParticle;
    public Rigidbody _rigidBody;
    Collider _hitCollider;
    public float _splatTime;
    float _currentSplatTime;
    public float _FullLifeTimer;
    float _LifeTimer;
    bool _bIsSplatting;
    void Start()
    {
        _LifeTimer = _FullLifeTimer;
        _bIsSplatting = false;
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _hitCollider = gameObject.GetComponent<Collider>();
        _splatParticle.Stop();
        _currentSplatTime = _splatTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.isTrigger)
        {

            if (collision.gameObject.GetComponentInParent<ALTPlayerController>() != null || collision.gameObject.GetComponentInParent<Tool>() != null)
            {

                FindObjectOfType<ALTPlayerController>().CallOnTakeDamage(_ProjectileDamage);
                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<Collider>().enabled = false;

            }
            // Debug.Log(collision.gameObject.transform.parent.tag);

            _bIsSplatting = true;
            _splatParticle.transform.position = transform.position;
        }
        // Debug.Log(collision.gameObject.transform.name);
    }

    void Splat()
    {
        _rigidBody.velocity = new Vector3();
        _rigidBody.angularVelocity = new Vector3();
        if (!_splatParticle.isPlaying)
        { _splatParticle.Play(); }


        if (_currentSplatTime > 0.0f)
        {
            _currentSplatTime -= Time.deltaTime;
        }

        else
        {
            _currentSplatTime = _splatTime;
            _bIsSplatting = false;
            gameObject.SetActive(false);
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (_bIsSplatting)
        {
            Splat();
        }
        if (gameObject.activeSelf == true)
        {
            if (_LifeTimer > 0.0f)
            {
                _LifeTimer -= Time.deltaTime;
            }
            else
            {
                _bIsSplatting = true;
            }
        }
    }
    public void Fire()
    {

        _currentSplatTime = _splatTime;
        _bIsSplatting = false;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
        _LifeTimer = _FullLifeTimer;
    }
}
