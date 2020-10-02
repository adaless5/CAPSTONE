using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    // Start is called before the first frame update
       public Rigidbody[] kids;
    void Start()
    {
        kids = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody obj in kids)
        { 
            if (obj.GetComponent<Rigidbody>())
            {
                obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Blowdedup()
    {
        foreach (Rigidbody obj in kids)
        {
            if (obj.GetComponent<Rigidbody>())
            {
                obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
        }

        Invoke("Suicide", 5f);
    }

    void Suicide()
    {
        Destroy(gameObject);
    }
}
