using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsSequenceManager : MonoBehaviour
{
    ALTPlayerController _player;

    public float _timeUntilNextCategory;
    float _currentTimeUntilNextCategory;

    public float _creditMovementSpeed;

    public CreditCategory[] _creditLists;

    int _activeIndex;
    bool _lightsOut;


    // Start is called before the first frame update
    void Start()
    {

    }
    private void Awake()
    {
        _currentTimeUntilNextCategory = _timeUntilNextCategory;
        _player = GameObject.FindObjectOfType<ALTPlayerController>();
        DisablePlayerLights();
        _creditLists[0].Activate();
    }

    void DisablePlayerLights()
    {
        GameObject lightSky = GameObject.Find("LightSkyFog");
        if (lightSky != null)
        {
            lightSky.SetActive(false);
            _lightsOut = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!_lightsOut)
        {
            DisablePlayerLights();
        }
        if (_activeIndex < _creditLists.Length && _creditLists[_activeIndex]._allNamesFaded)
        {
            if (_currentTimeUntilNextCategory > 0)
            {
                _currentTimeUntilNextCategory -= Time.deltaTime;
            }
            else
            {
                _activeIndex++;
                if (_activeIndex < _creditLists.Length)
                {
                    _creditLists[_activeIndex].Activate();
                    _currentTimeUntilNextCategory = _timeUntilNextCategory;
                }
            }
        }
        if(GameObject.FindObjectOfType<AmmoController>() != null)
        if(!GameObject.FindObjectOfType<AmmoController>().IsAmmoFull(WeaponType.BaseWeapon))
        {
            EventBroker.CallOnAmmoPickup(WeaponType.BaseWeapon, 1, 40);
        }
    }
}
