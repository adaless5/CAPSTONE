using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThermalEquipment : Equipment
{
    ALTPlayerController _playerController;
    bool bIsInThermalView = false;
    Light _directionalLight;
    List<Light> _SceneLights;
    List<Color> _SceneLightColors;

    Vector4 _originalLightValues; ///evan added this

    public GameObject _particleSystemPrefab;
    GameObject _particleSystemObject;
    public ParticleSystem _particleSystem;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _playerController = FindObjectOfType<ALTPlayerController>();

        Light[] lightarray = FindObjectsOfType<Light>();

        foreach (Light light in lightarray)
        {
            if (light.gameObject.tag == "Dir Light")
            {
                _directionalLight = light;
                _originalLightValues = light.color; ///evan added this

                break;
            }
        }

        //_particleSystemPrefab = Instantiate<GameObject>(_particleSystemPrefab, gameObject.transform);
        //_particleSystem = _particleSystemPrefab.GetComponentInChildren<ParticleSystem>();
        //_particleSystemPrefab.transform.position = _playerController.gameObject.transform.forward * 10.0f;
        //_particleSystemPrefab.SetActive(false);
    }

    void Awake()
    {
    }

    void SetLightsThermal()
    {
        _SceneLights = new List<Light>();
        _SceneLightColors = new List<Color>();
        Light[] lights = FindObjectsOfType<Light>();
        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i].gameObject.tag != "Dir Light")
            {
                _SceneLights.Add(lights[i]);
                _SceneLightColors.Add(lights[i].color);
                lights[i].color = new Color(lights[i].color.r * 2, 0, lights[i].color.b * 2);
            }
        }
    }

    void SetLightsNormal()
    {
        for (int i = 0; i < _SceneLights.Count; i++)
        {
            _SceneLights[i].color = _SceneLightColors[i];
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (bIsObtained)
        {
            if (Keyboard.current != null)
            {
                if (Keyboard.current.fKey.wasPressedThisFrame)
                    UseTool();
            }
            if (Gamepad.current != null)
            {
                if (Gamepad.current.selectButton.wasPressedThisFrame)
                    UseTool();
            }

            if (bIsInThermalView)
            {
                _directionalLight.color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
                if (_particleSystemPrefab != null)
                {
                    _particleSystemPrefab.transform.position = _playerController.gameObject.transform.forward * 10.0f;
                }

                CheckNewScene();
            }
        }
    }

    public bool GetIsInView()
    {
        return bIsInThermalView;
    }

    void ThermalOn()
    {
        ThermalSkin[] ThermalObjs = FindObjectsOfType<ThermalSkin>();
        SetLightsThermal();
        foreach (ThermalSkin obj in ThermalObjs)
        {
            obj.ChangeToThermalSkin();
        }

        if (_directionalLight != null)
        {
            _directionalLight.color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
        }

        bIsInThermalView = true;
    }
    void ThermalOff()
    {
        ThermalSkin[] ThermalObjs = FindObjectsOfType<ThermalSkin>();
        SetLightsNormal();
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
                _directionalLight.color = _originalLightValues;
            }
        }

        //_particleSystemPrefab.SetActive(false);
        bIsInThermalView = false;
    }

    public override void UseTool()
    {
        if (!bIsInThermalView)
        {
            ThermalOn();
        }
        else if (bIsInThermalView)
        {
            ThermalOff();
        }
        _playerController.SetThermalView(bIsInThermalView);
    }
    /// EVAN ADDED ^^^


    void CheckNewScene()
    {
        bool didswitch = false;

        if (_SceneLights.Count != FindObjectsOfType<Light>().Length -1)
        {
            didswitch = true;
        }

        if (didswitch)
        {
            for (int i = 0; i < _SceneLightColors.Count; i++)
            {
                if (_SceneLights[i] != null)
                {
                    _SceneLights[i].color = _SceneLightColors[i];
                }
            }
            ThermalOn();
        }
    }


    /// OG CODE VVVV


    //if (_playerController != null)
    //{
    //    if(_playerController.CheckForUseThermalInput())
    //    {
    //        ThermalSkin[] ThermalObjs = FindObjectsOfType<ThermalSkin>();

    //        if (!bIsInThermalView)
    //        {
    //            foreach (ThermalSkin obj in ThermalObjs)
    //            {
    //                obj.ChangeToThermalSkin();
    //            }

    //            if (_directionalLight != null)
    //            {
    //                if (_playerController.GetDarknessVolume())
    //                {
    //                    _directionalLight.color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
    //                }
    //                else if(!_playerController.GetDarknessVolume())
    //                {
    //                    _directionalLight.color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
    //                }
    //            }

    //            //if (_particleSystem != null)
    //            //{
    //            //    _particleSystem.Play();
    //            //}

    //            //_particleSystemPrefab.SetActive(true);

    //            bIsInThermalView = true;


    //        }
    //        else if (bIsInThermalView)
    //        {
    //            foreach (ThermalSkin obj in ThermalObjs)
    //            {
    //                obj.ChangeToNormalSkin();
    //            }

    //            if (_directionalLight != null)
    //            {
    //                if (_playerController.GetDarknessVolume())
    //                {
    //                    _directionalLight.color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
    //                }
    //                else if (!_playerController.GetDarknessVolume())
    //                {
    //                    _directionalLight.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
    //                }
    //            }

    //            //_particleSystemPrefab.SetActive(false);

    //            bIsInThermalView = false;
    //        }
    //    }

    //    _playerController.SetThermalView(bIsInThermalView);
    //}
}
