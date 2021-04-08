using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Animations : MonoBehaviour
{
    public ALTPlayerController _playerRef;
    public Animator _weaponAnimator;
    float _reloadAnimClipLength;
    float _playerSpeed;
    bool bHasFired;
    bool bIsSprinting;

    public void Initialize(Animator weaponAnimator, float reloadAnimClipLength)
    {
        EventBroker.OnPlayerSpawned += PlayerSpawned;
        EventBroker.OnWeaponSwap += WeaponSwapOut;
        _weaponAnimator = weaponAnimator;
        _reloadAnimClipLength = reloadAnimClipLength;
    }

    void Awake()
    {
        bHasFired = false;
        bIsSprinting = false;
    }

    // Update is called once per frame
    void Update()
    {
        _playerSpeed = _playerRef.GetMovementSpeed();
        bHasFired = _playerRef.CheckForUseWeaponInput();
        bIsSprinting = _playerRef.CheckForSprintInput();

        _weaponAnimator.SetFloat("WalkSpeed", _playerSpeed);

        if (_playerRef.m_stamina.GetCurrentStamina() > 0)
        {
            _weaponAnimator.SetBool("IsSprinting", bIsSprinting);
        }
        else if (_playerRef.m_stamina.GetCurrentStamina() <= 0)
        {
            _weaponAnimator.SetBool("IsSprinting", false);
        }
    }
    public void TriggerReloadAnimation()
    {
        _weaponAnimator.SetTrigger("OnReload");
    }

    //Sets a speed multiplier for both firing and reloading animations
    //Had to set animation clip length manually since can't get through code unless state is active
    //Please avoid touching animation length in editor, instead use the fire rate and reload rate, the rate is a delay i.e a reload rate of 2f, the gun takes 2 seconds to reload
    public void SetReloadAnimationSpeed(float reloadRate)
    {
        float rate = _reloadAnimClipLength / reloadRate;
        _weaponAnimator.SetFloat("ReloadRate", rate);
        Debug.Log("Reload Rate is " + rate);
    }

    //Action Functions
    protected void PlayerSpawned(GameObject player)
    {
        _playerRef = player.GetComponent<ALTPlayerController>();

    }

    public void WeaponSwapOut()
    {
        _weaponAnimator.SetTrigger("IsDisabled");
    }
}
