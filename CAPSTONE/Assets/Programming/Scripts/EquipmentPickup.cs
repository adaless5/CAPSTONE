using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPickup : MonoBehaviour
{
    [SerializeField] int _CorrespondingEquipmentBeltIndex = 0;

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Belt belt = other.gameObject.GetComponentInChildren<Belt>();
            belt.ObtainEquipmentAtIndex(_CorrespondingEquipmentBeltIndex);
            Destroy(gameObject);
        }
    }
}
