using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoProjectorScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Hologram;
    public GameObject Beam_1;
    public GameObject Beam_2;
    private GameObject m_Player;

    void Start()
    {

        EventBroker.OnPlayerSpawned += PlayerSpawn;
    }
    void Awake()
    {
        EventBroker.OnPlayerSpawned += PlayerSpawn;

    }

    void PlayerSpawn(GameObject playerRef)
    {
        m_Player = GameObject.Find("Player_Camera");
    }

    // Update is called once per frame
    void Update()
    {
        //if (m_Player == null)
        //{
        //    m_Player = GameObject.Find("Player_Camera");
        //}

        if (m_Player != null)
        {
            if (Hologram != null)

            {
                Hologram.transform.LookAt(m_Player.transform);
                Hologram.transform.Rotate(90.0f, 0.0f, 0.0f);
            }
                if(Beam_1 != null && Beam_1 != null)
            {
                Beam_1.transform.Rotate(0.0f, 0.0f, 2.0f);
                Beam_2.transform.Rotate(0.0f, 0.0f, 3.5f);
            }
        }
        else
            m_Player = GameObject.Find("Player_Camera");
    }
}
