using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AmmoPickup : MonoBehaviour
{
    public WeaponType ammoType;
    private int weaponIndex;

    public int m_amountOfClipsInPickup = 2;
    public int m_clipSize = 6;
    Pickup m_ammoPickup;

    Weapon m_baseWeapon;
    Weapon m_creatureWeapon;
    Weapon m_grenadeWeapon;

    public Compass m_compass;
    public CompassMarkers m_marker;
    public ALTPlayerController m_player;

    bool isPickedUp;
    bool m_markerCreated;

    // Start is called before the first frame update
    void Start()
    {
        EventBroker.OnPlayerSpawned += PlayerSpawned;
        SceneManager.sceneLoaded += onSceneLoaded;

        isPickedUp = false;
        m_markerCreated = false;      
    }

    void onSceneLoaded(Scene currentScene, LoadSceneMode newScene)
    {       
        
    }

    void PlayerSpawned(GameObject playerReference)
    {
        try
        {
            m_ammoPickup = GetComponent<Pickup>();
            m_compass = FindObjectOfType<Compass>();
            m_marker = GetComponent<CompassMarkers>();

        }
        catch { }     
           if (m_marker != null && m_markerCreated == false)
            {               
                m_compass.AddMarker(m_marker);
                m_markerCreated = true;
            }
    }

    void Awake()
    {
        switch (ammoType)
        {
            case WeaponType.BaseWeapon:
                weaponIndex = 0;
                break;

            case WeaponType.GrenadeWeapon:
                weaponIndex = 1;
                break;

            case WeaponType.CreatureWeapon:
                weaponIndex = 2;
                break;
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && isPickedUp == false)
        {
            isPickedUp = true;
            EventBroker.CallOnAmmoPickup(ammoType, m_amountOfClipsInPickup);
            //Destroy(gameObject); // EVAN COMMENTED THIS OUT AND ADDED THE LINE BELOW, IF THERE IS A PROBLEM YELL AT HIM
            gameObject.SetActive(false);

            if (m_marker != null)
                m_compass.RemoveMarker(m_marker);
        }
    }
}
