using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DarknessTrigger : MonoBehaviour, ITippable
{
    enum DirectionalLightState
    {
        Set, 
        Decrementing,
        Incrementing,
    }

    DirectionalLightState _directionalLightState;
    Light _directionalLight;
    Vector4 _baseDirLightColour;

    Vector4 _lightVals;
    Vector4 _targetLightVals = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

    ALTPlayerController _playerController;

    GameObject _imageObject = null;

    private void Awake()
    {
        
    }

    void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;

        Light[] lightarray = FindObjectsOfType<Light>();

        foreach(Light light in lightarray)
        {
            if (light.gameObject.tag == "Dir Light")
            {
                _directionalLight = light;
                _targetLightVals = _directionalLight.color;
                _baseDirLightColour = _directionalLight.color;
                break;
            }
        }
    }

    void Update()
    {
        //The problem here is that the Darkness trigger spawns in before the player so if I init this on awake it returns a null
        //After release 1 this will probably be a good candidate for Leo's event broker. 
        if(_playerController == null)
        {
            _playerController = FindObjectOfType<ALTPlayerController>();
        }

        switch (_directionalLightState)
        {
            case DirectionalLightState.Set:
                break;

            case DirectionalLightState.Decrementing:
                DecrementLight();
                break;

            case DirectionalLightState.Incrementing:
                IncrementLight();
                break;
        }

        if (_playerController != null)
        {
            if (_playerController.GetThermalView())
            {
                _targetLightVals = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
            }
            else if (_playerController.GetThermalView() == false)
            {
                _targetLightVals = _baseDirLightColour;
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            _directionalLightState = DirectionalLightState.Decrementing;
            _playerController.SetDarknessVolume(true);

            ALTPlayerController pc = collider.gameObject.GetComponent<ALTPlayerController>();

            print(pc._equipmentBelt._items[2]);

            if (pc._equipmentBelt._items[2].bIsObtained == false)
            {
                CreateTip("Sprites/Messages/DARKNESS_WARNING");
            }

        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            _directionalLightState = DirectionalLightState.Incrementing;

            _playerController.SetDarknessVolume(false);

            DestroyTip();
        }
    }

    void DecrementLight()
    {
        _lightVals = _directionalLight.color;

        if(_lightVals.x >= 0f && _lightVals.y >= 0f && _lightVals.z >= 0f)
        {
            _lightVals.x -= Time.deltaTime;
            _lightVals.x = Mathf.Clamp(_lightVals.x, 0.0f, 1.0f);

            _lightVals.y -= Time.deltaTime;
            _lightVals.y = Mathf.Clamp(_lightVals.y, 0.0f, 1.0f);

            _lightVals.z -= Time.deltaTime;
            _lightVals.z = Mathf.Clamp(_lightVals.z, 0.0f, 1.0f);

            _directionalLight.color = new Vector4(_lightVals.x, _lightVals.y, _lightVals.z, 1);


        }
        else
        {
            _lightVals.x = 0.0f;
            _lightVals.y = 0.0f;
            _lightVals.z = 0.0f;
            _directionalLightState = DirectionalLightState.Set;
        }
    }

    void IncrementLight()
    {
        _lightVals = _directionalLight.color;


        if(_lightVals.x <= _targetLightVals.x)
        {
            _lightVals.x += Time.deltaTime;
            _lightVals.x = Mathf.Clamp(_lightVals.x, 0.0f, _targetLightVals.x);
        }

        if(_lightVals.y <= _targetLightVals.y)
        {
            _lightVals.y += Time.deltaTime;
            _lightVals.y = Mathf.Clamp(_lightVals.y, 0.0f, _targetLightVals.y);
        }

        if(_lightVals.z <= _targetLightVals.z)
        {
            _lightVals.z += Time.deltaTime;
            _lightVals.z = Mathf.Clamp(_lightVals.z, 0.0f, _targetLightVals.z);
        }
        else
        {
            _lightVals.x = _targetLightVals.x;
            _lightVals.y = _targetLightVals.y;
            _lightVals.z = _targetLightVals.z;
            _directionalLightState = DirectionalLightState.Set;
            return;
        }
        _directionalLight.color = new Vector4(_lightVals.x, _lightVals.y, _lightVals.z, 1.0f);
    }

    public void CreateTip(string filename)
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud != null)
        {
            Canvas canvas = hud.GetComponent<Canvas>();

            if (canvas != null)
            {
                DestroyTip();

                _imageObject = new GameObject("testTip");
                _imageObject.tag = "Tip";

                RectTransform trans = _imageObject.AddComponent<RectTransform>();
                trans.transform.SetParent(canvas.transform); // setting parent
                trans.localScale = Vector3.one;
                trans.pivot = new Vector2(0.5f, -1f);
                trans.anchoredPosition = new Vector2(0f, -600f);
                Texture2D tex = Resources.Load<Texture2D>(filename);
                if (tex != null)
                {
                    trans.sizeDelta = new Vector2(tex.width / 1f, tex.height / 1.5f); // custom size
                }

                Image image = _imageObject.AddComponent<Image>();
                if (image != null)
                {
                    if (tex != null)
                    {
                        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, -1f));
                        _imageObject.transform.SetParent(canvas.transform);
                    }
                }
            }
        }
    }

    public void DestroyTip()
    {
        GameObject[] array = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in array)
        {
            if (obj.tag == "Tip")
            {
                Destroy(obj);
            }
        }
        _imageObject = null;
    }
}
