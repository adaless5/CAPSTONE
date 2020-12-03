using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickup : MonoBehaviour, ITippable
{
    public WeaponType _pickUpWeapon;
    int weaponNum;
    bool isUsed = false;

    GameObject _imageObject = null;

    string[] _tipName = { "EQUIPMENT_DEFAULT_GUN", "EQUIPMENT_GRENADE", "EQUIPMENT_GLANDGUN" };

    void Awake()
    {
        LoadDataOnSceneEnter();
        SaveSystem.SaveEvent += SaveDataOnSceneChange;

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

        if (isUsed) GetComponent<MeshRenderer>().enabled = false;
        else GetComponent<MeshRenderer>().enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            DestroyTip();
        }
    }

    void OnDisable()
    {
        SaveSystem.SaveEvent -= SaveDataOnSceneChange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //GameObject g = GameObject.FindGameObjectWithTag("WeaponBelt");
            //Belt b = g.GetComponent<Belt>();
            EventBroker.CallOnPickupWeapon(weaponNum);
            //b.EquipToolAtIndex(weaponNum);
            isUsed = true;
            GetComponent<MeshRenderer>().enabled = false;

            CreateTip("Sprites/Messages/" + _tipName[weaponNum]);
            //Destroy(gameObject);
        }
    }

    public void SaveDataOnSceneChange()
    {
        SaveSystem.Save(gameObject.name, "isEnabled", gameObject.scene.name, isUsed);
        //Debug.Log(isUsed);
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

                _imageObject = new GameObject("testTip");
                _imageObject.tag = "Tip";

                RectTransform trans = _imageObject.AddComponent<RectTransform>();
                trans.transform.SetParent(canvas.transform); // setting parent
                trans.localScale = Vector3.one;
                trans.anchoredPosition = new Vector2(0f, 0f); // setting position, will be on center
                Texture2D tex = Resources.Load<Texture2D>(filename);
                print(tex);

                if (tex != null)
                {
                    //print("Found Texture");
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
