using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{    
    public int m_overallAmmo;
    public int m_currentAmmo;
    public int m_clipSize;   
    Weapon m_baseGun;
    Weapon m_creatureWeapon;
    Weapon m_grenade;

    AmmoController m_ammo;
    Text m_text;

    // Start is called before the first frame update
    void Awake()
    {
        m_text = GetComponent<Text>();
        m_baseGun = FindObjectOfType<WeaponBase>();
        m_creatureWeapon = FindObjectOfType<CreatureWeapon>();
        m_grenade = FindObjectOfType<MineSpawner>();
        m_ammo = FindObjectOfType<AmmoController>();     
    }

    private void Update()
    {    
        if(m_baseGun.bIsActive)
        {           
            SetBaseGunInfo();           
        }
        else if(m_creatureWeapon.bIsActive)
        {
            SetCreatureWeaponInfo();           
        }
        else if(m_grenade.bIsActive)
        {
            SetGrenadeInfo();           
        }
        else
        {
            SetAmmoText(0, 0, 0);
        }         
    }

    private void SetBaseGunInfo()
    {
        m_ammo.SetAmmoType((int)WeaponType.BaseWeapon);
        GetAmmoInfo();
        SetAmmoText(m_currentAmmo, m_overallAmmo, m_clipSize);
    }

    private void SetCreatureWeaponInfo()
    {
        m_ammo.SetAmmoType((int)WeaponType.CreatureWeapon);
        GetAmmoInfo();
        SetAmmoText(m_currentAmmo, m_overallAmmo, m_clipSize);
    }

    private void SetGrenadeInfo()
    {
        m_ammo.SetAmmoType((int)WeaponType.GrenadeWeapon);
        GetAmmoInfo();
        SetAmmoText(m_currentAmmo, m_overallAmmo, m_clipSize);
    }

    private void GetAmmoInfo()
    {
        m_currentAmmo = m_ammo.GetCurrentAmmo();
        m_overallAmmo = m_ammo.GetOverallAmmo();
        m_clipSize = m_ammo.GetClipSize();
    }

    public void SetAmmoText(int currentAmmo, int overallAmmo, int clipSize)
    {
        if (m_text != null)
        {
            if(clipSize < 10 && overallAmmo < 10)
            {                
                m_text.text = "0" + currentAmmo + "/0" + clipSize + "\n+000" + overallAmmo;
            }
            else if(clipSize < 10 && overallAmmo < 100)
            {
                m_text.text = "0" + currentAmmo + "/0" + clipSize + "\n+00" + overallAmmo;
            }
            else if(clipSize > 10 && overallAmmo < 10)
            {
                m_text.text = "" + currentAmmo + "/" + clipSize + "\n+000" + overallAmmo;
            }
            else if(clipSize < 10 && overallAmmo > 100)
            {
                m_text.text = "0" + currentAmmo + "/0" + clipSize + "\n+0" + overallAmmo;
            }
            else
            {
                m_text.text = "" + currentAmmo + "/" + clipSize + "\n+00" + overallAmmo;
            }
        }
    }

}
