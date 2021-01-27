using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AmmoController : MonoBehaviour
{
    Weapon weaponType;

    public enum AmmoTypes : int
    {
        Default,
        Explosive,
        Creature,
        MaxAmmoTypes
    };

    int m_ammoType; 

    protected List<int> currentAmmoList = new List<int>();
    protected List<int> overstockAmmoList = new List<int>();
    protected List<int> weaponClipList = new List<int>();

    private void Awake()
    {
        EventBroker.OnAmmoPickup += AmmoPickup;      
        for (int i = 0; i < (int)AmmoTypes.MaxAmmoTypes; i++)
        {
            currentAmmoList.Add(0);
            overstockAmmoList.Add(0);
            weaponClipList.Add(0);
        }
        weaponType = FindObjectOfType<Weapon>();         
    }

    // Start is called before the first frame update
    void Start()
    {
    }
       

    public void InitializeAmmo(AmmoTypes ammoType, int clipSize, int OverstockAmmo)
    {
        m_ammoType = (int)ammoType;     
        weaponClipList[m_ammoType] = clipSize;
        currentAmmoList[m_ammoType] = weaponClipList[m_ammoType];
        overstockAmmoList[m_ammoType] = OverstockAmmo;
    }
  

    public bool CanReload()
    {
        if (overstockAmmoList[m_ammoType] >= 1 && currentAmmoList[m_ammoType] < weaponClipList[m_ammoType])
        {
            return true;
        }
        else
            return false;
    }

    public bool NeedsReload()
    {
        if (currentAmmoList[m_ammoType] == 0 && overstockAmmoList[m_ammoType] >= 1)
            return true;
        else
            return false;
    }

    public bool CanUseAmmo()
    {
        if (currentAmmoList[m_ammoType] > 0)
            return true;
        else
            return false;
    }

    public void Reload()
    {
        while (currentAmmoList[m_ammoType] < weaponClipList[m_ammoType] && overstockAmmoList[m_ammoType] > 0)
        {
            currentAmmoList[m_ammoType]++;
            overstockAmmoList[m_ammoType]--;
        }
    }

    public void AmmoPickup(WeaponType type, int numberOfClips)
    {
        int weaponType = (int)type;
        overstockAmmoList[weaponType] += (weaponClipList[weaponType] * numberOfClips);
    }

    public void UseAmmo()
    {
        currentAmmoList[m_ammoType]--;
    }

    public bool OutOfAmmo()
    {
        if (currentAmmoList[m_ammoType] == 0 && overstockAmmoList[m_ammoType] == 0)
        {
            return true;
        }
        else
            return false;
    }

    public int GetCurrentAmmo()
    {
        return currentAmmoList[m_ammoType];
    }

    public int GetOverallAmmo()
    {
        return overstockAmmoList[m_ammoType];
    }

    public int GetClipSize()
    {
        return weaponClipList[m_ammoType];
    }

    public void SetAmmoType(int type)
    {
        m_ammoType = type;
    }

}
