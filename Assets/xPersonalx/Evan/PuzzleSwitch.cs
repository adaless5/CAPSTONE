using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSwitch : MonoBehaviour
{
    public bool bIsActive;
    public float _SwitchTimer;
    public float _FullTimer;
    bool bCanSwitch;

    public GameObject _OffSwitch;
    public GameObject _OnSwitch;

    public bool bPlayerNearby;

    // Start is called before the first frame update
    void Start()
    {

        _FullTimer = 1.0f;
        _SwitchTimer = _FullTimer;
        SetSwitchModel(bIsActive);
    }

    public void Activate(bool onOff)
    {
        if (bCanSwitch && bPlayerNearby)
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
            if (_SwitchTimer > 0.0f)
            { _SwitchTimer -= Time.fixedDeltaTime; }

            else
            {
                _SwitchTimer = _FullTimer;
                bCanSwitch = true;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        CanSwitchTimer(); 
        PlayerInput();
    }


    void PlayerInput()
    {
        if(Input.GetButtonDown("Interact"))
        {
            Activate(!bIsActive);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bPlayerNearby = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bPlayerNearby = false;
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
