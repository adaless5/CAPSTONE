using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCreditTrack : MonoBehaviour
{
    public AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioManager_Ambiance>().StopAmbiance();

        _audioSource.PlayOneShot(_audioSource.clip);
    }
}
