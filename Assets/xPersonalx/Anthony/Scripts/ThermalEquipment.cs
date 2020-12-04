using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalEquipment : Equipment
{
    ALTPlayerController _playerController; 
    bool bIsInThermalView = false;
    Light _directionalLight;

    // Start is called before the first frame update
    public override void Start()
    {
        _playerController = FindObjectOfType<ALTPlayerController>();

        Light[] lightarray = FindObjectsOfType<Light>();

        foreach (Light light in lightarray)
        {
            if (light.gameObject.tag == "Dir Light")
            {
                _directionalLight = light;

                break;
            }
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (bIsObtained)
        {
            UseTool(); 
        }
    }

    public override void UseTool()
    {
        if (_playerController != null)
        {
            if(_playerController.CheckForUseThermalInput())
            {
                ThermalSkin[] ThermalObjs = FindObjectsOfType<ThermalSkin>();

                if (!bIsInThermalView)
                {
                    foreach (ThermalSkin obj in ThermalObjs)
                    {
                        obj.ChangeToThermalSkin();
                    }

                    if (_directionalLight != null)
                    {
                        if (_playerController.GetDarknessVolume())
                        {
                            _directionalLight.color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
                        }
                        else if(!_playerController.GetDarknessVolume())
                        {
                            _directionalLight.color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
                        }
                    }

                    bIsInThermalView = true;
                }
                else if (bIsInThermalView)
                {
                    foreach (ThermalSkin obj in ThermalObjs)
                    {
                        obj.ChangeToNormalSkin();
                    }

                    if (_directionalLight != null)
                    {
                        if (_playerController.GetDarknessVolume())
                        {
                            _directionalLight.color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
                        }
                        else if (!_playerController.GetDarknessVolume())
                        {
                            _directionalLight.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
                        }
                    }

                    bIsInThermalView = false;
                }
            }

            _playerController.SetThermalView(bIsInThermalView);
        }
    }
}
