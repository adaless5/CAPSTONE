using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using TMPro;

public class AmmoUI : MonoBehaviour
{    
    public int m_overallAmmo;
    public int m_currentAmmo;
    public int m_clipSize;   
    Weapon m_baseGun;
    Weapon m_creatureWeapon;
    Weapon m_grenade;  
  
    AmmoController m_ammo;  
    public TMP_Text m_ammoText;

    // Start is called before the first frame update
    void Awake()
    {            
        m_ammoText = GetComponentInChildren<TextMeshProUGUI>();
        m_baseGun = FindObjectOfType<WeaponBase>();
        m_creatureWeapon = FindObjectOfType<CreatureWeapon>();
        m_grenade = FindObjectOfType<MineSpawner>();
        m_ammo = FindObjectOfType<AmmoController>();
        //m_ammoText.fontSize = 40f;
        //m_ammoText.transform.position = new Vector2(250, 100);
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
        //m_ammoText.SetText("<size=100%>Current Ammo:</i> {0:2} \n<size=60%>Overall Ammo: {1} \nClipSize: {2}", currentAmmo, overallAmmo, clipSize);
        m_ammoText.text = "<space=0.2em><voffset=0.8em>" + m_currentAmmo.ToString("D2") + "<voffset=0.2em><size=115%><space=-0.15em>/</voffset><space=-0.1em><size=75%>" + m_clipSize.ToString("D2") + "\n<voffset=-0.1em>+" + m_overallAmmo.ToString("D4");
        //choice 2 settings <space=0.1em><voffset=0.75em>06<voffset=0.2em><size=125%><space=-0.275em>/</voffset><space=-0.275em><size=75%>06   +0006
    }

}
