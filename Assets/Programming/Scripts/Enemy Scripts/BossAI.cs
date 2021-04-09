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
    Health _health;
    State _currentBossState;
    Vector3 boxDimensions;
    float ucLength;
    event Action OnWeakStateStarted;
    event Action OnWeakStateEnded;
    public float _ucHealth = 20.0f;
    public GameObject _deathParticle;

    private void Awake()
    {
        int UCindex = 0;
        _umbilicalCords = new GameObject[3];
        foreach (Transform g in transform)
        {
            if (g.gameObject.name == "Weak")
            {
                Debug.Log("Weak Component Found");
                _health = g.GetComponent<Health>();
                g.GetComponent<Health>().OnDeath += BossDeath;
            }
            if (g.GetComponent<UmbilicalCord>() != null)
            {
                _umbilicalCords[UCindex] = g.GetComponent<UmbilicalCord>().gameObject;
                UCindex++;
            }
        }



        direction = (Vector3.down - transform.position);


        Vector3 _rotAxis = Vector3.forward;
        GameObject _rot = gameObject;


        OnWeakStateEnded += RegenerateUC;
    }

    private void Start()
    {
        _currentBossState = new UCState(gameObject);
    }

    void BossDeath()
    {
        gameObject.SetActive(false);
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        EventBroker.CallOnGameEnd();
    }

    void RegenerateUC()
    {
        foreach (GameObject g in _umbilicalCords)
        {
            foreach (Transform f in g.transform)
            {
                f.gameObject.SetActive(true);
                f.gameObject.GetComponent<Health>().Heal(_ucHealth);
            }
            g.SetActive(true);
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
        }
    }

    bool AreGOInactive()
    {
        if (GetComponentInChildren<UmbilicalCord>())
        {
            Debug.Log("Still UCs");
            return false;
        }
        return true;
    }

    public void CallOnWeakStateEnded()
    {
        OnWeakStateEnded?.Invoke();
    }

}
