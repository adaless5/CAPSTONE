using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_UpgradePickup : MonoBehaviour
{
    public int worth = 1;
    private void OnTriggerEnter(Collider other)
    {
        ALTPlayerController player = other.GetComponent<ALTPlayerController>();
        if(player)
        {
            player.m_UpgradeCurrencyAmount += worth;
            Destroy(gameObject);
        }
    }
}
