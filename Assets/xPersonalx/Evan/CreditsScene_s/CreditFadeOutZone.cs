using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditFadeOutZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        CreditObject credit = other.gameObject.GetComponent<CreditObject>();
        if(credit!=null)
        {
            credit.StartFadeOut();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
