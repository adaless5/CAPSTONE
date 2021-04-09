using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureProjectileAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    float _pulseTime;
    Vector3 _firingOrganSize;
    Vector3 _currentScale;
    public GameObject _globObject;
    public float _pulseScale;
    public bool bGrow;
    public bool dontRotate;
    void Awake()
    {
        _firingOrganSize = transform.localScale;
        _pulseTime = Random.Range(0.0f, 360.0f);
        if (!dontRotate)
        {
            Vector3 RandomRot = new Vector3(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180));
            _globObject.transform.Rotate(RandomRot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _pulseTime += Time.deltaTime * 3;
        if (!dontRotate)
        { _globObject.transform.Rotate(Time.deltaTime * 10, Time.deltaTime * 20, Time.deltaTime * 30); }
        _currentScale = new Vector3(Mathf.Sin(_pulseTime) * 0.1f * _pulseScale, Mathf.Cos(_pulseTime) * 0.1f * _pulseScale, Mathf.Cos(_pulseTime) * 0.1f * _pulseScale);
        transform.localScale = _firingOrganSize + _currentScale;
        if (_pulseTime > 360)
        {
            _pulseTime = 0.0f;
        }

        if (bGrow)
        {
            _pulseScale += Time.deltaTime;
        }
    }
}
