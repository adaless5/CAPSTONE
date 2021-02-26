using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    //public Slider slider;
    public RectTransform healthFill;

    private void Awake()
    {
        // slider = GetComponent<Slider>();  
        
    }

    public void SetMaxHealth(float HP)
    {
       // slider.maxValue = HP;
        //slider.value = HP;
    }

    public void LoseHealth(float HP, float damage, float maxHP)
    {
        float damagePercent = damage / maxHP;
        
        if (healthFill.transform.localScale.x <= 1 && healthFill.transform.localScale.x > 0)
        {            

            healthFill.transform.localScale -= new Vector3(damagePercent, damagePercent, damagePercent);

            if(healthFill.transform.localScale.x < 0)
            healthFill.transform.localScale = new Vector3(0f, 0f, 0f);
                  
        }
        else
        {
            healthFill.transform.localScale = new Vector3(0f, 0f, 0f);
        }
       // slider.value = HP;
    } 

    public void GainHealth(float HP, float healAmount, float maxHP)
    {
        float healPercent = healAmount / maxHP;

        if(healthFill.transform.localScale.x < 1 && healthFill.transform.localScale.x >= 0)      
        healthFill.transform.localScale += new Vector3(healPercent, healPercent, healPercent);       
        
    }
}
