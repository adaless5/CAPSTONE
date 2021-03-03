using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalSkin : MonoBehaviour
{
    public Material m_ThermalViewMaterial;
    Material m_NormalViewMaterial;
    public MeshRenderer[] m_MeshRenderers;
    public MeshRenderer m_MeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            m_MeshRenderer = GetComponent<MeshRenderer>();
            m_NormalViewMaterial = m_MeshRenderer.material;
        }

        if (GetComponentsInChildren<MeshRenderer>() != null)
        {
            m_MeshRenderers = GetComponentsInChildren<MeshRenderer>();
        }




    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeToThermalSkin()
    {
        foreach (MeshRenderer m in m_MeshRenderers)
        {
            m.material = m_ThermalViewMaterial;
        }
        m_MeshRenderer.material = m_ThermalViewMaterial;
    }

    public void ChangeToNormalSkin()
    {
        foreach (MeshRenderer m in m_MeshRenderers)
        {
            m.material = m_NormalViewMaterial;
        }
        m_MeshRenderer.material = m_NormalViewMaterial;
    }
}
