using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultLight : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject Lamp;
    public GameObject Armor;

    public float floatingOffset;
    Vector3 OriginalPosition;
    void Start()
    {
        floatingOffset = Random.Range(0.0f, 6.32f);
        OriginalPosition = transform.position;
        Armor.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f,90.0f));
        Lamp.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 90.0f));
    }

    void Float()
    {
        transform.position = new Vector3(OriginalPosition.x,OriginalPosition.y + (2  * Mathf.Sin(floatingOffset)), OriginalPosition.z);
        floatingOffset += Time.deltaTime;
        if(floatingOffset>6.32f)
        {
            floatingOffset = 0.0f;
        }
    }

    void RotateArmor()
    {
        Armor.transform.Rotate(0.0f, 0.0f, 10.0f * Time.deltaTime);
        Lamp.transform.Rotate(0.0f, 0.0f, -10.0f * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        Float();
        RotateArmor();
    }
}
