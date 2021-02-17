using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_PORTAL : MonoBehaviour
{
    public GameObject Target;
    public ParticleSystem Teleparticles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider Other)
    {
        if(Other.tag == "Player" && Target != null)
        {
            Transform playerTransform = Other.GetComponentInParent<Transform>();

            //In Same Scene
            Vector3 newpos = Target.transform.position;
            Vector3 newrot = Target.transform.eulerAngles;
                playerTransform.position = newpos;
            Teleparticles.Play();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
