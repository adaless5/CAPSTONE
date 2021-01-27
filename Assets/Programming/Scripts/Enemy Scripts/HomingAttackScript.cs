using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingAttackScript : MonoBehaviour
{
    GameObject _player;
    Vector3 _newPos;
    public float _damageValue;
    private void Awake()
    {
        if (_damageValue == 0)
            _damageValue = 30;
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        _newPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _newPos.y += Mathf.Sin(Time.time) * Time.deltaTime;
        _newPos = Vector3.Lerp(_newPos, _player.transform.position, 0.5f * Time.deltaTime);
        transform.position = _newPos;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == _player.name)
        {
            _player.GetComponent<ALTPlayerController>().CallOnTakeDamage(_damageValue);
        }

        //TODO: Object Pool this
        Destroy(gameObject);
    }
}
