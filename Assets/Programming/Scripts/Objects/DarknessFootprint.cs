using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessFootprint : MonoBehaviour
{
    public GameObject FootLeft;
    public GameObject FootRight;

    bool _bStep = false;
    bool _bLast = false;
    ALTPlayerController _PC;

    float _maxStepTime = 0.3f;
    float _stepTime = 0.5f;

    List<GameObject> _footSteps;

    void Start()
    {
        _stepTime = _maxStepTime;
        _PC = gameObject.GetComponent<ALTPlayerController>();
        _footSteps = new List<GameObject>();
        enabled = false;
    }

    void Update()
    {
        //make footsteps

        if (ALTPlayerController.instance.GetIsWalking())
        {
            _stepTime -= Time.deltaTime;
            if (_stepTime <= 0)
            {
                if (_bStep)
                {
                    RightStep();
                }
                else
                {
                    LeftStep();
                }
            }
       
        }

        //decides whether its active or not
        //if (_bLast != _PC.GetThermalView())
        {
            if (!_PC.GetThermalView())
            {
                ActivateSteps();
            }
            else
            {
                DeactivateSteps();
            }
        }
    }

    private void RightStep()
    {
        _stepTime = _maxStepTime;
        _bStep = false;

        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, -gameObject.transform.up, out hit, 2))
        {
            GameObject newprint = Instantiate(FootRight, hit.point - new Vector3(0.5f, -0.01f, 0), Quaternion.FromToRotation(Vector3.forward, hit.normal));
            newprint.transform.Rotate(0, 0, gameObject.transform.localEulerAngles.y + 180);
            _footSteps.Add(newprint);
        }
    }

    private void LeftStep()
    {
        _stepTime = _maxStepTime;
        _bStep = true;
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, -gameObject.transform.up, out hit, 2))
        { 
            GameObject newprint = Instantiate(FootLeft, hit.point + new Vector3(0.5f, 0.01f, 0), Quaternion.FromToRotation(Vector3.forward, hit.normal));
            newprint.transform.Rotate(0, 0, gameObject.transform.localEulerAngles.y + 180);
            _footSteps.Add(newprint);
        }
    }

    private void ActivateSteps()
    {
        foreach (GameObject obj in _footSteps)
        {
            obj.SetActive(true);
        }
        _bLast = _PC.GetThermalView();
    }

    private void DeactivateSteps()
    {
        foreach (GameObject obj in _footSteps)
        {
            obj.SetActive(false);
        }
        _bLast = _PC.GetThermalView();
    }

    public void EnterDarkness()
    {
        enabled = true;
        if (!_PC.GetThermalView())
        {
            ActivateSteps();
        }
    }

    public void ExitDarkness()
    {
        enabled = false;
        DeactivateSteps();
        _footSteps.Clear();
    }
}
