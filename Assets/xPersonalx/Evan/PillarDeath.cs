using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarDeath : MonoBehaviour
{
    // Start is called before the first frame update
    bool _showTime;
    float _finalCountdown;
    public ParticleSystem[] _deathEffects;
    public ParticleSystem deathPop;
    public FinalPowerPillar _finalPillar;
    void Start()
    {
        
    }

    void FinalCountdown()
    {
        if(_showTime && _finalCountdown > 0)
        {
          _finalCountdown -= Time.deltaTime;
            for (int i = 0; i < _deathEffects.Length; i++)
            { _deathEffects[i].transform.position = _finalPillar.transform.position; }
            //transform.position = _finalPillar.gameObject.transform.position; 
            if (_finalCountdown <= 0)
            {
                _finalPillar.gameObject.SetActive(false);
                deathPop.transform.position = _finalPillar.transform.position;
                deathPop.Play();
            }
        }

    }
    private void Awake()
    {
        _finalCountdown = 8;
    }
    public void Die()
    {
        _showTime = true;
        for (int i = 0; i < _deathEffects.Length; i++)
        { _deathEffects[i].Play(); }
    }
    // Update is called once per frame
    void Update()
    {
        FinalCountdown();
    }
}
