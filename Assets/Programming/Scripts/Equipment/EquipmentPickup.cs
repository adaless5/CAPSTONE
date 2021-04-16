using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class EquipmentPickup : MonoBehaviour, ISaveable, ITippable
{
    [SerializeField] int _CorrespondingEquipmentBeltIndex = 0;

    public bool isUsed = false;
    public GameObject _modelObj;
    GameObject _imageObject = null;

    string[] _tipName = { "EQUIPMENT_GRAPPLE", "EQUIPMENT_BLADE", "EQUIPMENT_THERMAL" };

    public ALTPlayerController _player;

    bool bCanDestroyMessage = false;

    bool _canPlayerPickUp = false;

    public Image _toolTip;

    void Awake()
    {
        //SceneManager.sceneLoaded += UpdateEquipmentPickupData;
        EventBroker.OnDataChange += LoadDataOnSceneEnter;
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
        GetComponent<Collider>().enabled = true;
        EventBroker.OnPlayerSpawned += PlayerStart;


    }

    void UpdateEquipmentPickupData(Scene scene, LoadSceneMode scenemode)
    {

        if (scene.name != "R3_0_Persistant")
            LoadDataOnSceneEnter();
    }

    void PlayerStart(GameObject player)
    {
        _player = player.GetComponent<ALTPlayerController>();
        _toolTip = GameObject.FindWithTag("Tip").GetComponent<Image>();
    }

    void Update()
    {
        if (_player != null)
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
                else if (!Keyboard.current.eKey.isPressed && isUsed)
                    bCanDestroyMessage = true;

            }

            if (bCanDestroyMessage)
            {
                if (_player.CheckForInteract())
                {
                    DestroyTip();
                }
            }


            if (_canPlayerPickUp && isUsed == false)
            {
                if (_player.CheckForInteract())
                {
                    _player.GetComponentInChildren<Belt>().ObtainEquipmentAtIndex(_CorrespondingEquipmentBeltIndex);
                    CreateTip("Sprites/Messages/" + _tipName[_CorrespondingEquipmentBeltIndex]);
                    //EventBroker.CallOnPickupWeapon();
                    _canPlayerPickUp = false;
                    isUsed = true;
                    SaveDataOnSceneChange();
                    if (GetComponent<MeshRenderer>() != null)
                    {
                        GetComponent<MeshRenderer>().enabled = false;
                    }
                    if (_modelObj != null)
                    {
                        _modelObj.SetActive(false);

                    }
                    if (transform.GetChild(0) != null)
                    {
                        transform.GetChild(0).gameObject.SetActive(false);
                    }

                    GetComponent<Collider>().enabled = false;



                }
            }
        }
    }

    public void SaveDataOnSceneChange()
    {
        try
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name != "R3_0_Persistant")
                    FileIO.ExportRespawnInfoToFile(ALTPlayerController.instance.transform, SceneManager.GetSceneAt(i).name);

            }
            SaveSystem.Save(gameObject.name, "isEnabled", gameObject.scene.name, isUsed);

        }
        catch { }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            _canPlayerPickUp = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            _canPlayerPickUp = false;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isUsed)
        GameObject.FindObjectOfType<InteractableText>().b_inInteractCollider = true;
    }

    public void LoadDataOnSceneEnter()
    {
        try
        {
            isUsed = SaveSystem.LoadBool(gameObject.name, "isEnabled", gameObject.scene.name);
            if (transform.childCount > 0 && transform.GetChild(0) != null)
            {
                transform.GetChild(0).gameObject.SetActive(!isUsed);
            }
        }
        catch { }
    }

    public void CreateTip(string filename)
    {
        GameObject hud = GameObject.Find("HUD");

        try { GetComponent<AudioManager_Universal>().Play(); }
        catch { }

        if (hud != null)
        {
            Canvas canvas = hud.GetComponent<Canvas>();

            if (canvas != null)
            {
                //DestroyTip();
                bCanDestroyMessage = false;

                //RectTransform trans = _imageObject.AddComponent<RectTransform>();
                //trans.transform.SetParent(canvas.transform); // setting parent
                //trans.localScale = Vector3.one;
                //trans.anchoredPosition = new Vector2(0f, 0f); // setting position, will be on center
                //Texture2D tex = Resources.Load<Texture2D>(filename);
                Sprite spr = Resources.Load<Sprite>(filename);

                //if (tex != null)
                //{
                //    trans.sizeDelta = new Vector2(tex.width, tex.height); // custom size
                //}

                //Image image = _toolTip.GetComponent<Image>();

                //if (image != null)
                //{
                    if (spr != null)
                    {
                        //image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                        //_imageObject.transform.SetParent(canvas.transform);
                        _toolTip.enabled = true;
                        _toolTip.sprite = spr;
                    }
                //}
            }
        }
    }

    public void DestroyTip()
    {
        //GameObject[] array = FindObjectsOfType<GameObject>();
        //foreach (GameObject obj in array)
        //{
        //    if (obj.tag == "Tip")
        //    {
        //        Destroy(obj);
        //    }
        //}

        //_imageObject = null;
        _toolTip.enabled = false;
    }
}
