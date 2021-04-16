using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultRefillStation : MonoBehaviour
{
    public GameObject Full;
    public GameObject NotFull;
    public GameObject FillUsed;

    float useTimer;
    public int m_amountOfClipsInPickup = 1;
    public int m_clipSize = 100;
    protected int m_ammoCap = 0;

    public AmmoController m_ammoController;

    void Awake()
    {
        m_ammoController = FindObjectOfType<AmmoController>();
        Color newCol = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        FillUsed.GetComponent<MeshRenderer>().material.color = newCol;
    }

    bool IsEverythingFull()
    {
        bool itsAllOn = true;
        if (m_ammoController != null)
        {
            Health healthcomp = m_ammoController.GetComponentInChildren<Health>();
            if (healthcomp != null)
            {
                if (healthcomp.IsAtFullHealth() == false)
                {
                    itsAllOn = false;
                }
            }
            if (m_ammoController.IsAmmoFull(WeaponType.BaseWeapon) == false && GameObject.FindObjectOfType<WeaponBase>().bIsObtained)
            {
                itsAllOn = false;
            }
            if (m_ammoController.IsAmmoFull(WeaponType.BaseWeapon) == false && GameObject.FindObjectOfType<WeaponBase>().bIsObtained)
            {
                itsAllOn = false;
            }
            if (m_ammoController.IsAmmoFull(WeaponType.BaseWeapon) == false && GameObject.FindObjectOfType<WeaponBase>().bIsObtained)
            {
                itsAllOn = false;
            }
        }
        else
        {
            m_ammoController = FindObjectOfType<AmmoController>();
        }

        return itsAllOn;
    }
    private void Update()
    {
        if (IsEverythingFull())
        {
            NotFull.gameObject.SetActive(false);
            Full.gameObject.SetActive(true);
        }
        else
        {
            NotFull.gameObject.SetActive(true);
            Full.gameObject.SetActive(false);
        }

        if (useTimer > 0.0f)
        {
            useTimer -= Time.deltaTime;
        }
        else if (FillUsed.GetComponent<MeshRenderer>().material.color.a > 0.0f)
        {
            float alpha = FillUsed.GetComponent<MeshRenderer>().material.color.a;
            alpha -= Time.deltaTime;
            if (alpha < 0)
            { alpha = 0; }
            FillUsed.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, alpha);
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject.FindObjectOfType<InteractableText>().b_inInteractCollider = true;
            if (GameObject.FindObjectOfType<ALTPlayerController>().CheckForInteract())
            {

                Health playerHP = other.GetComponent<Health>();
                if (playerHP)
                {
                    if (!playerHP.IsAtFullHealth())
                    {
                        playerHP.Heal(100.0f);
                        EventBroker.CallOnHealthPickupAttempt(playerHP.IsAtFullHealth());

                    }
                }
                if (m_ammoController == null)
                {

                    m_ammoController = FindObjectOfType<AmmoController>();
                }
                if (m_ammoController.IsAmmoFull(WeaponType.BaseWeapon) == false)
                {
                    if (GameObject.FindObjectOfType<WeaponBase>().bIsObtained)
                    {
                        EventBroker.CallOnAmmoPickup(WeaponType.BaseWeapon, m_amountOfClipsInPickup, m_ammoCap);
                        EventBroker.CallOnAmmoPickupAttempt();
                    }
                }

                if (m_ammoController.IsAmmoFull(WeaponType.CreatureWeapon) == false)
                {
                    if (GameObject.FindObjectOfType<CreatureWeapon>().bIsObtained)
                    {
                        EventBroker.CallOnAmmoPickup(WeaponType.CreatureWeapon, m_amountOfClipsInPickup, m_ammoCap);

                    }
                }
                if (m_ammoController.IsAmmoFull(WeaponType.GrenadeWeapon) == false)
                {
                    if (GameObject.FindObjectOfType<MineSpawner>().bIsObtained)
                    {
                        EventBroker.CallOnAmmoPickup(WeaponType.GrenadeWeapon, m_amountOfClipsInPickup, m_ammoCap);

                    }
                }

                Color newCol = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                FillUsed.GetComponent<MeshRenderer>().material.color = newCol;
                useTimer = 2.5f;

            }
        }
    }
}
