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

    private float m_MaxArmor = 100f;
    private float m_MaxHealth = 100f;

    [Space]
    [Header("Shield Settings (CHECK OFF ARMOR BOOL TO ACTIVATE ARMOR)")]
    [SerializeField]
    private bool m_HasArmor = false;
    [SerializeField]
    private float m_Armor = 0.0f;
    [SerializeField]
    private float m_ArmorCooldown = 6f;
    [SerializeField]
    private float m_ArmorRefreshRate = 0.3f;

    [Space]
    [Header("Debug Variables")]
    [SerializeField]
    private bool m_HasTakenDamage = false;
    [SerializeField]
    private bool m_IsLoadingShield = false;

    public event Action<float> OnTakeDamage;
    public event Action<float> OnHeal;
    public event Action OnDeath;

    bool isDead = false;

    void Awake()
    {
        LoadDataOnSceneEnter();
        SaveSystem.SaveEvent += SaveDataOnSceneChange;

        //if (isDead) GetComponent<MeshRenderer>().enabled = false;
        //else GetComponent<MeshRenderer>().enabled = true;
    }

    void OnDisable()
    {
        SaveSystem.SaveEvent -= SaveDataOnSceneChange;
    }

    public void TakeDamage(float damage)
    {

        StartCoroutine(CheckHealth());
        if (m_Armor > 0)
        {
            m_Armor -= damage;
            Debug.Log("Damage to Armor, Current Armor at " + m_Armor);
        }
        else
        {

            m_HP -= damage;
            if (m_HP <= 0.0f)
            {
                Die();
            }
        }

        //Clamp values -LCC
        m_Armor = Mathf.Clamp(m_Armor, 0, m_MaxArmor);
        m_HP = Mathf.Clamp(m_HP, 0, m_MaxHealth);

    }

    private void Update()
    {
        if (m_HasArmor)
        {
            if (m_Armor == 0 && m_IsLoadingShield == false)
            {
                m_IsLoadingShield = true;
                StartCoroutine(ReloadArmor());
            }
        }
    }


    public IEnumerator CheckHealth()
    {
        StopCoroutine(ReloadArmor());
        m_HasTakenDamage = true;
        yield return new WaitForSeconds(5.0f);
        m_HasTakenDamage = false;
    }

    public IEnumerator ReloadArmor()
    {
        Debug.Log("Shield Depleated!");
        yield return new WaitForSeconds(m_ArmorCooldown);

        if (!m_HasTakenDamage)
        {
            while (m_Armor != m_MaxArmor)
            {
                Debug.Log("Regenerating...");

                m_Armor += 20f;
                yield return new WaitForSeconds(m_ArmorRefreshRate);
            }
        }

        m_IsLoadingShield = false;
        //Debug.Log("Shield Regenerated");
        yield return null;
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
