using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class WeaponPickup : MonoBehaviour, ITippable
{
    public WeaponType _pickUpWeapon;
    int weaponNum;
    bool isUsed = false;
    public GameObject _modelObj;
    GameObject _imageObject;

    string[] _tipName = { "EQUIPMENT_DEFAULT_GUN", "EQUIPMENT_GRENADE", "EQUIPMENT_GLANDGUN" };

    ALTPlayerController player;

    bool bCanDestroyMessage = false;

    void Awake()
    {
        LoadDataOnSceneEnter();

        switch (_pickUpWeapon)
        {
            case WeaponType.BaseWeapon:
                weaponNum = 0;
                break;
            case WeaponType.GrenadeWeapon:
                weaponNum = 1;
                break;
            case WeaponType.CreatureWeapon:
                weaponNum = 2;
                break;
        }
        if (GetComponent<MeshRenderer>() != null)
        {
            if (isUsed) GetComponent<MeshRenderer>().enabled = false;
            else GetComponent<MeshRenderer>().enabled = true;
        }
        if (_imageObject != null)
        {
            if (isUsed) _modelObj.SetActive(false);
            else _modelObj.SetActive(true);
        }

        EventBroker.OnPlayerSpawned += PlayerSpawn;
    }

    void PlayerSpawn(GameObject playerref)
    {
        try
        {
            player = playerref.GetComponent<ALTPlayerController>();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    void Update()
    {
        if (player != null)
        {
            //Prevents player from being able to hold the interact button and dismiss the notification
            if (!bCanDestroyMessage)
            {
                if (Gamepad.current != null)
                {
                    if (Gamepad.current.xButton.IsActuated())
                    {
                        if (!Gamepad.current.xButton.isPressed)
                            bCanDestroyMessage = true;
                    }
                    else if (!Keyboard.current.eKey.isPressed)
                        bCanDestroyMessage = true;
                }
                else if (!Keyboard.current.eKey.isPressed)
                    bCanDestroyMessage = true;
            }

            if (bCanDestroyMessage)
            {
                if (player.CheckForInteract())
                {
                    DestroyTip();
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "Player" && !isUsed)
        {
            GameObject.FindObjectOfType<InteractableText>().b_inInteractCollider = true;
            if (FindObjectOfType<ALTPlayerController>() != null && FindObjectOfType<ALTPlayerController>().CheckForInteract())
            {
                EventBroker.CallOnPickupWeapon(weaponNum);
                isUsed = true;
                if (GetComponent<MeshRenderer>() != null)
                {
                    GetComponent<MeshRenderer>().enabled = false;
                }
                if (_modelObj != null)
                {
                    _modelObj.SetActive(false);
                }
                GetComponent<Collider>().enabled = false;

                CreateTip("Sprites/Messages/" + _tipName[weaponNum]);
            }
        }
    }


    public void SaveDataOnSceneChange()
    {
        SaveSystem.Save(gameObject.name, "isEnabled", gameObject.scene.name, isUsed);
    }

    public void LoadDataOnSceneEnter()
    {
        isUsed = SaveSystem.LoadBool(gameObject.name, "isEnabled", gameObject.scene.name);
    }

    public void CreateTip(string filename)
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud != null)
        {
            Canvas canvas = hud.GetComponent<Canvas>();

            if (canvas != null)
            {
                DestroyTip();
                bCanDestroyMessage = false;
                _imageObject = new GameObject("testTip");
                _imageObject.tag = "Tip";

                RectTransform trans = _imageObject.AddComponent<RectTransform>();
                trans.transform.SetParent(canvas.transform); // setting parent
                trans.localScale = Vector3.one;
                trans.anchoredPosition = new Vector2(0f, 0f); // setting position, will be on center
                Texture2D tex = Resources.Load<Texture2D>(filename);

                if (tex != null)
                {
                    trans.sizeDelta = new Vector2(tex.width, tex.height); // custom size
                }

                Image image = _imageObject.AddComponent<Image>();
                if (image != null)
                {
                    if (tex != null)
                    {
                        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                        _imageObject.transform.SetParent(canvas.transform);
                    }
                }
            }
        }
    }

    public void DestroyTip()
    {
        GameObject[] array = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in array)
        {
            if (obj.tag == "Tip")
            {
                Destroy(obj);
            }
        }

        _imageObject = null;
    }
}
