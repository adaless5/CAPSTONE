using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DG_Animations : MonoBehaviour
{
    public ALTPlayerController _player;
    public Animator _defaultGunAnimator;
    public WeaponBase _weapon;

    float _fireAnimClipLength = 0.567f;
    float _reloadAnimClipLength = 2.2f;

    // Start is called before the first frame update
    void Start()
    {
        _defaultGunAnimator = GetComponentInChildren<Animator>();
    }

    private void Awake()
    {
        
        EventBroker.OnPlayerSpawned += OnPlayerLoaded;
        EventBroker.OnWeaponSwap += WeaponSwapOut;
    }

    void OnPlayerLoaded(GameObject player)
    {
        _player = player.GetComponent<ALTPlayerController>();
        _weapon = GetComponent<WeaponBase>();
    }


    // Update is called once per frame
    void Update()
    {
        float _playerSpeed = _player.GetMovementSpeed();      
        bool bHasFired = _player.CheckForUseWeaponInput();
        bool bIsSprinting = _player.CheckForSprintInput();
      
        _defaultGunAnimator.SetFloat("WalkSpeed", _playerSpeed);

        if(_player.m_stamina.GetCurrentStamina() > 0)
        _defaultGunAnimator.SetBool("IsSprinting", bIsSprinting);       
    }

    public void WeaponSwapOut()
    {
        _defaultGunAnimator.SetTrigger("IsDisabled");
    }

    public void TriggerReloadAnimation()
    {
        _defaultGunAnimator.SetTrigger("OnReload");
    }


    //Sets a speed multiplier for both firing and reloading animations
    //Had to set animation clip length manually since can't get through code unless state is active
    //Please avoid touching animation length in editor, instead use the fire rate and reload rate, the rate is a delay i.e a reload rate of 2f, the gun takes 2 seconds to reload
    //public void SetFireAnimationSpeed(float fireRate)
    //{
    //    float rate = _fireAnimClipLength / fireRate;
    //    _defaultGunAnimator.SetFloat("FireRate", 1);
    //    Debug.Log("Fire Rate is " + rate);
    //}

    public void SetReloadAnimationSpeed(float reloadRate)
    {
        float rate = _reloadAnimClipLength / reloadRate;
        _defaultGunAnimator.SetFloat("ReloadRate", rate);
        Debug.Log("Reload Rate is " + rate);
    }    
       
}
