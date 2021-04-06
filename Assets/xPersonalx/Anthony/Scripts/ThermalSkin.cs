using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalSkin : MonoBehaviour, ISaveable
{
    public Material m_ThermalViewMaterial;
    List<Material> m_NormalViewMaterials;
    List<Material> m_skinnedNormalViewMaterials;
    public MeshRenderer[] m_MeshRenderers;
    public MeshRenderer m_MeshRenderer;
    public SkinnedMeshRenderer[] m_skinnedMeshRenderers;
    public SkinnedMeshRenderer m_skinnedMeshRenderer;
    ThermalEquipment m_ThermalEquipment;
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
        if (GetComponentsInChildren<SkinnedMeshRenderer>() != null)
        {
            m_skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            m_skinnedNormalViewMaterials = new List<Material>();

            foreach (SkinnedMeshRenderer m in m_skinnedMeshRenderers)
            {
                m_skinnedNormalViewMaterials.Add(m.material);
            }
        }
    }

    private void Awake()
    {
        m_ThermalEquipment = GameObject.FindObjectOfType<ThermalEquipment>();
        LoadDataOnSceneEnter();
        if (m_ThermalEquipment!= null)
        {
            if(m_ThermalEquipment.bIsActive)
            {
                ChangeToThermalSkin();
            }
            else
            {
                ChangeToNormalSkin();
            }
        }

    }
    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeToThermalSkin()
    {
        foreach (SkinnedMeshRenderer m in m_skinnedMeshRenderers)
        {
            m.material = m_ThermalViewMaterial;
        }
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
        int j = 0;
        foreach (SkinnedMeshRenderer m in m_skinnedMeshRenderers)
        {
            m.material = m_skinnedNormalViewMaterials[j];
            j++;
        }
    }

    public void LoadDataOnSceneEnter()
    {
        m_ThermalEquipment = GameObject.FindObjectOfType<ThermalEquipment>();
        if (m_ThermalEquipment != null)
        {
            if (m_ThermalEquipment.bIsActive)
            {
                ChangeToThermalSkin();
            }
            else
            {
                ChangeToNormalSkin();
            }
        }
    }
}
