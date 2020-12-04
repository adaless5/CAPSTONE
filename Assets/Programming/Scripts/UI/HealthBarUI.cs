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
        healthFill.transform.localScale -= new Vector3(damagePercent, damagePercent, damagePercent);        
       // slider.value = HP;
    } 

    public void GainHealth(float HP, float healAmount, float maxHP)
    {
        float healPercent = healAmount / maxHP;
        healthFill.transform.localScale += new Vector3(healPercent, healPercent, healPercent);
    }
}
