using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingAttackScript : MonoBehaviour
{
    GameObject _player;
    Vector3 _newPos;
    Vector3 _originalPos;
    public float _damageValue;
    Health _health;
    Animator _haAnimator;
    private void Awake()
    {
        _health = GetComponent<Health>();
        _haAnimator = GetComponent<Animator>();
        _health.OnDeath += PlayDeathAnimation;
        if (_damageValue == 0)
            _damageValue = 30;
        _player = GameObject.FindGameObjectWithTag("Player");
        _originalPos = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        _originalPos = transform.position;
        _newPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _newPos.y += Mathf.Sin(Time.time) * Time.deltaTime;
        _newPos = Vector3.Slerp(_newPos, _player.transform.position, 0.5f * Time.deltaTime);
        transform.position = _newPos;
        if (Vector3.Distance(transform.position, _player.transform.position) <= 3.0f)
        {
            ApplyDamage();
        }
    }

    void PlayDeathAnimation()
    {
        _haAnimator.SetTrigger("Death");
    }

    public void ReturnToPoolDead()
    {
        ObjectPool.Instance.ReturnToPool("Homing Attack", gameObject);
        _newPos = _originalPos;
    }
    
    void ApplyDamage()
    {
        _player.GetComponent<ALTPlayerController>().CallOnTakeDamage(_damageValue);
        ObjectPool.Instance.ReturnToPool("Homing Attack", gameObject);
        _newPos = _originalPos;
    }

    private void OnTriggerEnter(Collider collision)
    {

    }
}
