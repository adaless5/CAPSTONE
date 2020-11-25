using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    //[Header("Stamina Settings")]
    [SerializeField]
    private float m_stamina;    
    [SerializeField]
    private float m_maxStamina;
    [SerializeField]
    private float m_energyLossAmount;
    [SerializeField]
    private float m_energyRegenAmount;
    [SerializeField]
    private float m_regenRate;
    [SerializeField]
    private float m_staminaTimer;

    // Start is called before the first frame update
    void Start()
    {
        m_stamina = 100f;
        m_energyLossAmount = 5f;
        m_energyRegenAmount = 15f;
        m_maxStamina = 100f;
        m_regenRate = 0.3f;
        m_staminaTimer = 3f;
    }
    void Update()
    {
        if (m_stamina < m_maxStamina)
        {
            m_staminaTimer -= Time.deltaTime;
        }

        if (m_staminaTimer <= 0)
        {            
            StartCoroutine(RegenerateStamina());
        }
    }

    public float GetCurrentStamina()
    {
        return m_stamina;
    }

    public void UseStamina()
    {
        if(m_stamina > 0)
        {
            m_stamina -= m_energyLossAmount;
        }
        Debug.Log("Losing Stamina. Stamina at " + m_stamina);
    }

    public IEnumerator RegenerateStamina()
    {
        while (m_stamina != m_maxStamina)
        {
            Debug.Log("Regenerating...");

            m_stamina += m_energyRegenAmount;            
            yield return new WaitForSeconds(m_regenRate);
        }

        Debug.Log("Stamina Regenerated");
        yield return null;
    }
}
