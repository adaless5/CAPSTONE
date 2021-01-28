using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    bool bDebug = false;

    //[Header("Stamina Settings")]
    [SerializeField]
    private float m_stamina;    
    [SerializeField]
    private float m_maxStamina;
    [SerializeField]
    private float m_energyAmount;    
    [SerializeField]
    private float m_regenRate;

    private float m_timerMax;
    private float m_staminaTimer;

    public StaminaUI staminaBar;
    private bool bIsRegenerating;
    public bool bCanRegenerate;

    // Start is called before the first frame update
    void Start()
    {
        //stamina settings
        m_maxStamina = 100f;
        m_stamina = m_maxStamina;
        m_energyAmount = 2f;       
        m_regenRate = 0.25f;       
        bIsRegenerating = false;
        bCanRegenerate = false;
        m_timerMax = 0.15f;
        m_staminaTimer = m_timerMax;

        //stamina UI        
        staminaBar = FindObjectOfType<StaminaUI>();
        if (staminaBar != null)
            staminaBar.SetMaxStamina(m_maxStamina);
    }

    void Update()
    {
        
    }

    public float GetCurrentStamina()
    {
        return m_stamina;
    }

    public void UseStamina()
    {
        bIsRegenerating = false;
        bCanRegenerate = true;
        
        if (m_stamina > 0)
        {
            
            m_staminaTimer -= Time.deltaTime;
            if (m_staminaTimer <= 0)
            {
                m_stamina -= m_energyAmount;
                m_staminaTimer = m_timerMax;

                if (staminaBar != null)
                    staminaBar.SetStamina(m_stamina);              
            }        

        }
        m_stamina = Mathf.Clamp(m_stamina, 0, m_maxStamina);
    }

    public IEnumerator RegenerateStamina()
    {
        if(bIsRegenerating == false)
        {
            bIsRegenerating = true;
            while (m_stamina < m_maxStamina && bIsRegenerating == true)
            {
                if (bDebug) Debug.Log("Regenerating...");

                m_stamina += m_energyAmount;               
                yield return new WaitForSeconds(m_regenRate);

                if (staminaBar != null)
                    staminaBar.SetStamina(m_stamina);

                m_stamina = Mathf.Clamp(m_stamina, 0, m_maxStamina);
            }

            bIsRegenerating = false;
            bCanRegenerate = true;           
            yield return null;
        }
    }
}
