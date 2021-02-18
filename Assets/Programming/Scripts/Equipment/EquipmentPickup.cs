using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentPickup : MonoBehaviour, ISaveable, ITippable
{
    [SerializeField] int _CorrespondingEquipmentBeltIndex = 0;

    bool isUsed = false;

    GameObject _imageObject = null;

    string[] _tipName = { "EQUIPMENT_GRAPPLE", "EQUIPMENT_BLADE", "EQUIPMENT_THERMAL" };

    ALTPlayerController _player;

    void Awake()
    {
        LoadDataOnSceneEnter();

        if (isUsed) GetComponent<MeshRenderer>().enabled = false;
        else GetComponent<MeshRenderer>().enabled = true;

        EventBroker.OnPlayerSpawned += PlayerStart;


    }

    void PlayerStart(GameObject player)
    {
        _player = player.GetComponent<ALTPlayerController>();
    }

    void Update()
    {
        if (_player != null)
        {
            if (_player.CheckForInteract())
            {
                DestroyTip();
            }

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!isUsed)
        {
            if (other.gameObject.tag == "Player")
            {
                Belt belt = other.gameObject.GetComponentInChildren<Belt>();
                belt.ObtainEquipmentAtIndex(_CorrespondingEquipmentBeltIndex);

                isUsed = true;
                SaveSystem.Save(gameObject.name, "isEnabled", gameObject.scene.name, isUsed);

                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<SphereCollider>().enabled = false;

                CreateTip("Sprites/Messages/" + _tipName[_CorrespondingEquipmentBeltIndex]);
            }
        }
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
