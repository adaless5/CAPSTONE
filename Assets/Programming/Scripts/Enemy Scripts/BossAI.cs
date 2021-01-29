using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    GameObject _ucReference;
    GameObject[] _umbilicalCords;
    private float _distance = 0.6f;
    private float[] _facePos = new float[3];
    public BoxCollider _boxCol;
    Vector3 direction;
    Health _health;
    BossState _currentBossState;

    private void Awake()
    {
        _health = GetComponent<Health>();

        direction = (Vector3.down - transform.position);
        _boxCol = GetComponent<BoxCollider>();
        _ucReference = (GameObject)Resources.Load("Prefabs/Enemies/Boss/Umbilical Cord");
        _umbilicalCords = new GameObject[3];



        Vector3 _rotAxis = Vector3.forward;
        GameObject _rot = gameObject;

        float ucLength = _ucReference.GetComponent<MeshRenderer>().bounds.size.magnitude;

        Vector3 boxDimensions = _boxCol.size;
        boxDimensions.x *= _boxCol.transform.lossyScale.x;
        boxDimensions.y *= _boxCol.transform.lossyScale.y;
        boxDimensions.z *= _boxCol.transform.lossyScale.z;

        Vector3 sidePos = new Vector3(_boxCol.center.x, _boxCol.center.y, _boxCol.center.z - 0.5f * boxDimensions.z - ucLength) + transform.localPosition;

        InitializeUC(_umbilicalCords[0], sidePos);

        sidePos = new Vector3(_boxCol.center.x - 0.5f * boxDimensions.x - ucLength, _boxCol.center.y, _boxCol.center.z) + transform.localPosition;

        InitializeUC(_umbilicalCords[1], sidePos);

        sidePos = new Vector3(_boxCol.center.x + 0.5f * boxDimensions.x + ucLength, _boxCol.center.y, _boxCol.center.z) + transform.localPosition;

        InitializeUC(_umbilicalCords[2], sidePos);

        //for (int i = 0; i < _umbilicalChords.Length; i++)
        //{

        //    RaycastHit hit;

        //    if (Physics.Raycast(_umbilicalChords[i].transform.position, Vector3.down, out hit))
        //    {
        //        direction = Vector3.down - _umbilicalChords[i].transform.position;
        //        _umbilicalChords[i].transform.LookAt(direction);
        //        _umbilicalChords[i].transform.localScale = new Vector3(1, 1, 5);
        //    }
        //    else
        //    {
        //        Debug.Log("No hit");
        //    }
        //    //_umbilicalChords[i]
        //}
    }

    private void Start()
    {
        _currentBossState = new UCState(gameObject);
    }

    void InitializeUC(GameObject umcord, Vector3 position)
    {
        umcord = Instantiate(_ucReference, position, Quaternion.identity) as GameObject;
        umcord.GetComponentInChildren<UmbilicalCord>().GetHealth().OnDeath += CheckUC;
        umcord.transform.SetParent(transform);
    }


    // Update is called once per frame
    void Update()
    {
        _currentBossState.Process();
        //Debug.Log(_currentBossState);
    }

    void CheckUC()
    {
        Debug.Log("Checking cords...");
        if (AreGOInactive())
        {
            Debug.Log("Weak Spot State");
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

}
