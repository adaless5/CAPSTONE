﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleProximityTrigger : MonoBehaviour
{
    public bool bIsTriggered;

    void Start()
    {
        bIsTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bIsTriggered = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bIsTriggered = false;
        }
    }
}
