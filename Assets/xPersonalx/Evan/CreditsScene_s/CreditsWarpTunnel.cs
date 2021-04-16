using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsWarpTunnel : MonoBehaviour
{
    public GameObject[] _tunnels;
    public float _warpSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _tunnels.Length; i++)
        {
            //_tunnels[i].transform.position = new Vector3(_tunnels[i].transform.position.x + Time.deltaTime * _warpSpeed, 0,0);
            _tunnels[i].transform.position = new Vector3(_tunnels[i].transform.position.x + Time.deltaTime * _warpSpeed, _tunnels[i].transform.position.y, _tunnels[i].transform.position.z);
            if (_tunnels[i].transform.position.x>750)
            {
                //_tunnels[i].transform.position = new Vector3(-750,0,0);
                _tunnels[i].transform.position = new Vector3(-750, _tunnels[i].transform.position.y, _tunnels[i].transform.position.z);
            }
        }
    }
}
