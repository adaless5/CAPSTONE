using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampTransform : MonoBehaviour
{

    public Vector3 Position;
    public Quaternion Rotation;

    void Start()
    {
        Position = gameObject.transform.position;
        Rotation = gameObject.transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {

       gameObject.transform.position = Position;
       gameObject.transform.rotation = Rotation;
    }
}
 