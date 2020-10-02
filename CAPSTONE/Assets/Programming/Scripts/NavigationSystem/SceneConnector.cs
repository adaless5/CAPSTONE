using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneConnector : MonoBehaviour
{
    [SerializeField] public enum SceneConnectorType
    {
        Portal,
        Seamless,
    }

    public static string lastKnownID {get; private set;} = null;
    public static SceneConnectorType lastKnownConnectorType {get; private set;} = SceneConnectorType.Seamless;


    [Tooltip("Enable this to make this portal the entrance point when you press play. Used for testing specific scenes")]
    public bool isDefault = false;
    
    [Tooltip("Custom ID for this specific sceneTrigger")]
    public string _ID = "";

    public SceneConnectorType _sceneConnectorType;

    [Tooltip("This is the scene that you will go to when entering the trigger")]
    public string _goesToScene;

    [Tooltip("What portal ID would u like to go to on scene change. leave this blank to go to default")]
    public string _goesToID = "";

    void Awake()
    {
        GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(delayTrigger());
    }
    
    //This prevents the trigger from being active for the first second of a new scene.
    IEnumerator delayTrigger()
    {
        yield return new WaitForSeconds(1);
        GetComponent<BoxCollider>().enabled = true;
    }

    IEnumerator DispatchSave()
    {
        SaveSystem.SaveEvent.Invoke();
        yield return new WaitForSeconds(.5f);
    }

    IEnumerator LoadNewSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_goesToScene,LoadSceneMode.Single);
        while (!asyncLoad.isDone) yield return null;
    }

    IEnumerator SaveThenSwitchScenesPortal()
    {
        yield return StartCoroutine(DispatchSave());
        //yield return StartCoroutine(LoadNewSceneAsync());
        SceneManager.LoadScene(_goesToScene,LoadSceneMode.Single);
    }

    IEnumerator SaveThenSwitchScenesSeamless_NonEuclidian()
    {
        yield return StartCoroutine(DispatchSave());
        //yield return StartCoroutine(LoadNewSceneAsync());
        SceneManager.LoadScene(_goesToScene,LoadSceneMode.Single);
    }

    void OnTriggerEnter(Collider other)
    {
        //When Player enters the trigger call a save event and switch scenes.
        if (other.tag == "Player")
        {
            lastKnownID = _goesToID;
            lastKnownConnectorType = _sceneConnectorType;

            if (_sceneConnectorType == SceneConnectorType.Portal) StartCoroutine(SaveThenSwitchScenesPortal());
            else if (_sceneConnectorType == SceneConnectorType.Seamless) StartCoroutine(SaveThenSwitchScenesSeamless_NonEuclidian());
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