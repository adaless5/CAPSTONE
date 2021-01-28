using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenuUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject UpgradeMenu;
    public GameObject DeafaultGunParent;
    public GameObject CreatureGunParent;
    public GameObject ExplosiveWeaponParent;

    GameObject _player;
    bool _bInMenu = false;

    List<Weapon> _weapons;
    List<WeaponScalars> _upgrades;

    int debug;
    void Start()
    {
        _weapons = new List<Weapon>();

        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (Input.GetButtonDown("Pause") && _bInMenu == true)
        {
            Deactivate();
        }
    }

    public void Activate()
    {
        UpgradeMenu.SetActive(true);
        _bInMenu = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ALTPlayerController pc = _player.GetComponent<ALTPlayerController>();
        //pc.enabled = false;
        pc.m_ControllerState = ALTPlayerController.ControllerState.Menu;
    }

    public void Deactivate()
    {
        UpgradeMenu.SetActive(false);
        _bInMenu = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ALTPlayerController pc = _player.GetComponent<ALTPlayerController>();
        //pc.enabled = false;
        pc.m_ControllerState = ALTPlayerController.ControllerState.Play;
    }

    public void InitUpgradeMenu(List<WeaponScalars> list)
    {
        _upgrades = list;
        for (int i = 0; i < (int)WeaponType.NumberOfWeapons; i++)
        {
            Tool item = _player.GetComponent<ALTPlayerController>()._weaponBelt._items[i];
            if(item.GetComponent<Weapon>())
            {
                Weapon weapon = item.GetComponent<Weapon>();
                _weapons.Add(weapon);
            }
        }
        //todo get weapons and what upgrades they have
        //get all buttons and add function listeners
        for (int i = 0; i < 3; i++)
        {
            if(DeafaultGunParent.transform.Find("Upgrade" + i))
            {
                GameObject obj = DeafaultGunParent.transform.Find("Upgrade" + i).gameObject;
                if(obj.GetComponent<Button>())
                {
                    int x = i;
                    Button button = obj.GetComponent<Button>();
                    button.onClick.AddListener(() => AddUpgrade(0, x));
                    debug = i;
                }
            }
            if (ExplosiveWeaponParent.transform.Find("Upgrade" + i))
            {
                GameObject obj = ExplosiveWeaponParent.transform.Find("Upgrade" + i).gameObject;
                if (obj.GetComponent<Button>())
                {
                    int x = i;
                    Button button = obj.GetComponent<Button>();
                    button.onClick.AddListener(() => AddUpgrade(1, x));
                }
            }
            if (CreatureGunParent.transform.Find("Upgrade" + i))
            {
                GameObject obj = CreatureGunParent.transform.Find("Upgrade" + i).gameObject;
                if (obj.GetComponent<Button>())
                {
                    int x = i;
                    Button button = obj.GetComponent<Button>();
                    button.onClick.AddListener(() => AddUpgrade(2, x));
                }
            }
        
        }

        Debug.Log(debug.ToString());
        //enable and disable icons and buttons as needed
    }

    public void AddUpgrade(int weaponindex, int upgradeindex)
    {
        Debug.Log(debug.ToString());
        Debug.Log(upgradeindex.ToString());
        int x = _upgrades.Count / 3;
        int y = (x * weaponindex) + upgradeindex;

        _weapons[weaponindex].AddUpgrade(_upgrades[y]);
    }

    public void AddAction()
    {

    }
}
