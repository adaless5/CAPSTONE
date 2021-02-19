using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AmmoController : MonoBehaviour, ISaveable
{    
    public enum AmmoTypes : int
    {
        Default,
        Explosive,
        Creature,
        MaxAmmoTypes
    };

    int m_ammoType; 
    public Animator outOfAmmoAnimator;
    protected List<int> currentAmmoList = new List<int>();
    protected List<int> overstockAmmoList = new List<int>();
    protected List<int> weaponClipList = new List<int>();

    private void Awake()
    {
        outOfAmmoAnimator = FindObjectOfType<AmmoUI>().GetComponent<Animator>();
        //SceneManager.sceneUnloaded += ctx => SaveAmmo();        

        LoadDataOnSceneEnter();
        EventBroker.OnAmmoPickup += AmmoPickup;      
        for (int i = 0; i < (int)AmmoTypes.MaxAmmoTypes; i++)
        {
            currentAmmoList.Add(0);
            overstockAmmoList.Add(0);
            weaponClipList.Add(0);
        }        
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
        outOfAmmoAnimator.SetBool("bIsOut", false);
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
        if(CanUseAmmo())
        {
            currentAmmoList[m_ammoType]--;
            if(OutOfAmmo())
            outOfAmmoAnimator.SetBool("bIsOut", true);
        }
        else
        {
            outOfAmmoAnimator.SetBool("bIsOut", true);
        }
    }

    public bool OutOfAmmo()
    {
        if (currentAmmoList[m_ammoType] == 0 && overstockAmmoList[m_ammoType] == 0)
        {
            return true;
        }
        else
            outOfAmmoAnimator.SetBool("bIsOut", false);
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
        if(OutOfAmmo())
        {
            outOfAmmoAnimator.SetBool("bIsOut", true);
        }
        else
        {
            outOfAmmoAnimator.SetBool("bIsOut", false);
        }
    }

    public void SaveAmmo()
    {
        for(int i = 0; i < (int)AmmoTypes.MaxAmmoTypes; i++)
        {
            SaveSystem.Save(gameObject.name, currentAmmoList[i].ToString(), gameObject.scene.name, currentAmmoList[i]);
            SaveSystem.Save(gameObject.name, weaponClipList[i].ToString(), gameObject.scene.name, weaponClipList[i]);
            SaveSystem.Save(gameObject.name, overstockAmmoList[i].ToString(), gameObject.scene.name, overstockAmmoList[i]);
        }
    }
    
    public void LoadDataOnSceneEnter()
    {
        for (int i = 0; i < (int)AmmoTypes.MaxAmmoTypes; i++)
        {
            if(currentAmmoList.Count > 0)
            currentAmmoList[i] = SaveSystem.LoadInt(gameObject.name, currentAmmoList[i].ToString(), gameObject.scene.name);
            if(weaponClipList.Count > 0)
            weaponClipList[i] = SaveSystem.LoadInt(gameObject.name, weaponClipList[i].ToString(), gameObject.scene.name);  
            if(overstockAmmoList.Count > 0)
            overstockAmmoList[i] = SaveSystem.LoadInt(gameObject.name, overstockAmmoList[i].ToString(), gameObject.scene.name);
        }
    }    
}
