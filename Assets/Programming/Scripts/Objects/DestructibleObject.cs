using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour, ISaveable
{
    [Tooltip("This list should contain every version of the mesh starting from least broken and ending in broken")]
    [SerializeField] GameObject[] DestructionStates;

    [Tooltip("This list should contain the time of how long you want a state to linger after being hit. The element of the timers line up with the elements of states")]
    [SerializeField] float[] Timers;

    [Tooltip("This list should contain the tags of things that can break the object")]
    [SerializeField] string[] Tags;

    [Tooltip("This variable should contain the amount of time that the last state lingers before disappearing")]
    [SerializeField] float deathtimer;

    int index = 0;
    bool isDead;
    GameObject currentstate;

    private void Start()
    {
        currentstate = DestructionStates[0];
        isDead = false;
    }

    private void Awake()
    {
        LoadDataOnSceneEnter();
        SaveSystem.SaveEvent += SaveDataOnSceneChange;


        Debug.Log(isDead);
        if (isDead == true)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveDataOnSceneChange()
    {
        SaveSystem.Save(gameObject.name, "isBroken", isDead);
    }

    public void LoadDataOnSceneEnter()
    {
        isDead = SaveSystem.LoadBool(gameObject.name, "isBroken");
    }

    public void OnDisable()
    {
        SaveSystem.SaveEvent -= SaveDataOnSceneChange;
    }

    public void Break(string tag)
    {
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

        isDead = true;
        Debug.Log(isDead);
        gameObject.SetActive(false);
    }

}
