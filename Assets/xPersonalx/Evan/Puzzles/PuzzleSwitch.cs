using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSwitch : MonoBehaviour
{
    public bool _DoesSwitchReset;
    public float _ActivationTimer;// the length of time the player has to wait to interact with the switch again
    public float _ResetTimer;    // the length of time before the switch interacts with itself after the player uses it if _DoesSwitchReset is true
    public GameObject _OffSwitch;// The object that represents the switch being off
    public GameObject _OnSwitch; // The object that represents the switch being on

    public PuzzleSwitch[] _AffectedSwitches; // The array of switches that this switch will affect


    public enum Switch_PlayerInteract_Type // Determines how the player interacts with the switch
    {
        UseButton,//------------------------------------------------ Player presses the use button to activate the switch

 //TODO       Damage,//--------------------------------------------------- Player uses a weapon on the switch to activate it  /

        Proximity//------------------------------------------------- Player gets within proximity of the switch to activate it
    };
    public Switch_PlayerInteract_Type _PlayerInteractType;


    public enum Switch_ActivationPolicy_Type // Determines when the switch can be interacted with.
    {
        CanAlwaysInteract, //--------------------------------------- Player can always interact with the switch

        CanInteractWhenInactive, //--------------------------------- Player can only interact with the switch when it is inactive

        CanInteractWhenActive //------------------------------------ Player can only interact with the switch when it is active
    };
    public Switch_ActivationPolicy_Type _ActivationPolicy;


    public enum Switch_Response_Type // the behavior the switch responds with when interacted with
    {
        InteractSelf,//---------------------------------------------- Interacting with the switch affects only this switch

        InteractSwitches,//------------------------------------------ Interacting with the switch interacts with all switches in its Affected Switches array
    };
    public Switch_Response_Type _ResponseType;


    bool bPlayerInRange;
    bool bIsActive;
    bool bCanSwitch;

    float fSwitchTimer;
    float fResetTimer;

    // Start is called before the first frame update
    void Start()
    {
        fSwitchTimer = _ActivationTimer;
        fResetTimer = 0.0f;
        SetSwitchModel(bIsActive);
    }
    public void Interact()
    {
        if (bCanSwitch)
        {
            if (_DoesSwitchReset)
            {
                fResetTimer = _ResetTimer;
            }
            if (_ActivationPolicy == Switch_ActivationPolicy_Type.CanAlwaysInteract)
            {
                Activate(!bIsActive);
            }

            if (_ActivationPolicy == Switch_ActivationPolicy_Type.CanInteractWhenActive && bIsActive)
            {
                Activate(!bIsActive);
            }

            if (_ActivationPolicy == Switch_ActivationPolicy_Type.CanInteractWhenInactive && !bIsActive)
            {
                Activate(!bIsActive);
            }

            if (_ResponseType == Switch_Response_Type.InteractSwitches)
            {
                for (int i = 0; i < _AffectedSwitches.Length; i++)
                {
                    if (_AffectedSwitches[i] != null)
                    {
                        _AffectedSwitches[i].Interact();
                    }
                }
            }
        }

    }
    public void Activate(bool onOff)
    {
        if (bCanSwitch)
        {
            bIsActive = onOff;
            SetSwitchModel(onOff);
            bCanSwitch = false;
        }
    }

    public bool GetIsActive()
    {
        return bIsActive;
    }

    void CanSwitchTimer()
    {
        if (bCanSwitch == false)
        {
            if (fSwitchTimer > 0.0f)
            { fSwitchTimer -= Time.fixedDeltaTime; }

            else
            {
                fSwitchTimer = _ActivationTimer;
                bCanSwitch = true;
            }
        }
    }

    void ResetSwitchTimer()
    {
        if (_DoesSwitchReset == true)
        {
            if (fResetTimer > 0.0f)
            {
                fResetTimer -= Time.fixedDeltaTime;
                if (fResetTimer <= 0.0f)
                {
                    Interact();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CanSwitchTimer(); ResetSwitchTimer();
        if (_PlayerInteractType == Switch_PlayerInteract_Type.UseButton && bPlayerInRange)
        {
            PlayerInput();
        }
      
    }


    void PlayerInput()
    {
        if (Input.GetButtonDown("Continue"))
        {
            Interact();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bPlayerInRange = true;
            if (_PlayerInteractType == Switch_PlayerInteract_Type.Proximity)
            {
                Interact();
            }
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bPlayerInRange = false;
            if (_PlayerInteractType == Switch_PlayerInteract_Type.Proximity)
            {
                Interact();
            }
        }

    }

    public void SetSwitchModel(bool OnOff)
    {
        if (_OffSwitch != null && _OnSwitch != null)
        {
            if (OnOff)
            {
                _OffSwitch.SetActive(false);
                _OnSwitch.SetActive(true);
            }
            else

            {
                _OffSwitch.SetActive(true);
                _OnSwitch.SetActive(false);
            }
        }
    }

}
