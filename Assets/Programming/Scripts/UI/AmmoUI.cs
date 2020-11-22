using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{    
    public static int m_overallAmmo;
    public static int m_currentAmmo;
    Text m_text;

    // Start is called before the first frame update
    void Awake()
    {        
        m_text = GetComponent<Text>();       
    }  

    public void SetAmmoText(int currentAmmo, int overallAmmo)
    {
        if(m_text != null)
        m_text.text = "" + currentAmmo + "\n\n" + overallAmmo;
    }
}
