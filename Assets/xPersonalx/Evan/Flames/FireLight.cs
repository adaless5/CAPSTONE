using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLight : MonoBehaviour
{
    // Start is called before the first frame update
    float _time;
    float _time2;
    public Light _light;
    float _intensity;
    public float _additive;
    void Start()
    {
        _intensity = _light.intensity;
    }


    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if (_time>=360)
        {
            _time = 0;
        }
        _light.intensity = _intensity + (Mathf.Sin(_time) * _additive);
    }
}
