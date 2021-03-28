using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorBarUI : MonoBehaviour
{
    //private float m_lowestScaleAmount = 0.30f;

    public Slider slider;
    float m_sliderSpeed = 3;
    public RectTransform armorFill;

    private void Awake()
    {        
        slider = GetComponent<Slider>();
    }

    public void SetMaxArmor(float armor)
    {
        slider.maxValue = armor;
        slider.value = armor;
    }

    public void SetCurrentArmor(float armor)
    {
        slider.value = armor;
    }

    //OLD ARMOR UI LOGIC, temporarily removed (sphere shrinking inwards)

    //public void LoseArmor(float armor, float damage, float maxArmor)
    //{
    //    float damagePercent = (damage / maxArmor) * m_lowestScaleAmount;
    //    armorFill.transform.localScale -= new Vector3(damagePercent, damagePercent, damagePercent);
    //    //slider.value = armor;
    //}

    //public void GainArmor(float armor, float regen, float maxArmor)
    //{
    //    float damagePercent = (regen / maxArmor) * m_lowestScaleAmount;
    //    armorFill.transform.localScale += new Vector3(damagePercent, damagePercent, damagePercent);
    //}

}
