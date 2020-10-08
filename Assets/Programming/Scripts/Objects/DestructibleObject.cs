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

    private void Start()
    {
        currentstate = DestructionStates[0];
    }

    // Update is called once per frame
    //void Update()
    //{
    //    timer -= Time.deltaTime;

    //    if (timer <= 0)
    //    {
    //        timer = 3f;
    //        if (index >= DestructionStates.Length - 1)
    //        {
    //            gameObject.SetActive(false);
    //        }
    //        else
    //        {
    //            Destroy(currentstate.gameObject);
    //            Debug.Log(index.ToString());
    //            currentstate = Instantiate(DestructionStates[index + 1], transform.position, transform.rotation, transform);
    //        }

    //        index++;
    //    }
    //}

    public void Break(GameObject obj)
    {
         Debug.Log("HIT");
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
        Debug.Log("WAITING");
        if (Timers.Length != 0)
        {
            yield return new WaitForSeconds(Timers[index]);
        }

        CycleState();
    }

    void CycleState()
    {
        Debug.Log("CYCLE");
        if (index >= DestructionStates.Length - 1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(currentstate.gameObject);
            Debug.Log(index.ToString());
            currentstate = Instantiate(DestructionStates[index + 1], transform.position, transform.rotation, transform);
        }
        index++;
    }
}
