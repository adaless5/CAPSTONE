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

    public event Action<float> OnTakeHealthDamage;
    public event Action<float> OnHeal;
    public event Action OnDeath;

    bool isDead = false;

    public HealthBarUI healthBar;

    void Start()
    {
        healthBar = FindObjectOfType<HealthBarUI>();

        if (healthBar != null)
           healthBar.SetMaxHealth(m_MaxHealth);
    }

    void Awake()
    {
        LoadDataOnSceneEnter();
        SaveSystem.SaveEvent += SaveDataOnSceneChange;

        OnTakeHealthDamage += TakeDamage;
        //if (isDead) GetComponent<MeshRenderer>().enabled = false;
        //else GetComponent<MeshRenderer>().enabled = true;
    }

    void OnDisable()
    {
        SaveSystem.SaveEvent -= SaveDataOnSceneChange;
    }

    public void TakeDamage(float damage)
    {
        m_HP -= damage;
        if (healthBar != null)
            healthBar.SetHealth(m_HP);

        if (m_HP <= 0.0f)
        {
            Die();
        }

        //Clamp values -LCC
        m_HP = Mathf.Clamp(m_HP, 0, m_MaxHealth);
    }


    void Die()
    {
        CallOnDeath();
        //Temporary Spawning Stuff - Anthony
        Spawner spawner = GetComponent<Spawner>();
        if (spawner != null)
        {
            spawner.Spawn();
        }

        isDead = true;
        //Destroy(gameObject);
        gameObject.SetActive(false);
        transform.DetachChildren();
    }

    public void CallOnTakeHealthDamage(float damageToTake)
    {
        OnTakeHealthDamage?.Invoke(damageToTake);
    }

    public void CallOnDeath()
    {
        OnDeath?.Invoke();
    }

    public void CallOnHeal(float healthToHeal)
    {
        OnHeal?.Invoke(healthToHeal);
    }

    public bool IsAtFullHealth()
    {
        if(m_HP < m_MaxHealth)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Heal(float healthToHeal)
    {
        if(m_HP < m_MaxHealth)
        {
            CallOnHeal(healthToHeal);
            m_HP += healthToHeal;

            if (healthBar != null)
                healthBar.SetHealth(m_HP);

            m_HP = Mathf.Clamp(m_HP, 0, m_MaxHealth);
            Debug.Log("Healed" + healthToHeal + "amount");
        }
    }

    public void SaveDataOnSceneChange()
    {
        SaveSystem.Save(gameObject.name, "isDead", gameObject.scene.name, isDead);

        SaveSystem.Save(gameObject.name, "posX", gameObject.scene.name, transform.position.x);
        SaveSystem.Save(gameObject.name, "posY", gameObject.scene.name, transform.position.y);
        SaveSystem.Save(gameObject.name, "posZ", gameObject.scene.name, transform.position.z);

        SaveSystem.Save(gameObject.name, "rotX", gameObject.scene.name, transform.rotation.x);
        SaveSystem.Save(gameObject.name, "rotY", gameObject.scene.name, transform.rotation.y);
        SaveSystem.Save(gameObject.name, "rotZ", gameObject.scene.name, transform.rotation.z);
        SaveSystem.Save(gameObject.name, "rotW", gameObject.scene.name, transform.rotation.w);
    }

    public void LoadDataOnSceneEnter()
    {
        isDead = SaveSystem.LoadBool(gameObject.name, "isDead", gameObject.scene.name);

        if (SaveSystem.LoadFloat(gameObject.name, "posX", gameObject.scene.name) != 0)
        {
            transform.position = new Vector3(SaveSystem.LoadFloat(gameObject.name, "posX", gameObject.scene.name), SaveSystem.LoadFloat(gameObject.name, "posY", gameObject.scene.name), SaveSystem.LoadFloat(gameObject.name, "posZ", gameObject.scene.name));
        }

        transform.rotation = new Quaternion(
            SaveSystem.LoadFloat(gameObject.name, "rotX", gameObject.scene.name),
            SaveSystem.LoadFloat(gameObject.name, "rotY", gameObject.scene.name),
            SaveSystem.LoadFloat(gameObject.name, "rotZ", gameObject.scene.name),
            SaveSystem.LoadFloat(gameObject.name, "rotW", gameObject.scene.name));
    }

}
