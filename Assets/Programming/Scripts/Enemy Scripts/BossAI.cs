using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BossAI : MonoBehaviour
{
    GameObject _ucReference;
    public GameObject[] _umbilicalCords;
    private float _distance = 0.6f;
    private float[] _facePos = new float[3];
    public MeshCollider _boxCol;
    Vector3 direction;
    public Health _health;
    State _currentBossState;
    Vector3 boxDimensions;
    float ucLength;
    event Action OnWeakStateStarted;
    event Action OnArmSmashStateEnded;
    public float _ucHealth = 20.0f;
    public GameObject _deathParticle;
    public GameObject _arm;
    public Animator _bossAnimator;
    public Transform _spawnPoint;
    public GameObject _weakPoint;
    public float _overallHealth;

    bool bIsDead = false;

    private void Awake()
    {
        _spawnPoint = transform.Find("Boss_Animated_Model/Stomach Attach Point");
        _bossAnimator = GetComponentInChildren<Animator>();
        int UCindex = 0;
        _umbilicalCords = new GameObject[3];
        _weakPoint = GameObject.Find("Weak");
        _health = _weakPoint.GetComponent<Health>();
        

        foreach (Transform g in transform)
        {
            if (g.gameObject.name.Contains("Arm"))
            {
                _arm = g.gameObject;
            }
            //if (g.gameObject.name == "Weak")
            //{
            //    _weakPoint = g.gameObject;
            //    Debug.Log("Weak Component Found");
            //    _health = g.GetComponent<Health>();
            //    _health.OnTakeDamage += TakingDamage;
            //    g.GetComponent<Health>().OnDeath += BossDeath;
            //}            
            if (g.GetComponent<UmbilicalCord>() != null)
            {
                _umbilicalCords[UCindex] = g.GetComponent<UmbilicalCord>().gameObject;
                UCindex++;
            }
        }

        direction = (Vector3.down - transform.position);


        Vector3 _rotAxis = Vector3.forward;
        GameObject _rot = gameObject;

        OnArmSmashStateEnded += RegenerateUC;
        _health.OnTakeDamage += TakingDamage;
        _weakPoint.GetComponent<Health>().OnDeath += BossDeath;
        _weakPoint.SetActive(false);
    }

    private void Start()
    {
        _currentBossState = new UCState(gameObject);
    }

    public GameObject GetBossArm()
    {
        return _arm;
    }

    IEnumerator BossDeathLogic()
    {
        _bossAnimator.SetTrigger("OnDeath");
        yield return new WaitForSeconds(8f);
        //gameObject.SetActive(false);
        //for (int i = 0; i < transform.childCount; ++i)
        //{
        //    transform.GetChild(i).gameObject.SetActive(false);
        //}
        EventBroker.CallOnGameEnd();
    }


    void BossDeath()
    {
        bIsDead = true;
        StartCoroutine(BossDeathLogic());
    }

    IEnumerator RegenerateAnimation()
    {
        //_bossAnimator.SetTrigger("Regenerating");
        _bossAnimator.SetBool("IsRegenerating", true);
        //Waits till Boss slams his arm down before spawning, about 1.2 seconds in regen animation
        yield return new WaitForSeconds(1.145f);
        foreach (GameObject g in _umbilicalCords)
        {
            //foreach (Transform f in g.transform)
            //{
            //    //f.gameObject.SetActive(true);
            //    f.gameObject.GetComponent<UmbilicalCord>()._bIsDead = false;
            //    f.gameObject.GetComponent<UmbilicalCord>().PlayUCRegenAnimation();
            //    f.gameObject.GetComponent<Health>().Heal(_ucHealth);
            //}
            //g.SetActive(true);
            try
            {

                g.GetComponent<UmbilicalCord>()._bIsDead = false;
                g.GetComponent<UmbilicalCord>().PlayUCRegenAnimation();
                g.GetComponent<Health>().Heal(_ucHealth);
            }
            catch { }
        }
        yield return new WaitForSeconds(.5f);
        _bossAnimator.SetBool("IsRegenerating", false);
    }

    void RegenerateUC()
    {
        _bossAnimator.SetBool("IsWeak", false);
        if (!bIsDead)
        {
            StartCoroutine(RegenerateAnimation());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentBossState != null)
            _currentBossState = _currentBossState.Process();
        //Debug.Log(_currentBossState);
    }

    public void CheckUC()
    {
        Debug.Log("Checking cords...");
        if (AreGOInactive())
        {
            Debug.Log("Weak Spot State");
            _currentBossState = new BossWeakState(gameObject);
            _bossAnimator.SetBool("IsWeak", true);
        }
    }

    bool AreGOInactive()
    {
        //if (!GetComponentInChildren<UmbilicalCord>()._bIsDead)
        //{
        //    Debug.Log("Still UCs");
        //    return false;
        //}
        float amountOfUC = 0;
        for (int i = 0; i < _umbilicalCords.Length; i++)
        {
            if (_umbilicalCords[i].GetComponent<UmbilicalCord>()._bIsDead)
            {
                amountOfUC += 1;
            }
            //else
            //{
            //    return false;
            //}
        }
        //return false;
        if (amountOfUC == 3)
        {
            return true;
        }
        else
        {
            amountOfUC = 0;
            return false;
        }
    }

    public void SetUCAnimationPosition(GameObject UC)
    {
        _bossAnimator.SetTrigger("UC_Death");
        for (int i = 0; i < _umbilicalCords.Length; i++)
        {
            if (_umbilicalCords[i] == UC)
            {
                _bossAnimator.SetInteger("UC_Side", i);
            }
        }
    }

    public void SetArmSmashAnimation()
    {
        _bossAnimator.SetTrigger("ArmSmash");
    }

    public void SetDroneAndHomingSpawnAnimation()
    {
        _bossAnimator.SetTrigger("Spawning");
    }

    public void CallOnMeleeAttackEnded()
    {
        OnArmSmashStateEnded?.Invoke();
    }

    public void TakingDamage()
    {
        if (!bIsDead)
        {
            _bossAnimator.SetTrigger("HitReact");
        }
    } 

}
