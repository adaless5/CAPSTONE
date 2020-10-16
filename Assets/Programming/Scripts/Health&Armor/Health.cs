using System;
using System.Collections;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;

//VR - This class is a damage system for any target/enemy
public class Health : MonoBehaviour, ISaveable
{
    [Header("Health Settings")]
    [SerializeField]
    private float m_HP = 50.0f;
    private float m_MaxHealth = 100f;



    [Space]
    [Header("Debug Variables")]
    [SerializeField]
    private bool m_IsLoadingShield = false;

    public event Action<float> OnTakeDamage;
    public event Action<float> OnHeal;
    public event Action OnDeath;

    bool isDead = false;

    //public HealthBarUI healthBar;

    void Awake()
    {
        LoadDataOnSceneEnter();
        SaveSystem.SaveEvent += SaveDataOnSceneChange;

        //if (isDead) GetComponent<MeshRenderer>().enabled = false;
        //else GetComponent<MeshRenderer>().enabled = true;
        //healthBar.SetMaxHealth(m_MaxHealth);
    }

    void OnDisable()
    {
        SaveSystem.SaveEvent -= SaveDataOnSceneChange;
    }

    public void TakeDamage(float damage)
    {
        CallOnTakeDamage(damage);

        m_HP -= damage;

        if (m_HP <= 0.0f)
        {
            Die();
        }


        //Clamp values -LCC
        m_HP = Mathf.Clamp(m_HP, 0, m_MaxHealth);

    }


    //VR - Currently destroying gameObjects when damaged, can look into creating object pool for recyclable assets later on
    void Die()
    {
        isDead = true;
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    public void CallOnTakeDamage(float damageToTake)
    {
        OnTakeDamage?.Invoke(damageToTake);
    }

    public void CallOnDeath()
    {
        OnDeath?.Invoke();
    }

    public void CallOnHeal(float healthToHeal)
    {
        OnHeal?.Invoke(healthToHeal);
    }

    public void Heal(float healthToHeal)
    {
        m_HP += healthToHeal;
        m_HP = Mathf.Clamp(m_HP, 0, m_MaxHealth);
        Debug.Log("Healed" + healthToHeal + "amount");
    }

    public void SaveDataOnSceneChange()
    {
        SaveSystem.Save(gameObject.name, "isDead", isDead);

        SaveSystem.Save(gameObject.name, "posX", transform.position.x);
        SaveSystem.Save(gameObject.name, "posY", transform.position.y);
        SaveSystem.Save(gameObject.name, "posZ", transform.position.z);

        SaveSystem.Save(gameObject.name, "rotX", transform.rotation.x);
        SaveSystem.Save(gameObject.name, "rotY", transform.rotation.y);
        SaveSystem.Save(gameObject.name, "rotZ", transform.rotation.z);
        SaveSystem.Save(gameObject.name, "rotW", transform.rotation.w);
    }

    public void LoadDataOnSceneEnter()
    {
        isDead = SaveSystem.LoadBool(gameObject.name, "isDead");

        if (SaveSystem.LoadFloat(gameObject.name, "posX") != 0)
        {
            transform.position = new Vector3(SaveSystem.LoadFloat(gameObject.name, "posX"), SaveSystem.LoadFloat(gameObject.name, "posY"), SaveSystem.LoadFloat(gameObject.name, "posZ"));
        }

        transform.rotation = new Quaternion(
            SaveSystem.LoadFloat(gameObject.name, "rotX"),
            SaveSystem.LoadFloat(gameObject.name, "rotY"),
            SaveSystem.LoadFloat(gameObject.name, "rotZ"),
            SaveSystem.LoadFloat(gameObject.name, "rotW"));
    }



}
