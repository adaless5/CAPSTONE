using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    public GameObject _Container;


    public GameObject _DefaultAmmoPickup;
    public GameObject _GrenadePickup;
    public GameObject _CreaturePickup;
    public GameObject _HealthPickup;
    public GameObject _Jumper;

    AmmoController _ammoController;

    string[] Tags = { "Creature_Weapon", "Player_Weapon", "Player_Blade", "Player_Mine" };
    List<string> PossiblePickupsTags = new List<string>();

    bool bIsBroken;
    // Start is called before the first frame update
    void Start()
    {
        _ammoController = FindObjectOfType<AmmoController>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void CheckPlayerEquipment()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (_ammoController == null)
        {
            _ammoController = FindObjectOfType<AmmoController>();
        }
        if (player)
        {
            WeaponBase playerWeapon = player.GetComponentInChildren<WeaponBase>();
            if (playerWeapon.bIsObtained && !_ammoController.IsAmmoFull(WeaponType.BaseWeapon))
            {
                PossiblePickupsTags.Add("Player_Weapon");
            }

            CreatureWeapon creatureWeapon = player.GetComponentInChildren<CreatureWeapon>();
            if (creatureWeapon.bIsObtained && !_ammoController.IsAmmoFull(WeaponType.CreatureWeapon))
            {
                PossiblePickupsTags.Add("Creature_Weapon");
            }

            MineSpawner mineSpawner = player.GetComponentInChildren<MineSpawner>();
            if (mineSpawner.bIsObtained && !_ammoController.IsAmmoFull(WeaponType.GrenadeWeapon))
            {
                PossiblePickupsTags.Add("Player_Mine");
            }

            Health health = player.GetComponentInChildren<Health>();
            if (!health.IsAtFullHealth())
            {
                PossiblePickupsTags.Add("Health_Pickup");
            }
        }
    }
    void SpawnPickup()
    {
        CheckPlayerEquipment();
        AmmoController ammoController = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<AmmoController>();
        if (_ammoController == null)
        {
            _ammoController = FindObjectOfType<AmmoController>();
        }

        if (PossiblePickupsTags.Count > 0)
        {
            int randint = Random.Range(0, PossiblePickupsTags.Count);

            if (PossiblePickupsTags[randint] == "Player_Weapon")
            {
                GameObject pickup = Instantiate(_DefaultAmmoPickup, transform.position, transform.rotation, transform);
                pickup.isStatic = false;
                pickup.SetActive(true);
            }

            else if (PossiblePickupsTags[randint] == "Player_Mine")
            {
                GameObject pickup = Instantiate(_GrenadePickup, transform.position, transform.rotation, transform);
                pickup.isStatic = false;
                pickup.SetActive(true);
            }

            else if (PossiblePickupsTags[randint] == "Creature_Weapon")
            {
                GameObject pickup = Instantiate(_CreaturePickup, transform.position, transform.rotation, transform);
                pickup.isStatic = false;
                pickup.SetActive(true);
            }

            else if (PossiblePickupsTags[randint] == "Health_Pickup")
            {
                GameObject pickup = Instantiate(_HealthPickup, transform.position, transform.rotation, transform);
                pickup.isStatic = false;
                pickup.SetActive(true);
            }

        }
        else
        {
            GameObject pickup = Instantiate(_Jumper, transform.position, transform.rotation, transform);
            pickup.isStatic = false;
            pickup.SetActive(true);
        }
    }
    public void Break(string tag)
    {
        if (!bIsBroken)
            foreach (string t in Tags)
            {
                if (tag == t)
                {
                    GetComponent<Collider>().enabled = false;
                    _Container.SetActive(false);
                    SpawnPickup();
                    bIsBroken = true;
                }
            }
    }
}
