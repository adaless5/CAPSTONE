using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassMarkers : MonoBehaviour
{
    [Header("Marker Icon")]
    [Tooltip("Set marker sprite that appears on compass")]
    public Sprite icon;
   
    public Image m_markerImage;  
    public Vector2 m_markerPosition;

    [Tooltip("Mark as true for enemies or movable markers")]
    public bool bIsMoving;



    void Start()
    {       
        m_markerPosition = new Vector2(transform.position.x, transform.position.z);        
    }

    private void Update()
    {
        if (bIsMoving)
        {
            m_markerPosition = new Vector2(transform.localPosition.x, transform.position.z);
        }
    }


}
