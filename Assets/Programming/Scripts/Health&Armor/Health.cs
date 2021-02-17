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

    //Compass Markers
    public Compass m_compass;
    public CompassMarkers m_marker;

    void Start()
    {
        EventBroker.OnPlayerSpawned += PlayerSpawned;

        healthBar = FindObjectOfType<HealthBarUI>();

        if (healthBar != null)
            healthBar.SetMaxHealth(m_MaxHealth);


    }

    void PlayerSpawned(GameObject playerReference)
    {
        m_compass = FindObjectOfType<Compass>();
        m_marker = GetComponent<CompassMarkers>();
        if(gameObject.tag != "Player" && m_marker != null)
        {           
                m_compass.AddMarker(m_marker);
        }
    }

    void Awake()
    {
        LoadDataOnSceneEnter();

        OnTakeHealthDamage += TakeDamage;
        //if (isDead) GetComponent<MeshRenderer>().enabled = false;
        //else GetComponent<MeshRenderer>().enabled = true;
    }

    public void TakeDamage(float damage)
    {
        m_HP -= damage;
        if (healthBar != null && gameObject.tag == "Player")
            healthBar.LoseHealth(m_HP, damage, m_MaxHealth);


        if (m_HP <= 0.0f)
        {
            if (gameObject.tag == "Player")
            {
                PlayerDeath();
            }
            else
            {
                Die();                
            }
        }

        //Clamp values -LCC
        m_HP = Mathf.Clamp(m_HP, 0, m_MaxHealth);

    }

    public float GetMaxHealth()
    {
        return m_MaxHealth;
    }
    void Die()
    {
        if (m_marker != null)
        {
            m_compass.RemoveMarker(m_marker);
        }

        //Temporary Spawning Stuff - Anthony
        Spawner spawner = GetComponent<Spawner>();
        if (spawner != null)
        {
            spawner.Spawn();
        }

        isDead = true;
        //Destroy(gameObject);
        gameObject.SetActive(false);
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        //transform.DetachChildren();
        CallOnDeath();
    }

    void PlayerDeath()
    {
        if (gameObject.tag == "Player")
        {
            EventBroker.CallOnPlayerDeath();

        }

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
        if (m_HP < m_MaxHealth)
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
        if (m_HP < m_MaxHealth)
        {
            CallOnHeal(healthToHeal);
            m_HP += healthToHeal;

            if (healthBar != null)
                healthBar.GainHealth(m_HP, healthToHeal, m_MaxHealth);

            m_HP = Mathf.Clamp(m_HP, 0, m_MaxHealth);
            Debug.Log("Healed" + healthToHeal + "amount");
        }
    }

    public void LoadDataOnSceneEnter()
    {
        //isDead = SaveSystem.LoadBool(gameObject.name, "isDead", gameObject.scene.name);

        //if (SaveSystem.LoadFloat(gameObject.name, "posX", gameObject.scene.name) != 0)
        //{
        //    transform.position = new Vector3(SaveSystem.LoadFloat(gameObject.name, "posX", gameObject.scene.name), SaveSystem.LoadFloat(gameObject.name, "posY", gameObject.scene.name), SaveSystem.LoadFloat(gameObject.name, "posZ", gameObject.scene.name));
        //}

        //transform.rotation = new Quaternion(
        //    SaveSystem.LoadFloat(gameObject.name, "rotX", gameObject.scene.name),
        //    SaveSystem.LoadFloat(gameObject.name, "rotY", gameObject.scene.name),
        //    SaveSystem.LoadFloat(gameObject.name, "rotZ", gameObject.scene.name),
        //    SaveSystem.LoadFloat(gameObject.name, "rotW", gameObject.scene.name));
    }

}
