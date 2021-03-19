using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavigationBaker : MonoBehaviour
{
    public NavMeshSurface[] surfaces;
    public Transform[] objectToRotate;
    // Start is called before the first frame update
    void Start()
    {
        for (int j = 0; j < objectToRotate.Length; j++)
        {
            objectToRotate[j].localRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
        }
        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
    }

}
