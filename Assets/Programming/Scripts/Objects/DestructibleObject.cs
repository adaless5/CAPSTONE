using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestructibleObject : MonoBehaviour, ISaveable
{
    bool bDebug = false;

    [Tooltip("This list should contain every version of the mesh starting from least broken and ending in broken")]
    [SerializeField] GameObject[] DestructionStates = { };

    [Tooltip("This list should contain the time of how long you want a state to linger after being hit. The element of the timers line up with the elements of states")]
    [SerializeField] float[] Timers = { };

    [Tooltip("This list should contain the tags of things that can break the object")]
    [SerializeField] string[] Tags = { };

    [Tooltip("This variable should contain the amount of time that the last state lingers before disappearing")]
    [SerializeField] float deathtimer = 0;

    int _index = 0;
    bool _bisDead = false;
    GameObject _currentstate;

    //TODO:GET FADE OUT WORKING

    private void Start()
    {
        _currentstate = DestructionStates[0];
        //isDead = false;
    }

    private void Awake()
    {
        LoadDataOnSceneEnter();

        if(bDebug)Debug.Log(_bisDead);

        _currentstate = DestructionStates[0];
        if (_bisDead == true)
        {
            Collider[] col = _currentstate.GetComponentsInChildren<Collider>();
            MeshRenderer[] mesh = _currentstate.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer r in mesh)
            {
                r.enabled = false;
            }
            foreach (Collider c in col)
            {
                c.enabled = false;
            }
        }
    }

    public void LoadDataOnSceneEnter()
    {
        _bisDead = SaveSystem.LoadBool(gameObject.name, "_bisDead", gameObject.scene.name);
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
        if (_index < DestructionStates.Length - 1)
        {
            if (_currentstate == gameObject)
            {
                gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
                gameObject.GetComponentInChildren<Collider>().enabled = false;
            }
            else
            {
                Destroy(_currentstate.gameObject);
            }
            _currentstate = Instantiate(DestructionStates[_index + 1], transform.position, transform.rotation, transform);
            _index++;
        }

        if (_index >= DestructionStates.Length - 1)
        {
            //start death 
            StartCoroutine(TriggerDeath());
        }
    }
    IEnumerator TriggerBreak()
    {
        if (_index < Timers.Length)
        {
            yield return new WaitForSeconds(Timers[_index]);
        }

        CycleState();
    }

    IEnumerator TriggerDeath()
    {
        yield return new WaitForSeconds(deathtimer);

        _bisDead = true;
        SaveSystem.Save(gameObject.name, "_bisDead", gameObject.scene.name, _bisDead);

        Collider[] col = _currentstate.GetComponentsInChildren<Collider>();
        MeshRenderer[] mesh = _currentstate.GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer r in mesh)
        {
             r.enabled = false;
        }
        foreach (Collider c in col)
        {
            c.enabled = false;
        }
        // gameObject.SetActive(false);
    }

}
