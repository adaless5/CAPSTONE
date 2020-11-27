using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_EnemyDamage : MonoBehaviour
{
    public int health = 100;
    // Start is called before the first frame update
 

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Blade blade = other.GetComponent<Blade>();
        if(blade)
        {
            //health -= blade.GetDamage();
        }
    }
}
