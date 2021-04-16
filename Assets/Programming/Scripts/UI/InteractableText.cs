using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractableText : MonoBehaviour
{
    //This class displays raycast info on HUD so player knows what they're looking at, anything you want display text to show up for must have a tag added
    //Anything that's interactable must have the "Interactable" tag

    public bool b_inInteractCollider;

    public ALTPlayerController m_player;
    public Camera m_playerCamera;
    public TMP_Text m_pickupText;
    public Image m_backgroundImage;
    public RectTransform m_transform;    
    float m_raycastRange;
    List<string> m_TagsToIgnore = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        m_player = FindObjectOfType<ALTPlayerController>();
        m_playerCamera = FindObjectOfType<Camera>();
        m_pickupText = GetComponentInChildren<TextMeshProUGUI>();
        m_backgroundImage = GetComponent<Image>();
        m_transform = GetComponent<RectTransform>();
        m_backgroundImage.enabled = false;
        m_pickupText.text = "";
        m_raycastRange = 50f;

        //List of Tags to Ignore for Interactable Text
        m_TagsToIgnore.Add("Player");
        m_TagsToIgnore.Add("Untagged");
        m_TagsToIgnore.Add("Enemy");
        m_TagsToIgnore.Add("Player_Blade");
        m_TagsToIgnore.Add("Roamer");
        m_TagsToIgnore.Add("LeaperExtras");
    }

    // Update is called once per frame
    void Update()
    {
        OnRaycastHit();
    }

    void OnRaycastHit()
    {
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(m_playerCamera.transform.position, m_playerCamera.transform.forward, out hitInfo, m_raycastRange))
        {
            //if (hitInfo.collider.gameObject.tag != "Untagged" && hitInfo.collider.gameObject.tag != "Player") 
            if(ShouldIgnoreTag(hitInfo.collider.gameObject.tag) == false)
            {
                m_backgroundImage.enabled = true;
                if (b_inInteractCollider)
                {
                    m_backgroundImage.rectTransform.sizeDelta = new Vector2(220, 20);
                    m_pickupText.rectTransform.sizeDelta = new Vector2(200, 15);
                    if (m_player.m_ControllerType == ControllerType.Controller)
                    { m_pickupText.text = "Press 'X' to Interact"; }
                    else
                    { m_pickupText.text = "Press 'E' to Interact"; }
                    b_inInteractCollider = false;
                }
                else if (ALTPlayerController.instance._InInteractionVolume == false && !b_inInteractCollider)
                {
                    m_backgroundImage.rectTransform.sizeDelta = new Vector2(150, 20);
                    m_pickupText.rectTransform.sizeDelta = new Vector2(146, 15);
                    m_pickupText.text = hitInfo.collider.gameObject.tag;
                }               
            }
            else
            {
                m_backgroundImage.enabled = false;
                m_pickupText.text = "";
            }          
        }
    }

    bool ShouldIgnoreTag(string tagName)
    {      
        foreach(string tag in m_TagsToIgnore)
        {
            if (tag == tagName)
            return true;            
        }
        return false;
    }
}
