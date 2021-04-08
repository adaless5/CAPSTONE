using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Gland Gun animations logic
public class GG_Animations : MonoBehaviour
{
    public ALTPlayerController _player;
    public Animator _glandGunAnimator;

    float _fireAnimClipLength = 0.567f;
    float _reloadAnimClipLength = 2.2f;   

    private void Awake()
    {
        _glandGunAnimator = GetComponentInChildren<Animator>();

        EventBroker.OnPlayerSpawned += OnPlayerLoaded;
        EventBroker.OnWeaponSwap += WeaponSwapOut;
    }

    void OnPlayerLoaded(GameObject player)
    {
        _player = player.GetComponent<ALTPlayerController>();
       // _weapon = GetComponent<WeaponBase>();
    }

    // Update is called once per frame
    void Update()
    {
        float _playerSpeed = _player.GetMovementSpeed();
        bool bHasFired = _player.CheckForUseWeaponInput();
        bool bIsSprinting = _player.CheckForSprintInput();

        _glandGunAnimator.SetFloat("WalkSpeed", _playerSpeed);

        if (_player.m_stamina.GetCurrentStamina() > 0)
        {
            _glandGunAnimator.SetBool("IsSprinting", bIsSprinting);
        }
        else if (_player.m_stamina.GetCurrentStamina() <= 0)
        {
            _glandGunAnimator.SetBool("IsSprinting", false);
        }
    }

    public void WeaponSwapOut()
    {
        _glandGunAnimator.SetTrigger("IsDisabled");
    }

    public void TriggerReloadAnimation()
    {
        _glandGunAnimator.SetTrigger("OnReload");
    }

    //Sets a speed multiplier for both firing and reloading animations
    //Had to set animation clip length manually since can't get through code unless state is active
    //Please avoid touching animation length in editor, instead use the fire rate and reload rate, the rate is a delay i.e a reload rate of 2f, the gun takes 2 seconds to reload
    public void SetReloadAnimationSpeed(float reloadRate)
    {
        float rate = _reloadAnimClipLength / reloadRate;
        _glandGunAnimator.SetFloat("ReloadRate", rate);
        Debug.Log("Reload Rate is " + rate);
    }
}
