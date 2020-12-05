using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDoor : MonoBehaviour
{

    public bool bTask_Completed;

    public bool bHasShutTrigger;
   public bool bHasShut;

    public enum Task_Type { Switch, EnemyArray, Proximity};
    public Task_Type _Door_Type;
    public GameObject _Player;
    public GameObject _OpenDoor;
    public GameObject _ClosedDoor;

    public GameObject[] _Enemy_Array;
    public PuzzleSwitch[] _DoorSwitches;
    public GameObject _ProximityTrigger;

    // Start is called before the first frame update
    void Start()
    {
        _Player = GameObject.FindWithTag("Player");
        SetDoorOpen(bTask_Completed);
        if(bHasShutTrigger)
        {

            SetDoorOpen(bHasShut);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_Door_Type == Task_Type.EnemyArray)
        {
            CheckEnemyArray();
        }
        if(_Door_Type == Task_Type.Switch)
        {
            CheckSwitches();
        }
        if (_Door_Type == Task_Type.Proximity)
        {
            CheckProximity();
        }
    }

    void CheckEnemyArray()
    {
        if (!bTask_Completed)
        {
            if(bHasShutTrigger && !bHasShut)
            {
                if (_ProximityTrigger != null)
                {
                    TaskDoorProximityTrigger trigger = _ProximityTrigger.GetComponent<TaskDoorProximityTrigger>();
                    if (trigger != null)
                    {
                        SetDoorOpen(!trigger.bIsTriggered);
                        bHasShut = trigger.bIsTriggered;
                    }
                }
            }

            bool AllDead = true;
            for (int i = 0; i < _Enemy_Array.Length; i++)
            {
                if (_Enemy_Array[i].activeSelf == true)
                {
                    AllDead = false;
                }
            }
            if(AllDead)
            { SetDoorOpen(AllDead); bTask_Completed = AllDead; }
        }
    }
    void CheckSwitches()
    {
       // if (!bTask_Completed) // DELETE if you want door to stay open even after you flick a switch back off
        {
            bool AllSwitchesOn = true;
            for (int i = 0; i < _DoorSwitches.Length; i++)
            {
                if (!_DoorSwitches[i].GetIsActive())
                {
                    AllSwitchesOn = false;
                }
            }
            
             SetDoorOpen(AllSwitchesOn);
            bTask_Completed = AllSwitchesOn;
        }
    }
    void CheckProximity()
    {

        if (_ProximityTrigger != null)
        {
            TaskDoorProximityTrigger trigger = _ProximityTrigger.GetComponent<TaskDoorProximityTrigger>();
            if (trigger != null)
            {
                SetDoorOpen(trigger.bIsTriggered);
            }
        }
        
    }
    public void SetDoorOpen(bool OpenClosed)
    {
        
        if (_OpenDoor != null && _ClosedDoor != null)
        {
            if (OpenClosed)
            {
                _ClosedDoor.SetActive(false);
                _OpenDoor.SetActive(true);
            }
            else

            {
                _ClosedDoor.SetActive(true);
                _OpenDoor.SetActive(false);
            }
        }
    }

}
