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
    public GrenadeExplosion _explosion;
    public ParticleSystem _explosion2;

    int _activeIndex;
    bool _lightsOut;

    float _startTime;

    // Start is called before the first frame update
    void Start()
    {
        _startTime = 12.0f;
    }
    private void Awake()
    {
        _currentTimeUntilNextCategory = _timeUntilNextCategory;
        _player = GameObject.FindObjectOfType<ALTPlayerController>();
        DisablePlayerLights();
        _explosion.bDontDie = true; _explosion2.Stop();
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

    void SetWeapon()
    {
        if (GameObject.FindObjectOfType<WeaponBase>() != null)
        {
            GameObject.FindObjectOfType<WeaponBase>().bIsActive = true;
            GameObject.FindObjectOfType<WeaponBase>().bIsObtained = true;
        }

        if (GameObject.FindObjectOfType<CreatureWeapon>() != null)
        { GameObject.FindObjectOfType<CreatureWeapon>().bIsActive = false; }

        if (GameObject.FindObjectOfType<MineSpawner>() != null)
        { GameObject.FindObjectOfType<MineSpawner>().bIsActive = false; }

        if (GameObject.FindObjectOfType<ThermalEquipment>() != null)
        { GameObject.FindObjectOfType<ThermalEquipment>().bIsActive = false; }

        if (GameObject.FindObjectOfType<Blade>() != null)
        { GameObject.FindObjectOfType<Blade>().bIsActive = false; }

        if (GameObject.FindObjectOfType<GrappleHook>() != null)
        { GameObject.FindObjectOfType<GrappleHook>().bIsActive = false; }

        if (GameObject.FindObjectOfType<ALTPlayerController>() != null)
        { GameObject.FindObjectOfType<ALTPlayerController>()._bIsCredits = true; }

        //  if (GameObject.Find("Weapon_Wheel_Canvas") != null)
        //  { GameObject.Find("Weapon_Wheel_Canvas").SetActive(false); }
        //
        //  if (GameObject.Find("Tool_Wheel_Canvas") != null)
        //  { GameObject.Find("Tool_Wheel_Canvas").SetActive(false); }

        // if (GameObject.FindObjectOfType<HUD>() != null)
        // { GameObject.FindObjectOfType<HUD>().gameObject.SetActive(false); }
    }
    // Update is called once per frame
    void Update()
    {
        SetWeapon();
        if (!_lightsOut)
        {
            DisablePlayerLights();
        }
        if (_startTime < 0)
        {
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
            if (GameObject.FindObjectOfType<AmmoController>() != null)
                if (!GameObject.FindObjectOfType<AmmoController>().IsAmmoFull(WeaponType.BaseWeapon))
                {
                    EventBroker.CallOnAmmoPickup(WeaponType.BaseWeapon, 1, 40);
                }
        }
        else
        {
            _startTime -= Time.deltaTime;

            if (_startTime < 0)
            {
                _creditLists[0].Activate();
            }
            if (_startTime < 5 && _startTime > 4)
            {
                _explosion.bDontDie = false;
                if (!_explosion2.isPlaying)
                {
                    _explosion2.Play();
                }
            }
        }

    }
}
