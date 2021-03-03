using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class holds necessary information to spawn a player in a scene. 
//also handles what connector the player spawns at while testing and editing a scene
public class PlayerStart : MonoBehaviour
{
    Object _playerPrefab;
    Vector3 _startPosition;
    Vector3 _startRotation;
    Vector3 _halfCharacterControllerHeight = new Vector3(0,1.3f,0);

    bool firstPass = false;

    // Start is called before the first frame update
    void Start()
    {
        firstPass = true;
    }

    private void LateUpdate()
    {
        if (firstPass)
        {
            firstPass = false;

            //Load Player Prefab and initialize player
            _playerPrefab = Resources.Load("Prefabs/Player/Player");
            InitializePlayer();
            //

            //Creates a player at the start of the game.
            if (GameObject.FindWithTag("Player") == null)
            {
                
                _playerPrefab = Instantiate(_playerPrefab, _startPosition + _halfCharacterControllerHeight, Quaternion.Euler(_startRotation));

                GameObject player = (GameObject)_playerPrefab;
                EventBroker.CallOnPlayerSpawned(ref player);
                player.transform.position = _startPosition;
                player.transform.Rotate(_startRotation, Space.Self);
            }
            //
        }
    }

    void OnDrawGizmos()
    {
        //Draw PlayerStart and direction coming out of scene Connector.
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, .5f);
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 2));
        //
    }

    //This function runs when the game intially starts. It finds all active scene connectors in the scene then
    //finds the default and creates a player at that scene connectors position.
    void InitializePlayer()
    {
        
        //Init Player From Spawn Point if its Present.
        SpawnPoint spawnPoint = FindObjectOfType<SpawnPoint>();
        if (spawnPoint != null)
        {
            _startPosition = spawnPoint.transform.position;
            _startRotation = spawnPoint.transform.rotation.eulerAngles;

        }
        
        //Else Init player from the first SceneConnector in the scene.
        else
        {
            PlayerStart start = FindObjectOfType<SceneConnector>().GetComponentInChildren<PlayerStart>();
            _startPosition = start.transform.position;
            _startRotation = start.transform.localEulerAngles;
        }
    }
}
