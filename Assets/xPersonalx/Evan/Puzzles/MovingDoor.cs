using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDoor : MonoBehaviour//, ISaveable
{
    // Start is called before the first frame update

    public float DoorSpeed = 1.0f;
    public enum Open_Type { None, Switch, Object, Proximity };
    public Open_Type _OpenType;

    public enum Close_Type { None, Switch, Object, Proximity };
    public Close_Type _CloseType;

    public bool _StartOpened;

    public bool _StayClosed;
    public bool _StayOpen;

    public GameObject[] _Doors;
    public GameObject[] _OpenPositions;
    public GameObject[] _ClosedPositions;



    public bool bIsOpen;
    public bool bHasBeenClosed;
    public bool bHasBeenOpened;

    public PuzzleSwitch[] _Switches;

    public GameObject[] _OpenObjects;
    public PuzzleProximityTrigger _OpenProximityTrigger;

    public GameObject[] _CloseObjects;
    public PuzzleProximityTrigger _CloseProximityTrigger;
    void Start()
    {
        bIsOpen = _StartOpened;
        bHasBeenClosed = false;
        bHasBeenOpened = false;
        SetDoorPositions(bIsOpen);
    }

    // Update is called once per frame

    void SetDoorPositions(bool OpenClosed)
    {

        for (int i = 0; i < _Doors.Length; i++)
        {
            if (OpenClosed)
            {
                _Doors[i].transform.position = _OpenPositions[i].transform.position;
                bHasBeenOpened = true;
            }
            else
            {
                _Doors[i].transform.position = _ClosedPositions[i].transform.position;
                bHasBeenClosed = true;
            }

        }
    }
    void Open()
    {
        bHasBeenClosed = false;
        bool AllDoorsReachedOpen = true;
        for (int i = 0; i < _Doors.Length; i++)
        {
            _Doors[i].transform.position = Vector3.MoveTowards(_Doors[i].transform.position, _OpenPositions[i].transform.position, DoorSpeed * Time.deltaTime);
            if (_Doors[i].transform.position != _OpenPositions[i].transform.position)
            {
                AllDoorsReachedOpen = false;
            }
        }
        bHasBeenOpened = AllDoorsReachedOpen;
    }
    void Close()
    {
        bHasBeenOpened = false;
        bool AllDoorsReachedClosed = true;
        for (int i = 0; i < _Doors.Length; i++)
        {

            _Doors[i].transform.position = Vector3.MoveTowards(_Doors[i].transform.position, _ClosedPositions[i].transform.position, DoorSpeed * Time.deltaTime);
            if (_Doors[i].transform.position != _ClosedPositions[i].transform.position)
            {
                AllDoorsReachedClosed = false;
            }
        }
        bHasBeenClosed = AllDoorsReachedClosed;

    }
    void Update()
    {
        if (!bHasBeenOpened)
        {
            CheckOpenDoors();
        }
        if (!bHasBeenClosed)
        {
            CheckCloseDoors();
        }
        OpenCloseDoors();
        ProximityOpenClose();
    }
    // public void SaveDataOnSceneChange()
    // {
    //     SaveSystem.Save(gameObject.name, "bIsOpen", gameObject.scene.name, bIsOpen);
    //     SaveSystem.Save(gameObject.name, "bHasBeenClosed", gameObject.scene.name, bHasBeenClosed);
    //     SaveSystem.Save(gameObject.name, "bHasBeenOpened", gameObject.scene.name, bHasBeenOpened);
    //
    // }

    // public void LoadDataOnSceneEnter()
    // {
    //     bIsOpen = SaveSystem.LoadBool(gameObject.name, "bIsOpen", gameObject.scene.name);
    //     bHasBeenClosed = SaveSystem.LoadBool(gameObject.name, "bHasBeenClosed", gameObject.scene.name);
    //     bHasBeenOpened = SaveSystem.LoadBool(gameObject.name, "bHasBeenOpened", gameObject.scene.name);
    //
    // }
    void OpenCloseDoors()
    {
        if (bIsOpen && bHasBeenOpened == false)
        {
            Open();
        }
        else if (!bIsOpen && bHasBeenClosed == false)
        {
            Close();
        }
    }
    bool CheckObjectArray(GameObject[] objects)
    {
        bool AllDead = true;
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null)
            {
                if (objects[i].activeSelf == true)
                {
                    AllDead = false;
                }
            }
        }
        return AllDead;

    }

    bool CheckSwitches(PuzzleSwitch[] switches)
    {

        bool AllSwitchesOn = true;
        for (int i = 0; i < switches.Length; i++)
        {
            if (switches[i] != null)
            {
                if (!switches[i].GetIsActive())
                {
                    AllSwitchesOn = false;
                }
            }
        }
        return AllSwitchesOn;
    }


    bool CheckProximity(PuzzleProximityTrigger trigger)
    {
        if (trigger != null)
        {
            return trigger.bIsTriggered;
        }
        else
        {
            return false;
        }
    }
    void CheckOpenDoors()
    {
        if (_OpenType == Open_Type.Object)
        {

            bIsOpen = CheckObjectArray(_OpenObjects);

        }

        else if (_OpenType == Open_Type.Switch)
        {

            bIsOpen = CheckSwitches(_Switches);

        }

    }
    void CheckCloseDoors()
    {

        if (_CloseType == Close_Type.Object)
        {

            bIsOpen = CheckObjectArray(_CloseObjects);

        }

        else if (_CloseType == Close_Type.Switch)
        {

            bIsOpen = !CheckSwitches(_Switches);

        }



    }
    void ProximityOpenClose()
    {
        if (_CloseType == Close_Type.Proximity)
        {
            if (_StayClosed)
            {
                if (CheckProximity(_CloseProximityTrigger) == true)
                { bIsOpen = false; }
            }
            else
            {
                bIsOpen = !CheckProximity(_CloseProximityTrigger);
            }
        }
        if (_OpenType == Open_Type.Proximity)
        {
            if (_StayOpen)
            {
                if (CheckProximity(_OpenProximityTrigger) == true)
                { bIsOpen = true; }
            }
            else
            {
                bIsOpen = CheckProximity(_OpenProximityTrigger);
            }
        }
    }
}
