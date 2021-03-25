using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalSkin : MonoBehaviour
{
    public Material m_ThermalViewMaterial;
    List<Material> m_NormalViewMaterials;
    public MeshRenderer[] m_MeshRenderers;
    public MeshRenderer m_MeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentsInChildren<MeshRenderer>() != null)
        {
            m_MeshRenderers = GetComponentsInChildren<MeshRenderer>();

            m_NormalViewMaterials = new List<Material>();

            foreach (MeshRenderer m in m_MeshRenderers)
            {
                m_NormalViewMaterials.Add(m.material);
            }
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
    }

    public void ChangeToNormalSkin()
    {
        int i = 0;
        foreach (MeshRenderer m in m_MeshRenderers)
        {
            m.material = m_NormalViewMaterials[i];
            i++;
        }
    }
}
