using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawner : MonoBehaviour
{
    public float Force = 20f;
    public float CoolDown = 5f;
    public GameObject minePrefab;
    public PlayerController m_playerController;

    bool m_bCanThrow = true;
    float timer;
    private void Start()
    {
        timer = CoolDown;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_playerController.CheckForMineInput())
        {
            if (m_bCanThrow)
            {
                ThrowMine();
                m_bCanThrow = false;
                timer = CoolDown;
                Debug.Log("THROWN");
            }
        }

        if(!m_bCanThrow)
        {
            timer -= Time.deltaTime;
            Debug.Log("COOLDOWN");

            if (timer <= 0f)
            {
                m_bCanThrow = true;
                Debug.Log("RESET");

            }
        }
    }

    void ThrowMine()
    {
        GameObject mine = Instantiate(minePrefab, transform.position, transform.rotation);
        if(mine)
        {
            mine.GetComponent<Rigidbody>().AddForce(transform.forward * Force);
        }
    }
}
