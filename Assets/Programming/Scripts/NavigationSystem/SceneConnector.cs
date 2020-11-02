using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneConnector : MonoBehaviour
{
    bool bDebug = false;

    [SerializeField] public enum SceneConnectorType
    {
        Portal,
        NonEuclidian,
        Seamless, 
    }

    public string _ID { get; set; } = "";
    public SceneConnectorType _type { get; set; }
    public string _destinationSceneName;
    public string _destinationSceneID = "";

    Object _unloadTrigger;
    SceneConnectorData _data;

    void Awake()
    {
        GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(delayActivationOnSceneEnter());
    }
    
    //This prevents the trigger from being active for the first second of a new scene.
    IEnumerator delayActivationOnSceneEnter()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<BoxCollider>().enabled = true;
    }

    IEnumerator DispatchSave()
    {
        SaveSystem.SaveEvent.Invoke();
        yield return new WaitForSeconds(.5f);
    }

    IEnumerator LoadNewSceneAsync(LoadSceneMode loadingMode, string destinationScene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(destinationScene,loadingMode);
        while (!asyncLoad.isDone) yield return null;
    }

    IEnumerator LoadNewScene_Portal(SceneConnectorData registryData, Transform playerTransform)
    {
        yield return StartCoroutine(DispatchSave());
        SceneManager.LoadScene(registryData.sceneName, LoadSceneMode.Single);

        playerTransform.position = registryData.pos;
        playerTransform.rotation = registryData.rot;
    }

    IEnumerator SaveThenLoadNewScene(LoadSceneMode loadingMode)
    {
        yield return StartCoroutine(DispatchSave());
        SceneManager.LoadScene(_destinationSceneName,loadingMode);
    }

    void OnTriggerEnter(Collider other)
    {
        //When Player enters the trigger call a save event and switch scenes.
        if (other.tag == "Player")
        {
            //Fetch connector data from permanent save
            _data = new SceneConnectorData(transform, "", "", "", _type, "", "");
            _data.FromString(SaveSystem.LoadString(name + SceneManager.GetActiveScene().name, ""));

            switch (_data.type)
            {
                case SceneConnectorType.Seamless:

                    if (!SceneManager.GetSceneByName(_data.destinationSceneName).IsValid())
                        SceneManager.LoadSceneAsync(_data.destinationSceneName, LoadSceneMode.Additive);
                    StartCoroutine(DispatchSave());
                    break;

                case SceneConnectorType.Portal:
                    
                    SceneConnectorData registryData = SceneConnectorRegistry.GetDataFromID(_data.destinationSceneID);
                    Transform playerTransform = other.GetComponentInParent<Transform>();

                    //In Same Scene
                    if (_data.sceneName.Equals(_data.destinationSceneName))
                    {
                        playerTransform.position = registryData.pos;
                        playerTransform.rotation = registryData.rot;
                    }

                    //In Different Scene
                    else StartCoroutine(LoadNewScene_Portal(registryData, playerTransform));
                    
                    break;

                case SceneConnectorType.NonEuclidian:
                    //StartCoroutine(SaveThenLoadNewScene(LoadSceneMode.Single));
                    break;
            }
        } 
    }

    public void CreateUnloadTrigger()
    {
        if (_unloadTrigger == null)
        _unloadTrigger = Instantiate(Resources.Load("Prefabs/NavigationSystem/UnloadTrigger"), transform);
    }

    public void DeleteUnloadTrigger()
    {
        if (_unloadTrigger != null)
            DestroyImmediate(_unloadTrigger);
    }

    public void UnloadScene()
    {
        if(bDebug)Debug.Log("Unloaded : " + _data.sceneName);
        SceneManager.UnloadSceneAsync(_data.sceneName);
    }

    //Container class holding necessary persistent date required by other connectors cross scene.
    public class SceneConnectorData
    {
        public Vector3 pos;
        public Quaternion rot;
        public string ID { get; set; }
        public string sceneName { get; set; }
        public string name { get; set; }

        public SceneConnectorType type;

        public string destinationSceneID { get; set; }
        public string destinationSceneName { get; set; }
        public string destinationConnectorName { get; set; }

        //Main Constructor
        public SceneConnectorData(Transform t, string ID, string sceneName, string name, SceneConnector.SceneConnectorType type, string goesToID, string goesToScene)
        {
            pos = t.position;
            rot = t.rotation;
            this.ID = ID;
            this.sceneName = sceneName;
            this.name = name;
            this.type = type;
            destinationSceneID = goesToID;
            destinationSceneName = goesToScene;

        }

        //Empty Constructor
        public SceneConnectorData()
        {
            pos = new Vector3(0.0f,0.0f,0.0f);
            rot = new Quaternion(0.0f,0.0f,0.0f,0.0f);
            ID = "";
            sceneName = "";
            name = "";
            type = SceneConnectorType.Portal;
            destinationSceneID = "";
            destinationSceneName = "";
            destinationConnectorName = "";
        }

        //Converts data to string representation, in order to be stored within save system
        public override string ToString()
        {
            return pos.x + "~" + pos.y + "~" + pos.z + "~"
                 + rot.x + "~" + rot.y + "~" + rot.z + "~" + rot.w + "~"
                 + ID + "~"
                 + sceneName + "~"
                 + name + "~"
                 + type.ToString() + "~"
                 + destinationSceneID + "~"
                 + destinationSceneName + "~"
                 + destinationConnectorName + "~";
        }

        //Converts data string back into usable data information
        public void FromString(string str)
        {
            string s = "";
            int i = 0;
            foreach (char c in str)
            {
                if (c.Equals('~'))
                {
                    switch (i)
                    {
                        case 0:
                            pos.x = float.Parse(s); break;
                        case 1:
                            pos.y = float.Parse(s); break;
                        case 2:
                            pos.z = float.Parse(s); break;
                        case 3:
                            rot.x = float.Parse(s); break;
                        case 4:
                            rot.y = float.Parse(s); break;
                        case 5:
                            rot.z = float.Parse(s); break;
                        case 6:
                            rot.w = float.Parse(s); break;
                        case 7:
                            ID = s; break;
                        case 8:
                            sceneName = s; break;
                        case 9:
                            name = s; break;
                        case 10:
                            type = ParseSceneConnectorTypeFromString(s); break;
                        case 11:
                            destinationSceneID = s; break;
                        case 12:
                            destinationSceneName = s; break;
                        case 13:
                            destinationConnectorName = s; break;
                    }
                    i++; s = "";
                }
                else s += c;
            }
        }

        SceneConnectorType ParseSceneConnectorTypeFromString(string type)
        {
            switch (type)
            {
                case "Seamless":
                    return SceneConnectorType.Seamless;
                case "Portal":
                    return SceneConnectorType.Portal;
                case "NonEuclidian":
                    return SceneConnectorType.NonEuclidian;
            }
            return SceneConnectorType.Portal; //Use portal as default if none is found.
        }
    }

}























//Debug.Log(_ID);

            // If its a portal we must reposition the player.
            // if (_sceneTriggerType == SceneTriggerType.Portal)
            // {
            //     Reset Player Position and orient Rotation.
            //     if (_ID != "")// If id is not default, look in new scene for portal with same ID    
            //     {
            //         GameObject[] sceneTriggers = GameObject.FindGameObjectsWithTag("SceneTrigger");
                    
            //         foreach (GameObject trigger in sceneTriggers)
            //         {
            //             if (trigger.scene.name ==  SceneManager.GetActiveScene().name)
            //             {
            //                 Debug.Log(trigger.scene.name);
                            
            //                 found portal with same ID
            //                 if(_GoToID == trigger.GetComponent<SceneTrigger>()._ID)
            //                 {
            //                     Debug.Log(trigger.GetComponentInChildren<PlayerStart>().GetComponent<Transform>().transform.position);

            //                     Reposition Player
            //                     GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().transform.position = 
            //                         trigger.GetComponentInChildren<PlayerStart>().GetComponent<Transform>().transform.position;

            //                     GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().transform.rotation = 
            //                         trigger.GetComponentInChildren<PlayerStart>().GetComponent<Transform>().transform.rotation;
                                
            //                     break;
            //                 }
            //             }
                        
            //         }
            //     } 
            // }