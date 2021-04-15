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
    int newAmmoAddedTotal;
    public Animator outOfAmmoAnimator;
    protected List<int> currentAmmoList = new List<int>();
    protected List<int> overstockAmmoList = new List<int>();
    protected List<int> weaponClipList = new List<int>();
    protected List<int> ammoCapList = new List<int>();
    protected List<bool> ammoFullList = new List<bool>();



    private void Awake()
    {
        outOfAmmoAnimator = FindObjectOfType<AmmoUI>().GetComponent<Animator>();

        LoadDataOnSceneEnter();
        EventBroker.OnAmmoPickup += AmmoPickup;
        for (int i = 0; i < (int)AmmoTypes.MaxAmmoTypes; i++)
        {
            currentAmmoList.Add(0);
            overstockAmmoList.Add(0);
            weaponClipList.Add(0);
            ammoCapList.Add(0);
            ammoFullList.Add(false);
        }
        SceneManager.sceneLoaded += UpdateAmmoData;
    }

    void UpdateAmmoData(Scene scene, LoadSceneMode scenemode)
    {
        SaveAmmo();
        if (scene.name == "Loading_Scene")
            LoadDataOnSceneEnter();
    }

    // Start is called before the first frame update
    void Start()
    {

    }


    public void InitializeAmmo(AmmoTypes ammoType, int clipSize, int OverstockAmmo, int ammoCap)
    {
        m_ammoType = (int)ammoType;
        weaponClipList[m_ammoType] = clipSize;
        currentAmmoList[m_ammoType] = weaponClipList[m_ammoType];
        overstockAmmoList[m_ammoType] = OverstockAmmo;
        ammoCapList[m_ammoType] = ammoCap;
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
            ammoFullList[m_ammoType] = false;
        }
    }

    public void AmmoPickup(WeaponType type, int numberOfClips, int ammoCap)
    {
        int weaponType = (int)type;
        if (!ammoFullList[weaponType])
        {
            int tempOverstock = overstockAmmoList[weaponType];
            //int ammoAddedTotal = (weaponClipList[weaponType] * numberOfClips) + overstockAmmoList[weaponType];
            overstockAmmoList[weaponType] += (weaponClipList[weaponType] * numberOfClips);
            if (overstockAmmoList[weaponType] > ammoCapList[weaponType])
            {
                newAmmoAddedTotal = ammoCapList[weaponType] - tempOverstock;
                overstockAmmoList[weaponType] = ammoCapList[weaponType];
                ammoFullList[weaponType] = true;
            }
        }
    }

    public void UseAmmo()
    {
        if (CanUseAmmo())
        {
            currentAmmoList[m_ammoType]--;
            if (OutOfAmmo())
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

    public int GetAmmoCap()
    {
        return ammoCapList[m_ammoType];
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

    public int GetAmmoAdded()
    {
        return newAmmoAddedTotal;
    }


    public bool IsAmmoFull(WeaponType ammoType)
    {
        int weaponType = (int)ammoType;
        return ammoFullList[weaponType];
    }

    public void SetAmmoType(int type)
    {
        m_ammoType = type;
        if (OutOfAmmo())
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
        for (int i = 0; i < (int)AmmoTypes.MaxAmmoTypes; i++)
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
            if (currentAmmoList.Count > 0)
                currentAmmoList[i] = SaveSystem.LoadInt(gameObject.name, currentAmmoList[i].ToString(), gameObject.scene.name);
            if (weaponClipList.Count > 0)
                weaponClipList[i] = SaveSystem.LoadInt(gameObject.name, weaponClipList[i].ToString(), gameObject.scene.name);
            if (overstockAmmoList.Count > 0)
                overstockAmmoList[i] = SaveSystem.LoadInt(gameObject.name, overstockAmmoList[i].ToString(), gameObject.scene.name);
        }
    }
}
