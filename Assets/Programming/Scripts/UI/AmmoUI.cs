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
    Text m_text;

    // Start is called before the first frame update
    void Awake()
    {
        //m_player = FindObjectOfType<ALTPlayerController>();
        m_text = GetComponent<Text>();
        m_baseGun = FindObjectOfType<WeaponBase>();
        m_creatureWeapon = FindObjectOfType<CreatureWeapon>();
        //m_currentAmmo = weaponType.GetCurrentAmmo();
        //weaponType =  m_player._weaponBelt.GetToolAtIndex(0);
        
    }

    private void Update()
    {    
        if(m_baseGun.bIsActive)
        {
            SetBaseGunInfo();
            SetAmmoText(m_currentAmmo, m_overallAmmo, m_clipSize);
        }
        else if(m_creatureWeapon.bIsActive)
        {
            SetCreatureWeaponInfo();
            SetAmmoText(m_currentAmmo, m_overallAmmo, m_clipSize);
        }
        else
        {
            SetAmmoText(0, 0, 0);
        }
    }

    private void SetBaseGunInfo()
    {
        m_currentAmmo = m_baseGun.GetCurrentAmmo();
        m_overallAmmo = m_baseGun.GetOverallAmmo();
        m_clipSize = m_baseGun.GetClipSize();
    }

    private void SetCreatureWeaponInfo()
    {
        m_currentAmmo = m_creatureWeapon.GetCurrentAmmo();
        m_overallAmmo = m_creatureWeapon.GetOverallAmmo();
        m_clipSize = m_creatureWeapon.GetClipSize();
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
