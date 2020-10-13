using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    // Start is called before the first frame update
    [Tooltip("This list should contain every version of the mesh starting from least broken and ending in broken")]
    [SerializeField] GameObject[] DestructionStates;

    [Tooltip("This list should contain the time of how long you want a state to linger after being hit. The element of the timers line up with the elements of states")]
    [SerializeField] float[] Timers;

    [Tooltip("This list should contain the tags of things that can break the object")]
    [SerializeField] string[] Tags;

    [Tooltip("This variable should contain the amount of time that the last state lingers before disappearing")]
    [SerializeField] float deathtimer;

    int index = 0;
    GameObject currentstate;
    
    //TODO:: SET UP SAVING


    private void Start()
    {
        currentstate = DestructionStates[0];
    }

    public void Break(string tag)
    {
        //TODO: THIS DONT WORK
        if (Tags.Length != 0)
        {
            foreach (string t in Tags)
            {
                if (tag == t)
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

    void CycleState()
    {
        if (index < DestructionStates.Length - 1)
        {
            Destroy(currentstate.gameObject);
            currentstate = Instantiate(DestructionStates[index + 1], transform.position, transform.rotation, transform);
            index++;
           // Debug.Log(index.ToString());
        }

        if (index >= DestructionStates.Length - 1)
        {
            //start death 
            StartCoroutine(TriggerDeath());
        }
    }
    IEnumerator TriggerBreak()
    {
        if (index < Timers.Length)
        {
            yield return new WaitForSeconds(Timers[index]);
        }

        CycleState();
    }

    IEnumerator TriggerDeath()
    {
        yield return new WaitForSeconds(deathtimer);

        StartCoroutine(TriggerFadeOut());
    }

    IEnumerator TriggerFadeOut()
    {
        //TODO: This works but only if the shader on the object supports alpha
       // Debug.Log(currentstate);
        MeshRenderer[] rends = currentstate.GetComponentsInChildren<MeshRenderer>();
       // Debug.Log(rends.Length.ToString());
        foreach (MeshRenderer mshr in rends)
        {
            float fadeDurationInSeconds = 1f;
            float timeout = 0.05f;
            float fadeAmount = 1 / (fadeDurationInSeconds / timeout);

            for (float f = 1; f >= -0.05; f -= fadeAmount)
            {
                Color c = mshr.material.color;
                c.a = f;
                mshr.material.color = c;
                yield return new WaitForSeconds(timeout);
               // Debug.Log(mshr.material.color.ToString());
            }
        }

        gameObject.SetActive(false);
    }
}
