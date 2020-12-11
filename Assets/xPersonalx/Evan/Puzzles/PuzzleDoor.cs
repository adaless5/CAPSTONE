using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoor : MonoBehaviour
{


    public enum Open_Type { Switch, Object, Proximity };
    public Open_Type _OpenType;

    public enum Close_Type { None, Switch, Object, Proximity };
    public Close_Type _CloseType;

    public bool _StartOpened;


    public GameObject _Player;
    public GameObject _OpenDoor;
    public GameObject _ClosedDoor;

    bool bIsOpen;
    bool bHasBeenClosed;
    bool bHasBeenOpened;

    public GameObject[] _OpenObjects;
    public PuzzleSwitch[] _OpenSwitches;
    public PuzzleProximityTrigger _OpenProximityTrigger;


    public GameObject[] _CloseObjects;
    public PuzzleSwitch[] _CloseSwitches;
    public PuzzleProximityTrigger _CloseProximityTrigger;

    // Start is called before the first frame update
    void Start()
    {
        if (bHasBeenOpened || bHasBeenClosed)
        {
            SetDoorOpen(bIsOpen);
        }
        else
        {
            SetDoorOpen(_StartOpened);
        }
    }

    void Open()
    {
        if (_OpenType == Open_Type.Object)
        {
            if (CheckObjectArray(_OpenObjects))
            {
                SetDoorOpen(true);
            }
        }

        else if (_OpenType == Open_Type.Switch)
        {
            if (CheckSwitches(_OpenSwitches))
            {
                SetDoorOpen(true);
            }
        }

        else if ( _OpenType == Open_Type.Proximity)
        {
                SetDoorOpen(CheckProximity(_OpenProximityTrigger));
        }

    }
    void Close()
    {
        if (bHasBeenClosed == false && _CloseType != Close_Type.None)
        {
            if (_CloseType == Close_Type.Object)
            {
                if (CheckObjectArray(_CloseObjects))
                {
                    SetDoorOpen(false);
                    bHasBeenClosed = true;
                }
            }

           else if (_CloseType == Close_Type.Switch)
            {
                if (CheckSwitches(_CloseSwitches))
                {
                    SetDoorOpen(false);
                    bHasBeenClosed = true;
                }
            }

           else if (_CloseType == Close_Type.Proximity)
            {
                if (_CloseProximityTrigger!=null && CheckProximity(_CloseProximityTrigger))
                {
                    SetDoorOpen(false);
                    bHasBeenClosed = true;
                }
            }

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

    // Update is called once per frame
    void Update()
    {
           Close();
           Open();
    }

    public void SetDoorOpen(bool OpenClosed)
    {
        if (OpenClosed == true)
        {
            if (_ClosedDoor != null)
            { _ClosedDoor.SetActive(false); }
            if (_OpenDoor != null)
            { _OpenDoor.SetActive(true); }
        }
        else if (OpenClosed == false)
        {
            if (_ClosedDoor != null)
            { _ClosedDoor.SetActive(true); }
            if (_OpenDoor != null)
            { _OpenDoor.SetActive(false); }
        }
    }
}
