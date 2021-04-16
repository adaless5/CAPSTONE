using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbilicalCord : MonoBehaviour
{
    public Health _health;
    public Animator _meatSackAnimator;
    public Animator _tendonAnimator;
    public bool _bIsDead;
   // public BossAI _bossRef;

    private void Awake()
    {
        //_bossRef = GetComponentInParent<BossAI>();
        _meatSackAnimator = GetComponentInChildren<Animator>();
        _tendonAnimator = gameObject.transform.Find("UmbilicalCord").GetComponentInChildren<Animator>();           
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
        _meatSackAnimator.SetTrigger("Death");
        _tendonAnimator.SetTrigger("Death");
        Debug.Log("UC died");
        SetDead();
    }    

    public void PlayUCRegenAnimation()
    {
        _meatSackAnimator.SetTrigger("Regen");
        _tendonAnimator.SetTrigger("Regen");
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
