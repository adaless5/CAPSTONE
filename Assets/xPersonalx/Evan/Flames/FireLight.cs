using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLight : MonoBehaviour
{
    // Start is called before the first frame update
    float _flickerTime;
    public Light _light;
    float _intensity;
    public float _additive;
    void Start()
    {
        _intensity = _light.intensity;
    }

    void Flicker()
    {
        if(_flickerTime <= 0)
        {
            _flickerTime = Random.Range(0.1f,1);
        }
        else
        {
            _flickerTime -= Time.deltaTime;
            _light.intensity = _intensity + (Mathf.Sin(_flickerTime*50) * _additive);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(_intensity < 0.1)
        {
            _intensity = _light.intensity;
        }
        else
        Flicker();
     
    }
}
