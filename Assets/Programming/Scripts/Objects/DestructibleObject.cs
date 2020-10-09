using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    // Start is called before the first frame update
    [Tooltip("This list should contain every version of the mesh starting from least broken and ending in broken")]
    [SerializeField] GameObject[] DestructionStates;

    [Tooltip("This list should contain the time of how long you want a state to linger after being hit. The element of the timers line up with the elements of states")]
    [SerializeField] int[] Timers;

    [Tooltip("This list should contain the tags of things that can break the object")]
    [SerializeField] string[] Tags;

    int index = 0;
    GameObject currentstate;


    //TODO: add a timer when it reaches final state to disable object

    private void Start()
    {
        currentstate = DestructionStates[0];
    }

    public void Break(GameObject obj)
    {
        if (Tags.Length != 0)
        {
            foreach (string t in Tags)
            {
                if (obj.tag == t)
                {
                    StartCoroutine(TriggerBreak());
                    break;
                }
            }
        }
        else
        {
            StartCoroutine(TriggerBreak());
        }
    }

    IEnumerator TriggerBreak()
    {
        //TODO: make timer only read the size of the array, so the timer array and object array can be different sizes
        if (Timers.Length != 0)
        {
            yield return new WaitForSeconds(Timers[index]);
        }

        CycleState();
    }

    void CycleState()
    { 
        if (index >= DestructionStates.Length - 1)
        {
            //TODO: add a fade away before deleting
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(currentstate.gameObject);
            Debug.Log(index.ToString());
            currentstate = Instantiate(DestructionStates[index + 1], transform.position, transform.rotation, transform);
            index++;
        }
    }
}
