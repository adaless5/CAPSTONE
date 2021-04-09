using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSwitch : MonoBehaviour, ISaveable
{
    public bool _DoesSwitchReset;
    public bool _DoesStartTurnedOn;
    public float _ActivationTimer;// the length of time the player has to wait to interact with the switch again
    public float _ResetTimer;    // the length of time before the switch interacts with itself after the player uses it if _DoesSwitchReset is true
    public GameObject _OffSwitch;// The object that represents the switch being off
    public GameObject _OnSwitch; // The object that represents the switch being on

    public GameObject _DamageObject;

    public PuzzleSwitch[] _AffectedSwitches; // The array of switches that this switch will affect

    ALTPlayerController _PlayerController;
    public enum Switch_PlayerInteract_Type // Determines how the player interacts with the switch
    {
        UseButton,//------------------------------------------------ Player presses the use button to activate the switch

        Damage,//--------------------------------------------------- Player uses a weapon on the switch to activate it  /

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
    public bool bIsActive;
    bool bCanSwitch;
    bool bObjectBroken;

    float fSwitchTimer;
    float fResetTimer;


    void Awake()
    {
        LoadDataOnSceneEnter();
    }

    // Start is called before the first frame update
    void Start()
    {
        fSwitchTimer = _ActivationTimer;
        fResetTimer = 0.1f;
        if (_DoesStartTurnedOn)
        {
            SetSwitchModel(_DoesStartTurnedOn);
        }
        else
        {
            SetSwitchModel(bIsActive);
        }
    }

    public void SwitchInteract()
    {
            if (_DoesSwitchReset)
            {
                fResetTimer = _ResetTimer;
            }
            if (_ActivationPolicy == Switch_ActivationPolicy_Type.CanAlwaysInteract)
            {
                Activate(!bIsActive);
            }

            else if (_ActivationPolicy == Switch_ActivationPolicy_Type.CanInteractWhenActive && bIsActive)
            {
                Activate(!bIsActive);
            }

            else if (_ActivationPolicy == Switch_ActivationPolicy_Type.CanInteractWhenInactive && !bIsActive)
            {
                Activate(!bIsActive);
            }

        
    }

    public void Interact()
    {
        SwitchInteract();
        if (_ResponseType == Switch_Response_Type.InteractSwitches)
        {
            foreach (PuzzleSwitch pswitch in _AffectedSwitches)
            {
                pswitch.SwitchInteract();
            }
        }
    }
    void Activate(bool onOff)
    {
        if (bCanSwitch)
        {
            bIsActive = onOff;
            SetSwitchModel(onOff);
            bCanSwitch = false;
            SaveDataOnSceneChange();
            if(_ActivationPolicy == Switch_ActivationPolicy_Type.CanInteractWhenInactive || _ActivationPolicy == Switch_ActivationPolicy_Type.CanInteractWhenActive)
            {
                gameObject.tag = "Untagged";
            }
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
            { fSwitchTimer -= Time.deltaTime; }

            else
            {
                fSwitchTimer = _ActivationTimer;
                bCanSwitch = true;
            }
        }
    }

    void ResetSwitchTimer()
    {
        if (_DoesSwitchReset == true && bIsActive != _DoesStartTurnedOn)
        {
            if (fResetTimer > 0.0f)
            {
                fResetTimer -= Time.deltaTime;
                if (fResetTimer < 0.0f)
                {
                    fResetTimer = _ResetTimer;
                    Activate(_DoesStartTurnedOn);
                }
            }
        }
    }

    void CheckDamage()
    {
        if(_PlayerInteractType == Switch_PlayerInteract_Type.Damage)
        if(!bObjectBroken && !_DamageObject.activeSelf)
        {
            Interact();
            bObjectBroken = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        CheckDamage();
        CanSwitchTimer(); 
        ResetSwitchTimer();
        if (_PlayerInteractType == Switch_PlayerInteract_Type.UseButton && bPlayerInRange)
        {
            PlayerInput();
        }

    }


    void PlayerInput()
    {
        if (_PlayerController != null)
        {
            if (_PlayerController.CheckForInteract())
            {
                Interact();
            }
        }
        else
        {
            _PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<ALTPlayerController>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<ALTPlayerController>().bWithinInteractVolume = true;
            bPlayerInRange = true;
            if (_PlayerInteractType == Switch_PlayerInteract_Type.Proximity)
            {
                Interact();
            }
        }

    }

    void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<ALTPlayerController>().bWithinInteractVolume = false;
        if (other.gameObject.tag == "Player")
        {
            bPlayerInRange = false;
            if (_PlayerInteractType == Switch_PlayerInteract_Type.Proximity)
            {
                Interact();
            }
        }

    }

    void SetSwitchModel(bool OnOff)
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


    public void SaveDataOnSceneChange()
    {
        SaveSystem.Save(gameObject.name, "bIsActive", gameObject.scene.name, bIsActive);
        SaveSystem.Save(gameObject.name, "objectBroken", gameObject.scene.name, bObjectBroken);
    }

    public void LoadDataOnSceneEnter()
    {
        bIsActive = _DoesStartTurnedOn;
        bIsActive = SaveSystem.LoadBool(gameObject.name, "bIsActive", gameObject.scene.name);
        bObjectBroken = SaveSystem.LoadBool(gameObject.name, "objectBroken", gameObject.scene.name);
        if (bObjectBroken)
        {
            Destroy(_DamageObject);
        }
    }
}
