using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorBarUI : MonoBehaviour
{

    public Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void SetMaxArmor(float armor)
    {
        slider.maxValue = armor;
        slider.value = armor;
    }

    public void SetArmor(float armor)
    {
        slider.value = armor;
    }

}
