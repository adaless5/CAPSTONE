using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCommsDevice : MonoBehaviour
{
    private void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<ALTPlayerController>())
        {
            if(other.GetComponent<ALTPlayerController>().CheckForInteract())
            {
                if(GetComponent<AudioManager_Universal>())
                    GetComponent<AudioManager_Universal>().Play();
            }
        }
    }
}
