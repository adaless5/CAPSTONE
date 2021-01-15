using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{    
    public int m_overallAmmo;
    public int m_currentAmmo;
    Text m_text;

    // Start is called before the first frame update
    void Awake()
    {        
        m_text = GetComponent<Text>();
        
    }  

    public void SetAmmoText(int currentAmmo, int overallAmmo, int clipSize)
    {
        if (m_text != null)
        {
            if(clipSize < 10 && overallAmmo < 10)
            {                
                m_text.text = "0" + currentAmmo + "/0" + clipSize + "\n+000" + overallAmmo;
            }
            else if(clipSize < 10 && overallAmmo < 100)
            {
                m_text.text = "0" + currentAmmo + "/0" + clipSize + "\n+00" + overallAmmo;
            }
            else if(clipSize > 10 && overallAmmo < 10)
            {
                m_text.text = "" + currentAmmo + "/" + clipSize + "\n+000" + overallAmmo;
            }
            else if(clipSize < 10 && overallAmmo > 100)
            {
                m_text.text = "0" + currentAmmo + "/0" + clipSize + "\n+0" + overallAmmo;
            }
            else
            {
                m_text.text = "" + currentAmmo + "/" + clipSize + "\n+00" + overallAmmo;
            }
        }
    }

}
