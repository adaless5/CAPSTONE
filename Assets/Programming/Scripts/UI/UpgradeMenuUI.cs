using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UpgradeMenuUI : MonoBehaviour
{
    public GameObject UpgradeMenu;

    public List<GameObject> Parents;
    public List<GameObject> ParentsButtons;
    public TMP_Text DiscriptionText;
    public TMP_Text CurrentCurrencyText;

    public int _numUpgradesAllowed = 999;
    int upgradeamt = 0;

    GameObject _player;
    bool _bInMenu = false;

    List<Weapon> _weapons;
    List<WeaponUpgrade> _upgrades;
    List<Button> _buttons;

    bool hasButtonListeners = false;

    void Start()
    {
        _weapons = new List<Weapon>();
        _buttons = new List<Button>();
        EventBroker.OnPlayerSpawned += PlayerSpawned;
    }

    void PlayerSpawned(GameObject player)
    {
        _player = player;
    }

    private void LateUpdate()
    {
        //if (ALTPlayerController.instance.CheckForInteract() && ALTPlayerController.instance.m_ControllerState == ALTPlayerController.ControllerState.Menu)
        //{
        //    Deactivate();
        //}

        if (DiscriptionText != null)
        {
            DiscriptionText.text = "";
            if (_buttons != null)
            {
                for (int i = 0; i < _buttons.Count; i++)
                {
                    if (_buttons[i].GetComponent<ButtonHighlight>().IsHighlighted)
                    {
                        DiscriptionText.text = _upgrades[i].Discription;
                        break;
                    }
                }
            }
        }
    }

    public void Activate()
    {
        if (upgradeamt <= _numUpgradesAllowed)
        {
            UpgradeMenu.SetActive(true);
            _bInMenu = true;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            ALTPlayerController pc = ALTPlayerController.instance;
            pc.m_ControllerState = ALTPlayerController.ControllerState.Menu;
            //Debug.Log("Upgrade menu state: " + ALTPlayerController.instance.m_ControllerState);
        }

    }

    public void Deactivate()
    {
        if (_bInMenu)
        {
            UpgradeMenu.SetActive(false);
            _bInMenu = false;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            ALTPlayerController pc = ALTPlayerController.instance;
            pc.m_ControllerState = ALTPlayerController.ControllerState.Play;
            //Debug.Log("Upgrade menu state: " + ALTPlayerController.instance.m_ControllerState);
        }

    }

    public void InitUpgradeMenu(List<WeaponUpgrade> list)
    {
        if (upgradeamt <= _numUpgradesAllowed)
        {
            _upgrades = list;
            _weapons.Clear();
            _buttons.Clear();

            //set up weapon display and gets info from weapons
            bool set = false;
            for (int i = 0; i < (int)WeaponType.NumberOfWeapons; i++)
            {
                Tool item = ALTPlayerController.instance._weaponBelt._items[i];
                if (item.GetComponent<Weapon>())
                {
                    Weapon weapon = item.GetComponent<Weapon>();
                    _weapons.Add(weapon);

                    if (weapon.bIsObtained )
                    {
                        ParentsButtons[i].SetActive(true);
                        if (!set)
                        {
                            Parents[i].SetActive(true);//todo: this will always set the default guns upgrades to active. it needs to check what the player has first
                            set = true;
                        }
                    }

                }
            }

            //Set up buttons 

            for(int i = 0; i < (int)WeaponType.NumberOfWeapons; i++)
            {
                for (int j = 0; j < (int)EUpgradeType.NumUpgrades; j++)
                {
                    if (Parents[i].transform.Find("Upgrade" + j))
                    {
                        GameObject obj = Parents[i].transform.Find("Upgrade" + j).gameObject;
                        if (obj.GetComponent<Button>())
                        {
                            int x = i;
                            int y = j;
                            Button button = obj.GetComponent<Button>();
                            if (!hasButtonListeners) button.onClick.AddListener(() => AddUpgrade(x, y));
                            button.gameObject.SetActive(true);
                            if (CheckHasUpgrade(i, j)) button.gameObject.SetActive(false);
                            _buttons.Add(button);
                        }
                    }
                }
            }

            //set up button text
            for (int i = 0; i < _buttons.Count; i++)
            {
                TMP_Text[] txts = _buttons[i].GetComponentsInChildren<TMP_Text>();
                foreach (TMP_Text obj in txts)
                {
                    if (obj.name == "UpgradeName")
                    {
                        obj.text = _upgrades[i].Title;
                    }
                    else if(obj.name == "Price")
                    {
                        obj.text = _upgrades[i].UpgradeWorth.ToString();
                    }
                }
            }

            CurrentCurrencyText.text = ALTPlayerController.instance.m_UpgradeCurrencyAmount.ToString();

            hasButtonListeners = true;

        }
    }

    public void AddUpgrade(int weaponindex, int upgradeindex)
    {
        int y = ((int)EUpgradeType.NumUpgrades * weaponindex) + upgradeindex;

        if (CheckValidBuy(y))
        {
            ALTPlayerController.instance.m_UpgradeCurrencyAmount -= _upgrades[y].UpgradeWorth;
            _weapons[weaponindex].AddUpgrade(_upgrades[y]);
            UpdateUI(y);
            upgradeamt++;

            if (upgradeamt >= _numUpgradesAllowed)
            {
                Deactivate();
            }
        }
    }

    public void DisplayUpgradeButtons(int index)
    {
        foreach(GameObject obj in Parents)
        {
            obj.SetActive(false);
        }

        Parents[index].SetActive(true);
    }

    private void UpdateUI(int index)
    {
        _buttons[index].gameObject.SetActive(false);
        CurrentCurrencyText.text = ALTPlayerController.instance.m_UpgradeCurrencyAmount.ToString();
    }

    private bool CheckHasUpgrade(int weapon, int upgrade)
    {
        bool hasup = false;
        if (weapon < _weapons.Count)
        {
            foreach (EUpgradeType up in _weapons[weapon].m_currentupgrades)
            {
                if (up == (EUpgradeType)upgrade)
                {
                    hasup = true;
                }
            }
        }
        return hasup;
    }

    private bool CheckValidBuy(int upgradeindex)
    {
        if (_upgrades[upgradeindex].UpgradeWorth <= ALTPlayerController.instance.m_UpgradeCurrencyAmount)
        {
            return true;
        }
        return false;
    }

}
