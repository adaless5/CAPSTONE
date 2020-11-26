using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject Pickups;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //change this to be "if a gun or sword comes into contact with this (overlapping hit boxes?);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //create the ammo and health pickups
            Instantiate(Pickups, transform.position, transform.rotation);

            //destroy the item spawner the player broke
            //Destroy(gameObject);
            DestroyImmediate(gameObject);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
        
    //}
}