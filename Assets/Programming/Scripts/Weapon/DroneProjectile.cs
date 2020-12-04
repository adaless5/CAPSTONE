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
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("Player found");
            ALTPlayerController tempplayer = collision.gameObject.GetComponent<ALTPlayerController>();
            if (tempplayer != null)
            {
                tempplayer.CallOnTakeDamage(_bulletDamage);
                Destroy(this);
            }
        }
        else
        {
            //Debug.Log("nah");
            Destroy(gameObject);
        }
    }
}
