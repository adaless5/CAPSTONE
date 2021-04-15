using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbilicalCord : MonoBehaviour
{
    public Health _health;
    public Animator _ucAnimator;
    public bool _bIsDead;
   // public BossAI _bossRef;

    private void Awake()
    {
        //_bossRef = GetComponentInParent<BossAI>();
        _ucAnimator = GetComponentInChildren<Animator>();
        _health = GetComponentInChildren<Health>();
        _health.OnDeath += PlayDeathAnimation;
        _bIsDead = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    
    void PlayDeathAnimation()
    {  
        _ucAnimator.SetTrigger("Death");
        Debug.Log("UC died");
        SetDead();
    }    

    public void PlayUCRegenAnimation()
    {
        _ucAnimator.SetTrigger("Regen");
    }

    public void SetDead()
    {
        //gameObject.SetActive(false);
        _bIsDead = true;
        gameObject.GetComponentInParent<BossAI>().CheckUC();
        gameObject.GetComponentInParent<BossAI>().SetUCAnimationPosition(gameObject);
    }

    public Health GetHealth()
    {
        return _health;
    }
}
