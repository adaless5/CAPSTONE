using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneProjectile : MonoBehaviour
{
    public float _bulletDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<ALTPlayerController>() != null || collision.gameObject.GetComponentInParent<Tool>() != null)
        {
            //Debug.Log("Player found");
            ALTPlayerController tempplayer = collision.gameObject.GetComponentInParent<ALTPlayerController>();
            if (tempplayer != null)
            {
                tempplayer.CallOnTakeDamage(_bulletDamage);
            }
                Destroy(this);
        }
        
            Destroy(gameObject);
        
    }
}
