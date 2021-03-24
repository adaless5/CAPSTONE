using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisorHitEffects : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] HealthHitImages;
    public GameObject[] ShieldHitImages;
    public GameObject[] ShieldBreakImages;

    ParticleSystem[] HealthHitParticles;
    ParticleSystem[] ShieldHitParticles;
    ParticleSystem[] ShieldBreakParticles;

    List<bool> bHealthHitImageActive = new List<bool>();
    List<bool> bShieldHitImageActive = new List<bool>();
    List<bool> bShieldBreakImageActive = new List<bool>();

    List<float> HealthHitImageAlpha = new List<float>();
    List<float> ShieldHitImageAlpha = new List<float>();
    List<float> ShieldBreakImageAlpha = new List<float>();

    void Start()
    {
        for (int i = 0; i < HealthHitImages.Length; i++)
        {
            HealthHitImageAlpha.Add(1.0f);
            bHealthHitImageActive.Add(true);
            Color col = HealthHitImages[i].GetComponent<MeshRenderer>().material.color;
            col.a = 0.0f;
            HealthHitImages[i].GetComponent<MeshRenderer>().material.color = col;
        }
        for (int i = 0; i < ShieldHitImages.Length; i++)
        {
            bShieldHitImageActive.Add(true);
            ShieldHitImageAlpha.Add(1.0f);
            Color col = ShieldHitImages[i].GetComponent<MeshRenderer>().material.color;
            col.a = 0.0f;
            ShieldHitImages[i].GetComponent<MeshRenderer>().material.color = col;
        }
        for (int i = 0; i < ShieldBreakImages.Length; i++)
        {
            bShieldBreakImageActive.Add(true);
            ShieldBreakImageAlpha.Add(1.0f);
            Color col = ShieldBreakImages[i].GetComponent<MeshRenderer>().material.color;
            col.a = 0.0f;
            ShieldBreakImages[i].GetComponent<MeshRenderer>().material.color = col;
        }

    }
    void ImageProcess(GameObject[] imageList, ref List<bool> activeList, ref List<float> alphaList)
    {
        for (int i = 0; i < imageList.Length; i++)
        {
            if (imageList[i] && activeList[i])
            {
                ImageFlash(imageList[i], ref activeList, ref alphaList, i);
            }
        }

    }
    void Hit(GameObject[] images, List<bool> activeList)
    {
        for (int i = 0; i < images.Length; i++)
        {
            int index = Random.Range(0, images.Length);
            if (!activeList[index])
            {
                activeList[index] = true;
                return;
            }
        }
    }
    public void HealthHit()
    {
        Hit(HealthHitImages, bHealthHitImageActive);
    }
    public void ShieldHit()
    {
        Hit(ShieldHitImages, bShieldHitImageActive);
    }
    public void ShieldBreak()
    {
        Hit(ShieldBreakImages, bShieldBreakImageActive);
    }
    void ImageFlash(GameObject image, ref List<bool> onOff, ref List<float> alpha, int index)
    {
        if (alpha[index] > 0.0f)
        {
            alpha[index] -= Time.deltaTime;
            Color col = image.GetComponent<MeshRenderer>().material.color;
            col.a = alpha[index];
            image.GetComponent<MeshRenderer>().material.color = col;
        }
        else
        {
            Color col = image.GetComponent<MeshRenderer>().material.color;
            col.a = 0.0f;
            image.GetComponent<MeshRenderer>().material.color = col;
            onOff[index] = false;
            alpha[index] = 1.0f;
        }

    }
    // Update is called once per frame
    void Update()
    { 
        ImageProcess(HealthHitImages, ref bHealthHitImageActive, ref HealthHitImageAlpha);
        ImageProcess(ShieldHitImages, ref bShieldHitImageActive, ref ShieldHitImageAlpha);
        ImageProcess(ShieldBreakImages, ref bShieldBreakImageActive, ref ShieldBreakImageAlpha);
    }
}
