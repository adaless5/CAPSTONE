using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void SetMaxHealth(float HP)
    {
        slider.maxValue = HP;
        slider.value = HP;
    }

    public void SetHealth(float HP)
    {
        slider.value = HP;
    }

 
}
