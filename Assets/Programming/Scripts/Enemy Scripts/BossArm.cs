using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArm : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ALTPlayerController player = other.GetComponent<ALTPlayerController>();
            if (player != null)
            {
                player.CallOnTakeDamage(50);
            }
        }
    }

    public void ShowArm()
    {
        GetComponent<MeshRenderer>().enabled = true;

    }

}
