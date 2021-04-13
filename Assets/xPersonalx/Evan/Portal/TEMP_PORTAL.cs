using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_PORTAL : MonoBehaviour
{
    public GameObject TeleportToPositionObject;
    public ParticleSystem LocalTeleparticles;
    public ParticleSystem TargetTeleparticles;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void MoveTarget(Transform targetTransform)
    {
        LocalTeleparticles.transform.position = targetTransform.position;
        LocalTeleparticles.Play();
        Vector3 newpos = TeleportToPositionObject.transform.position;
        targetTransform.position = newpos;
        TargetTeleparticles.Play();
    }
    void AboutToTeleportIndicator()
    {

    }

    void OnTriggerEnter(Collider Other)
    {
        if(TeleportToPositionObject != null)
        if(Other.tag == "Player" || Other.tag == "Enemy" || Other.tag == "Player_Mine")
        {
            Transform targetTransform = Other.GetComponentInParent<Transform>();
            MoveTarget(targetTransform);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
