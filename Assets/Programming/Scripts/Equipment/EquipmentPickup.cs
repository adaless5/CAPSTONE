using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentPickup : MonoBehaviour, ISaveable, ITippable
{
    [SerializeField] int _CorrespondingEquipmentBeltIndex = 0;

   
    bool isUsed = false;

    float temprot = 0.0f;

    //string[] resourceDirectory = { "Sprites/Messages/EQUIPMENT_GRAPPLE",  }

    void Awake()
    {
        LoadDataOnSceneEnter();
        SaveSystem.SaveEvent += SaveDataOnSceneChange;
        
        if (isUsed) GetComponent<MeshRenderer>().enabled = false;
        else GetComponent<MeshRenderer>().enabled = true;
    }

    void Update()
    {
        temprot += Time.deltaTime * 100.0f;

        gameObject.transform.eulerAngles = new Vector3(temprot, 0.0f, 0.0f);
    }

    void OnDisable()
    {
        SaveSystem.SaveEvent -= SaveDataOnSceneChange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Belt belt = other.gameObject.GetComponentInChildren<Belt>();
            belt.ObtainEquipmentAtIndex(_CorrespondingEquipmentBeltIndex);
            isUsed = true;
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;

            CreateTip();
        }
    }

    public void SaveDataOnSceneChange()
    {
        SaveSystem.Save(gameObject.name, "isEnabled",gameObject.scene.name, isUsed);
    }   

    public void LoadDataOnSceneEnter()
    {
        isUsed = SaveSystem.LoadBool(gameObject.name, "isEnabled", gameObject.scene.name);
    }

    public void CreateTip()
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud != null)
        {
            //print("HUD found");
            Canvas canvas = hud.GetComponent<Canvas>();

            if (canvas != null)
            {
                
                

                GameObject imgObject = new GameObject("testTip");

                RectTransform trans = imgObject.AddComponent<RectTransform>();
                trans.transform.SetParent(canvas.transform); // setting parent
                trans.localScale = Vector3.one;
                trans.anchoredPosition = new Vector2(0f, -190f); // setting position, will be on center
                Texture2D tex = Resources.Load<Texture2D>("Sprites/Messages/EQUIPMENT_GRAPPLE");
                if(tex != null)
                {
                    trans.sizeDelta = new Vector2(tex.width, tex.height); // custom size
                }

                Image image = imgObject.AddComponent<Image>();
                if(image != null)
                {
                    
                    if (tex != null)
                    {
                        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                        imgObject.transform.SetParent(canvas.transform);
                    }
                }
            }
        }
    }

    public void DestroyTip()
    {

    }
}
