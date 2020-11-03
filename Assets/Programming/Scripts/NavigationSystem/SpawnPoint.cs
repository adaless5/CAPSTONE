using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        //Draw Spawn Point and direction line.
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, .5f);
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 2));
        //
    }
}
