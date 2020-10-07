using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    // Start is called before the first frame update
    [Tooltip("This list should contain every version of the mesh starting from least broken and ending in broken")]
    [SerializeField] GameObject[] DestructionStates;

    float timer = 3f;
    int index = 0;


    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            timer = 3f;
            if (index >= DestructionStates.Length - 1)
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.Log(index.ToString());
                GameObject obj = Instantiate<GameObject>(DestructionStates[index + 1], transform.position, transform.rotation, transform);
                Instantiate(DestructionStates[index + 1], transform.position, transform.rotation, transform);
                //obj.SetActive(false);
                DestructionStates[index].gameObject.SetActive(false);
            }

            index++;
        }
    }
}
