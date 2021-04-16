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
            EventBroker.CallOnCurrencyPickup(worth, player.m_UpgradeCurrencyAmount);

            try { GetComponent<AudioManager_Universal>().Play(); }
            catch { }
            StartCoroutine(DelayDeactivate());
        }
    }

    IEnumerator DelayDeactivate()
    {
        yield return new WaitForSeconds(.1f);
        Destroy(gameObject);
    }
}
