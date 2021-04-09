using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testvo : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<AudioManager_VoiceOver>().PlayVoiceOver();
        Debug.Log("RAWR");
    }
}
