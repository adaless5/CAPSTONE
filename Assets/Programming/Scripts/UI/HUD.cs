using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void DamageHandler(int amount);

//this class will act as an event manager for all UI scripts - VR
public class HUD : MonoBehaviour
{
    public event DamageHandler TakeDamage;

    public HealthBarUI m_healthUI;
    public ArmorBarUI m_armorUI;
    public AmmoUI m_ammoUI;

    private ALTPlayerController m_player;

    //Weapon Icon
    public Image m_weaponIcon;
    public Sprite[] weaponIcons;

    //Ammo Icon
    public Image m_ammoIcon;
    public Sprite[] ammoIcons;

    //Tool Icon
    public Image m_toolIcon;
    public Sprite[] toolIcons;

    //HUD Overlay
    public Image m_HUDOverlay;

    //Pickup Notification Box
    private Image m_NotificationBox;
    private Text m_notificationText;
    private AmmoController m_ammoController;
    private Animator m_pickupAnimator;


    private void Awake()
    {
        m_healthUI = GetComponentInChildren<HealthBarUI>();
        m_armorUI = GetComponentInChildren<ArmorBarUI>();
        m_ammoUI = GetComponentInChildren<AmmoUI>();


        m_player = FindObjectOfType<ALTPlayerController>();
        m_ammoController = gameObject.transform.GetChild(1).GetComponentInChildren<AmmoController>();

        //Icons
        m_weaponIcon = gameObject.transform.GetChild(1).Find("Weapon_Icon").GetComponent<Image>();
        m_ammoIcon = gameObject.transform.GetChild(1).Find("Ammo_Icon").GetComponent<Image>();
        m_toolIcon = gameObject.transform.GetChild(1).Find("Tool_Icon").GetComponent<Image>();
        //HUD Overlay
        m_HUDOverlay = gameObject.transform.GetChild(1).GetComponent<Image>();


        //Pickup Notifications
        m_NotificationBox = gameObject.transform.GetChild(1).Find("Pickup Notification").GetComponent<Image>();       
        m_notificationText = gameObject.transform.GetChild(1).Find("Pickup Notification").GetComponentInChildren<Text>();
        m_pickupAnimator = gameObject.transform.GetChild(1).Find("Pickup Notification").GetComponent<Animator>();

        EventBroker.OnHealthPickup += NotificationText;
        EventBroker.OnAmmoPickup += NotificationText;
        EventBroker.OnAmmoPickupAttempt += NotificationText;
        EventBroker.OnHealthPickupAttempt += NotificationText;
        EventBroker.OnCurrencyPickup += NotificationText;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Weapon Icon
        weaponIcons = new Sprite[4];
        weaponIcons[0] = Resources.Load<Sprite>("Sprites/Icons/Icon_Empty");
        weaponIcons[1] = Resources.Load<Sprite>("Sprites/Icons/Icon_Gun");
        weaponIcons[2] = Resources.Load<Sprite>("Sprites/Icons/Icon_Grenade");
        weaponIcons[3] = Resources.Load<Sprite>("Sprites/Icons/Icon_Creature");

        //Ammo Icon
        //ammoIcons = new Sprite[4];
        //ammoIcons[0] = Resources.Load<Sprite>("Sprites/Icons/Icon_Empty");
        ammoIcons = new Sprite[3];
        ammoIcons[0] = Resources.Load<Sprite>("Sprites/Icons/Ammo/AMMO_DEFAULT");
        ammoIcons[1] = Resources.Load<Sprite>("Sprites/Icons/Ammo/AMMO_GRENADE");
        ammoIcons[2] = Resources.Load<Sprite>("Sprites/Icons/Ammo/AMMO_CREATURE");

        //Tool Icon
        toolIcons = new Sprite[3];
        toolIcons[0] = Resources.Load<Sprite>("Sprites/Icons/Icon_Empty");
        toolIcons[1] = Resources.Load<Sprite>("Sprites/Icons/Icon_Grapple");
        toolIcons[2] = Resources.Load<Sprite>("Sprites/Icons/Icon_Sword_HUD");

        //HUD Overlay        
        m_HUDOverlay.sprite = Resources.Load<Sprite>("Sprites/HUD/HUD_Overlay");      
    }

    // Update is called once per frame
    void Update()
    {
        CycleWeaponType(m_player._weaponBelt.selectedWeaponIndex);
        CycleEquipmentType(m_player._equipmentBelt.selectedWeaponIndex);
    }

    void CycleWeaponType(int index)
    {
        if (m_player._weaponBelt._items[index].GetComponentInChildren<Tool>().bIsActive)
        {
            m_weaponIcon.sprite = weaponIcons[index + 1];
            //m_ammoIcon.sprite = ammoIcons[index + 1];
        }
        else
        {
            m_weaponIcon.sprite = weaponIcons[index];
        }
        m_ammoIcon.sprite = ammoIcons[index];
    }

    void CycleEquipmentType(int index)
    {
        //Displays nothing until equipment is equipped
        if (m_player._equipmentBelt._items[index].GetComponentInChildren<Tool>().bIsActive)
        {
            m_toolIcon.sprite = toolIcons[index + 1];
        }
        else
        {
            m_toolIcon.sprite = toolIcons[index];
        }
    }

    void NotificationText()
    {
        m_pickupAnimator.SetTrigger("IsPicked");       
        m_notificationText.text = "MAX AMMO REACHED";
    }

    void NotificationText(bool healthFull) 
    {
        m_pickupAnimator.SetTrigger("IsPicked");
        m_notificationText.text = "MAX HEALTH REACHED";
    }

    void NotificationText(float healAmount)
    {
        m_pickupAnimator.SetTrigger("IsPicked");      

        float maxHealth = m_player.m_health.GetMaxHealth();
        float percentHealed = healAmount / maxHealth * 100;

        m_notificationText.text = "HEALED " + percentHealed + "%";     
    }

    void NotificationText(float currencyAmount, float currencyTotal)
    {
        m_pickupAnimator.SetTrigger("IsPicked");

        m_notificationText.text = "ACQUIRED " + currencyAmount + " CATALYST\nTOTAL: " + currencyTotal;
    }

    void NotificationText(WeaponType weapon, int clipAmount, int ammoCap)
    {
        m_pickupAnimator.SetTrigger("IsPicked");

        string weaponName;
        switch (weapon)
        {
            case WeaponType.BaseWeapon:
                m_ammoController.SetAmmoType((int)WeaponType.BaseWeapon);
                weaponName = "ARCHEBUS";
                break;

            case WeaponType.GrenadeWeapon:
                m_ammoController.SetAmmoType((int)WeaponType.GrenadeWeapon);
                weaponName = "GRENADES";
                break;

            case WeaponType.CreatureWeapon:
                m_ammoController.SetAmmoType((int)WeaponType.CreatureWeapon);
                weaponName = "GLAND GUN";
                break;
            default:
                weaponName = "";
                break;
        }

        int clipSize = m_ammoController.GetClipSize();
        int overallAmmo = m_ammoController.GetOverallAmmo();
        ammoCap = m_ammoController.GetAmmoCap();
        int ammoAdded = clipAmount * clipSize;
        //int newTotal = overallAmmo + ammoAdded;       
        

        //if (newTotal > ammoCap)
        if(m_ammoController.IsAmmoFull(weapon))
        {
            // ammoAdded = ammoCap - overallAmmo;
            ammoAdded = m_ammoController.GetAmmoAdded();
            m_notificationText.text = "ACQUIRED +" + ammoAdded + " AMMO FOR " + weaponName + "\nMAX AMMO REACHED";           
        }
        else
        {
            m_notificationText.text = "ACQUIRED +" + ammoAdded + " AMMO FOR " + weaponName;      
        }

    }
}
