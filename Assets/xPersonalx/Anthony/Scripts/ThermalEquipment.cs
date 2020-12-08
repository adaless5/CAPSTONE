using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalEquipment : Equipment
{
    ALTPlayerController _playerController; 
    bool bIsInThermalView = false;
    Light _directionalLight;

    public GameObject _particleSystemPrefab;
    GameObject _particleSystemObject;
    public ParticleSystem _particleSystem;

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

        _particleSystemPrefab = Instantiate<GameObject>(_particleSystemPrefab, gameObject.transform);
        _particleSystem = _particleSystemPrefab.GetComponentInChildren<ParticleSystem>();
        //_particleSystemPrefab.transform.position = _playerController.gameObject.transform.forward * 10.0f;
        //_particleSystemPrefab.SetActive(false);
    }

    // Update is called once per frame
    public override void Update()
    {
        if (bIsObtained)
        {
            UseTool(); 

            if(bIsInThermalView)
            {
                if(_particleSystemPrefab != null)
                {
                    _particleSystemPrefab.transform.position = _playerController.gameObject.transform.forward * 10.0f;
                }
            }
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

                    //if (_particleSystem != null)
                    //{
                    //    _particleSystem.Play();
                    //}

                    //_particleSystemPrefab.SetActive(true);

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

                    //_particleSystemPrefab.SetActive(false);

                    bIsInThermalView = false;
                }
            }

            _playerController.SetThermalView(bIsInThermalView);
        }
    }
}
