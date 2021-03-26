using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportingEnemyAnimation : MonoBehaviour
{
    public bool _PlayerSpotted;
    public GameObject[] _Arms;
    public GameObject[] _SpinningPieces;
    public GameObject _RotatingJet;
    public Vector2 _ArmSpeedRange;
    
    public float _FastArmSpeed;
    public float _FastBodySpeed;
    public float _SlowArmSpeed;
    public float _SlowBodySpeed;


    List<float> _ArmSpeeds = new List<float>();
    List<bool> _ArmUpDown = new List<bool>();
    List<float> _ArmAngles = new List<float>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject arm in _Arms)
        {
            _ArmSpeeds.Add(Random.Range(_ArmSpeedRange.x, _ArmSpeedRange.y));
            bool upDown = Random.value > 0.5f;
            _ArmUpDown.Add(upDown);
            _ArmAngles.Add(0.0f);
        }
    }
    void ArmMovement(float speed)
    {
        //this could all probably be improved using quaternions, but I still need to learn those :P
        //cycle through each arm, check its coresponding boolean to see whether it is moving up or down, rotate accordingly and flip bool if it is outside of rotation range
        for (int i = 0; i < _Arms.Length; i++)
        {
            if (_ArmUpDown[i])
            {
                    _ArmAngles[i] += Time.deltaTime * (speed + _ArmSpeeds[i]);
                if (_ArmAngles[i] < 45.0f)
                {
                    _Arms[i].transform.Rotate(new Vector3(0.0f, 0.0f, Time.deltaTime * (speed + _ArmSpeeds[i])));
                }
                else
                {
                    _ArmUpDown[i] = false;
                }
            }
            else
            {
                    _ArmAngles[i] -= Time.deltaTime * (speed + _ArmSpeeds[i]);
                if (_ArmAngles[i] > -45.0f)
                {
                    _Arms[i].transform.Rotate(new Vector3(0.0f, 0.0f, -Time.deltaTime * (speed + _ArmSpeeds[i])));
                }
                else
                {
                    _ArmUpDown[i] = true;
                }
            }
        }
    }
    void BodySpin(float speed)
    {
        for (int i = 0; i < _SpinningPieces.Length; i++)
        {
            if(i!=1)
            _SpinningPieces[i].transform.Rotate(new Vector3(0.0f,0.0f,Time.deltaTime * (speed * (i + 1))));
            else
                _SpinningPieces[i].transform.Rotate(new Vector3(0.0f, 0.0f, -Time.deltaTime * (speed * (i + 1))));
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (_PlayerSpotted)
        {
            ArmMovement(_FastArmSpeed);
            BodySpin(_FastBodySpeed);
        }
        else
        {
            ArmMovement(_SlowArmSpeed);
            BodySpin(_SlowBodySpeed);
        }
        if(_RotatingJet!=null)
        {
            _RotatingJet.transform.eulerAngles = new Vector3(-90,0,0);
        }
    }
}
