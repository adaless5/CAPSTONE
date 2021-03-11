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
    public BoxCollider _boxCol;
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
        foreach (Transform g in transform)
        {
            if (g.gameObject.name == "Weak")
            {
                Debug.Log("Weak Component Found");
                _health = g.GetComponent<Health>();
                g.GetComponent<Health>().OnDeath += BossDeath;
            }
        }

        direction = (Vector3.down - transform.position);
        _boxCol = GetComponent<BoxCollider>();
        _ucReference = (GameObject)Resources.Load("Prefabs/Enemies/Boss/Umbilical Cord");
        _umbilicalCords = new GameObject[3];



        Vector3 _rotAxis = Vector3.forward;
        GameObject _rot = gameObject;

        ucLength = _ucReference.GetComponent<MeshRenderer>().bounds.size.magnitude;

        boxDimensions = _boxCol.size;
        boxDimensions.x *= _boxCol.transform.lossyScale.x;
        boxDimensions.y *= _boxCol.transform.lossyScale.y;
        boxDimensions.z *= _boxCol.transform.lossyScale.z;



        Vector3 sidePos = new Vector3(_boxCol.center.x, _boxCol.center.y, _boxCol.center.z - 0.5f * boxDimensions.z - ucLength) + transform.localPosition;

        InitializeUC(ref _umbilicalCords[0], sidePos);

        sidePos = new Vector3(_boxCol.center.x - 0.5f * boxDimensions.x - ucLength, _boxCol.center.y, _boxCol.center.z) + transform.localPosition;

        InitializeUC(ref _umbilicalCords[1], sidePos);

        sidePos = new Vector3(_boxCol.center.x + 0.5f * boxDimensions.x + ucLength, _boxCol.center.y, _boxCol.center.z) + transform.localPosition;

        InitializeUC(ref _umbilicalCords[2], sidePos);

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

    void InitializeUC(ref GameObject umcord, Vector3 position)
    {
        umcord = Instantiate(_ucReference, position, Quaternion.identity) as GameObject;
        umcord.GetComponentInChildren<UmbilicalCord>().GetHealth().OnDeath += CheckUC;
        umcord.transform.SetParent(transform);
    }


    // Update is called once per frame
    void Update()
    {
        if (_currentBossState != null)
            _currentBossState = _currentBossState.Process();
        //Debug.Log(_currentBossState);
    }

    void CheckUC()
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
