using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider Other)
    {
        if(Other.tag == "Player")
        {
            Transform playerTransform = Other.GetComponentInParent<Transform>();

            //In Same Scene
            Vector3 newpos = new Vector3(166.3426f, 8.002193f, 515.1062f);
            Quaternion newrot =  Quaternion.Euler(0.0f,0.0f, 68.143f);
                playerTransform.position = newpos;
                playerTransform.rotation = newrot;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
