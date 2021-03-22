using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UpgradeMenuUI : MonoBehaviour
{
    // Start is called before the first frame update
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
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void LateUpdate()
    {
        if (_player != null)
        {
            if (_player.GetComponentInChildren<ALTPlayerController>().CheckForInteract() && _bInMenu == true)
            {
                Deactivate();
            }
        }
        else
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }
      //  DiscriptionText.text = "";
        for (int i = 0; i < _buttons.Count; i++)
        {
            if (_buttons[i].GetComponent<ButtonHighlight>().IsHighlighted)
            {
                DiscriptionText.text = _upgrades[i].Discription;
                break;
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
            ALTPlayerController pc = _player.GetComponent<ALTPlayerController>();
            pc.m_ControllerState = ALTPlayerController.ControllerState.Menu;
        }
    }

    public void Deactivate()
    {
        UpgradeMenu.SetActive(false);
        _bInMenu = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ALTPlayerController pc = _player.GetComponent<ALTPlayerController>();
        pc.m_ControllerState = ALTPlayerController.ControllerState.Play;
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
                Tool item = _player.GetComponent<ALTPlayerController>()._weaponBelt._items[i];
                if (item.GetComponent<Weapon>())
                {
                    Weapon weapon = item.GetComponent<Weapon>();
                    _weapons.Add(weapon);

                    if (weapon.bIsObtained)
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

            for (int i = 0; i < (int)WeaponType.NumberOfWeapons; i++)
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
                if (_buttons[i].GetComponentInChildren<TMP_Text>())
                {
                    _buttons[i].GetComponentInChildren<TMP_Text>().text = _upgrades[i].Title + " - Price: $" + _upgrades[i].UpgradeWorth.ToString();
                }
            }

            CurrentCurrencyText.text = "Current currency: $" + _player.GetComponent<ALTPlayerController>().m_UpgradeCurrencyAmount.ToString();

            hasButtonListeners = true;

        }
    }

    public void AddUpgrade(int weaponindex, int upgradeindex)
    {
        int y = ((int)EUpgradeType.NumUpgrades * weaponindex) + upgradeindex;

        if (CheckValidBuy(y))
        {
            _player.GetComponent<ALTPlayerController>().m_UpgradeCurrencyAmount -= _upgrades[y].UpgradeWorth;
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
        foreach (GameObject obj in Parents)
        {
            obj.SetActive(false);
        }

        Parents[index].SetActive(true);
    }

    private void UpdateUI(int index)
    {
        _buttons[index].gameObject.SetActive(false);
        CurrentCurrencyText.text = "Current currency: $" + _player.GetComponent<ALTPlayerController>().m_UpgradeCurrencyAmount.ToString();
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
        if (_upgrades[upgradeindex].UpgradeWorth <= _player.GetComponent<ALTPlayerController>().m_UpgradeCurrencyAmount)
        {
            Debug.Log(_upgrades[upgradeindex].UpgradeWorth.ToString());
            Debug.Log(_player.GetComponent<ALTPlayerController>().m_UpgradeCurrencyAmount.ToString());
            Debug.Log("can buy");
            return true;
        }
        Debug.Log(_upgrades[upgradeindex].UpgradeWorth.ToString());
        Debug.Log(_player.GetComponent<ALTPlayerController>().m_UpgradeCurrencyAmount.ToString());
        Debug.Log("cant buy");
        return false;
    }

}
