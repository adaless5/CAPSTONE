using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAmbianceOnStart : MonoBehaviour
{


    private void Update()
    {
        if (!GetComponent<AudioManager_Ambiance>().isPlaying())
        {
            GetComponent<AudioManager_Ambiance>().TriggerMenuAmbiance();
        }
    }
}
