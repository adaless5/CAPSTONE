using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawner : Weapon, ISaveable
{
    public float Force = 800f;
    public float CoolDown = 5f;
    public GameObject minePrefab;
    public ALTPlayerController m_playerController;

    bool m_bCanThrow = true;
    float timer;
    public override void Start()
    {
        timer = CoolDown;
        bIsActive = false;
        bIsObtained = false;
    }

    void Awake()
    {
        LoadDataOnSceneEnter();
        SaveSystem.SaveEvent += SaveDataOnSceneChange;

        if(m_playerController == null)
        {
            m_playerController = FindObjectOfType<ALTPlayerController>();
        }
    }

    void OnDisable()
    {
        SaveSystem.SaveEvent -= SaveDataOnSceneChange;
    }

    // Update is called once per frame
    public override void Update()
    {
        if(bIsActive && m_playerController.m_ControllerState == ALTPlayerController.ControllerState.Play)
        {
            UseTool();
        }
    }

    public override void UseTool()
    {
        if (m_playerController.CheckForUseWeaponInput())
        {
            if (m_bCanThrow)
            {
                ThrowMine();
                m_bCanThrow = false;
                timer = CoolDown;
                //Debug.Log("THROWN");
            }
        }


        if (!m_bCanThrow)
        {
            timer -= Time.deltaTime;
            //Debug.Log("COOLDOWN");

            if (timer <= 0f)
            {
                m_bCanThrow = true;
                //Debug.Log("RESET");
            }
        }
    }

    void ThrowMine()
    {
        GameObject mine = Instantiate(minePrefab, transform.position, transform.rotation);
        if(mine)
        {
            mine.GetComponent<Rigidbody>().AddForce(transform.forward * Force);
            mine.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), -90f));
        }
    }

    public void SaveDataOnSceneChange()
    {
        SaveSystem.Save(gameObject.name, "bIsActive", bIsActive);
        SaveSystem.Save(gameObject.name, "bIsObtained", bIsObtained);

    }   

    public void LoadDataOnSceneEnter()
    {
        bIsActive = SaveSystem.LoadBool(gameObject.name, "bIsActive");
        bIsObtained = SaveSystem.LoadBool(gameObject.name, "bIsObtained");
    }
}
