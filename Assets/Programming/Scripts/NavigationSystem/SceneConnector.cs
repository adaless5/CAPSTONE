using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneConnector : MonoBehaviour
{
    bool bDebug = false;

    [SerializeField]
    public enum SceneConnectorType
    {
        Portal,
        NonEuclidian,
        Seamless,
    }

    public string _ID { get; set; } = "";
    public SceneConnectorType _type { get; set; }
    public string _destinationSceneName;
    public string _destinationSceneID = "";

    UnityEngine.Object _unloadTrigger;
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

    IEnumerator LoadNewScene_Portal(SceneConnectorData registryData, Transform playerTransform)
    {
        SceneManager.LoadScene(registryData.sceneName, LoadSceneMode.Single);

        playerTransform.position = registryData.pos;
        playerTransform.rotation = registryData.rot;

        yield return null;
    }

    void OnTriggerEnter(Collider other)
    {
        //When Player enters the trigger call a save event and switch scenes.
        if (other.tag == "Player")
        {
            //Fetch connector data from permanent save
            _data = new SceneConnectorData(transform, "", "", "", _type, "", "");
            _data.FromString(SaveSystem.LoadConnector(name, gameObject.scene.name));

            Transform playerTransform = other.GetComponentInParent<Transform>();

            switch (_data.type)
            {
                case SceneConnectorType.Seamless:

                    //attempt at using less colliders for seamless scene transitions -lcc
                    //for (int i = 1; i < scenemanager.scenecount; i++)
                    //{

                    //    if (scenemanager.getsceneat(i).name != scenemanager.getscenebybuildindex(3).name)
                    //    {
                    //        scenemanager.unloadsceneasync(scenemanager.getsceneat(i).name);
                    //    }
                    //}

                    //startcoroutine(unloadscenedelayed());
                    StartCoroutine(UpdateRespawnInfo(playerTransform));
                    EventBroker.CallOnAutoSave();
                    if (!SceneManager.GetSceneByName(_data.destinationSceneName).IsValid())
                        SceneManager.LoadSceneAsync(_data.destinationSceneName, LoadSceneMode.Additive);



                    break;

                case SceneConnectorType.Portal:

                    SceneConnectorData registryData = SceneConnectorRegistry.GetDataFromID(_data.destinationSceneID);


                    //In Same Scene
                    if (_data.sceneName.Equals(_data.destinationSceneName))
                    {
                        playerTransform.position = registryData.pos;
                        playerTransform.rotation = registryData.rot;
                    }

                    //In Different Scene
                    else
                    {
                        StartCoroutine(LoadNewScene_Portal(registryData, playerTransform));
                        StartCoroutine(UpdateRespawnInfo(playerTransform));
                    }

                    break;

                case SceneConnectorType.NonEuclidian:
                    //StartCoroutine(SaveThenLoadNewScene(LoadSceneMode.Single));
                    break;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Transform playerTransform = other.GetComponentInParent<Transform>();
        //Debug.Log("Exit load trigger");
        //if (_data != null)
        //{
        //    switch (_data.type)
        //    {
        //        case SceneConnectorType.Seamless:
        //            UnloadScene();
        //            EventBroker.CallOnDataChange();
        //            if (playerTransform != null)
        //                StartCoroutine(UpdateRespawnInfo(playerTransform));
        //            break;
        //    }

        //}
    }

    IEnumerator UpdateRespawnInfo(Transform playerTransform)
    {
        yield return new WaitForSeconds(5f);
        for (int i = 1; i < SceneManager.sceneCount; i++)
        {

            if (SceneManager.GetSceneAt(i).name == _data.destinationSceneName)
            {
                FileIO.ExportRespawnInfoToFile(playerTransform, SceneManager.GetSceneAt(i).name);
            }
        }
        yield return null;
        //FileIO.ExportRespawnInfoToFile(playerTransform, SceneManager.GetActiveScene().name);
        //if (SceneManager.sceneCount > 1)
        //{
        //    FileIO.ExportRespawnInfoToFile(playerTransform, SceneManager.GetSceneAt(1).name);
        //}
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
        if (bDebug) Debug.Log("Unloaded : " + _data.sceneName);
        try
        {

            SceneManager.UnloadSceneAsync(_data.sceneName);
        }
        catch (Exception e)
        {
            Debug.Log("can't unload scene");
        }
    }

    public IEnumerator UnloadSceneDelayed()
    {
        yield return new WaitForEndOfFrame();
        if (bDebug) Debug.Log("Unloaded : " + _data.sceneName);
        try
        {
            SceneManager.UnloadSceneAsync(_data.sceneName);
        }
        catch (Exception e)
        {
            Debug.Log("can't unload scene");
        }
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
        public SceneConnectorData(Transform t, string ID, string sceneName, string name, SceneConnectorType type, string goesToID, string goesToScene)
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
            pos = new Vector3(0.0f, 0.0f, 0.0f);
            rot = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
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