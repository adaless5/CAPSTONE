using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultRefillStation : MonoBehaviour
{
    public int m_amountOfClipsInPickup = 1;
    public int m_clipSize = 100;
    protected int m_ammoCap = 0;

    public AmmoController m_ammoController;

    void Awake()
    {
        m_ammoController = FindObjectOfType<AmmoController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (GameObject.FindObjectOfType<ALTPlayerController>().CheckForInteract())
        {
            bool isMissingAmmo = false;
            if (m_ammoController == null)
            {

                m_ammoController = FindObjectOfType<AmmoController>();
            }
            if (other.tag == "Player" && m_ammoController.IsAmmoFull(WeaponType.BaseWeapon) == false)
            {
                if (GameObject.FindObjectOfType<WeaponBase>().bIsObtained)
                {
                    EventBroker.CallOnAmmoPickup(WeaponType.BaseWeapon, m_amountOfClipsInPickup, m_ammoCap);
                    isMissingAmmo = true;
                }
            }

            if (other.tag == "Player" && m_ammoController.IsAmmoFull(WeaponType.CreatureWeapon) == false)
            {
                if (GameObject.FindObjectOfType<CreatureWeapon>().bIsObtained)
                {
                    EventBroker.CallOnAmmoPickup(WeaponType.CreatureWeapon, m_amountOfClipsInPickup, m_ammoCap);
                    isMissingAmmo = true;
                }
            }
            if (other.tag == "Player" && m_ammoController.IsAmmoFull(WeaponType.GrenadeWeapon) == false)
            {
                if (GameObject.FindObjectOfType<MineSpawner>().bIsObtained)
                {
                    EventBroker.CallOnAmmoPickup(WeaponType.GrenadeWeapon, m_amountOfClipsInPickup, m_ammoCap);
                    isMissingAmmo = true;
                }
            }
            if (isMissingAmmo)
            {
                EventBroker.CallOnAmmoPickupAttempt();
            }
        }
    }
}
