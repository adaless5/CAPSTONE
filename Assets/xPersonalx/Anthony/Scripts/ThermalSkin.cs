using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalSkin : MonoBehaviour
{
    public Material m_ThermalViewMaterial;
    Material m_NormalViewMaterial;
    MeshRenderer m_MeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
        m_NormalViewMaterial = m_MeshRenderer.material; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToThermalSkin()
    {
        m_MeshRenderer.material = m_ThermalViewMaterial;
    }

    public void ChangeToNormalSkin()
    {
        m_MeshRenderer.material = m_NormalViewMaterial;
    }
}
