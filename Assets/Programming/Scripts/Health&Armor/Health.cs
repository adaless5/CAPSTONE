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
