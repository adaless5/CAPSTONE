using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public RawImage m_compassDirections;
    public ALTPlayerController m_player;
    public Image m_background;
    public CompassMarkers m_marker;

    //new
    public GameObject m_iconPrefab;
    List<CompassMarkers> m_compassMarkers = new List<CompassMarkers>();
    float m_compassUnit;

    [Header("Visibility Distance")]
    [Tooltip("Maximum distance between player and marker to appear on compass")]
    public float m_maxDistance = 75f;

    private void Awake()
    {
        m_player = FindObjectOfType<ALTPlayerController>();
        m_compassDirections = GetComponentInChildren<RawImage>();
        m_background = GetComponentInChildren<Image>();
    }

    void Start()
    {
        m_compassDirections.texture = Resources.Load<Texture>("Sprites/HUD/Compass_Directions");
        m_background.sprite = Resources.Load<Sprite>("Sprites/HUD/Compass_Background");
        m_compassUnit = m_compassDirections.rectTransform.rect.width / 360f;
    }

    // Update is called once per frame
    void Update()
    {
        m_compassDirections.uvRect = new Rect(m_player.transform.localEulerAngles.y / 360, 0, 1, 1);
        Vector3 m_forwardDir = m_player.transform.forward;
        m_forwardDir.y = 0;

        float m_playerAngle = Quaternion.LookRotation(m_forwardDir).eulerAngles.y;
        m_playerAngle = 5 * (Mathf.RoundToInt(m_playerAngle / 5.0f));

        int m_compassAngle;
        m_compassAngle = Mathf.RoundToInt(m_playerAngle);

        foreach (CompassMarkers marker in m_compassMarkers)
        {
            marker.m_markerImage.rectTransform.anchoredPosition = GetPosOnCompass(marker);

            //Setting Distance checks between marked item and player for whether to appear on compass
            float distance = Vector2.Distance(new Vector2(m_player.transform.position.x, m_player.transform.position.z), marker.m_markerPosition);
            float scale = 0f;
            if (distance < m_maxDistance)
            {
                scale = 1f - (distance / m_maxDistance);
            }
            marker.m_markerImage.rectTransform.localScale = Vector3.one * scale;       

        }
    }

    public void AddMarker(CompassMarkers marker)
    {       
            GameObject newMarker = Instantiate(m_iconPrefab, m_compassDirections.transform);            
            marker.m_markerImage = newMarker.GetComponent<Image>();
            marker.m_markerImage.sprite = marker.icon;
            m_compassMarkers.Add(marker);           
    }

    Vector2 GetPosOnCompass(CompassMarkers marker)
    {
        Vector2 playerPos = new Vector2(m_player.transform.position.x, m_player.transform.position.z);
        Vector2 playerForward = new Vector2(m_player.transform.forward.x, m_player.transform.forward.z);

        float angle = Vector2.SignedAngle(marker.m_markerPosition - playerPos, playerForward);

        return new Vector2(m_compassUnit * angle, 0f);
    }

    public void RemoveMarker(CompassMarkers marker)
    {
        marker.m_markerImage.enabled = false;
        m_compassMarkers.Remove(marker);
    }

}
