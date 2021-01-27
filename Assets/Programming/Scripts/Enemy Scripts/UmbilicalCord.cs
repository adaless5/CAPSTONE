using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbilicalCord : MonoBehaviour
{
    Health _health;
    private bool isDead;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _health.OnDeath += SetDead;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void SetDead()
    {

        transform.parent.gameObject.SetActive(false);
    }

    public Health GetHealth()
    {
        return _health;
    }
}
