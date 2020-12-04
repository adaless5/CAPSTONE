using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Armor : MonoBehaviour
{
    //[Header("Shield Settings")]
    [SerializeField]
    private float m_Armor;
    [SerializeField]
    private float m_ArmorCooldown;
    [SerializeField]
    private float m_ArmorRefreshRate;
    [SerializeField]
    private float m_MaxArmor;
    [SerializeField]
    private float m_ArmorTimer;
    [SerializeField]
    private float m_RegenAmount;

    public ArmorBarUI armorBar;

    // Start is called before the first frame update
    void Start()
    {
        m_Armor = 100.0f;
        m_ArmorRefreshRate = 0.3f;
        m_MaxArmor = 100f;
        m_ArmorCooldown = 6f;
        m_RegenAmount = 20f;
        m_ArmorTimer = m_ArmorCooldown;

        armorBar.SetMaxArmor(m_MaxArmor);
    }


    public float GetCurrentArmor()
    {
        return m_Armor;
    }

    public void ResetArmorTimer(float damage)
    {
        m_ArmorTimer = m_ArmorCooldown;
    }

    public void TakeDamage(float damage)
    {
        StopCoroutine(ReloadArmor());
        if (m_Armor > 0)
        {

            m_Armor -= damage;
            armorBar.LoseArmor(m_Armor, damage, m_MaxArmor);

            Debug.Log("Damage to Armor, Current Armor at " + m_Armor);
        }

        m_Armor = Mathf.Clamp(m_Armor, 0, m_MaxArmor);
    }

    void Update()
    {
        if (m_Armor < m_MaxArmor)
        {
            m_ArmorTimer -= Time.deltaTime;
        }
        
        if (m_ArmorTimer <= 0)
        {
            m_ArmorTimer = m_ArmorCooldown;
            StartCoroutine(ReloadArmor());
        }
    }

    public IEnumerator ReloadArmor()
    {
        while (m_Armor != m_MaxArmor)
        {
            Debug.Log("Regenerating...");

            m_Armor += m_RegenAmount;
            armorBar.GainArmor(m_Armor, m_RegenAmount, m_MaxArmor);
            yield return new WaitForSeconds(m_ArmorRefreshRate);
        }

        //Debug.Log("Shield Regenerated");
        yield return null;
    }


}
