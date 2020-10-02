using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    Object _playerPrefab;
    Vector3 _startPosition;
    Vector3 _startRotation;
    Vector3 _halfCharacterControllerHeight = new Vector3(0,1.3f,0);

    // Start is called before the first frame update
    void Start()
    {
        //Load Player Prefab and initialize player
        _playerPrefab = Resources.Load("Prefabs/Player/Player");
        InitializePlayer();
        //

        //Creates a player at the start of the game.
        if (GameObject.FindWithTag("Player") == null)
        {
            Instantiate(_playerPrefab, _startPosition + _halfCharacterControllerHeight, Quaternion.Euler(_startRotation));
        }
        //
    }

    void OnDrawGizmos()
    {
        //Draw PlayerStart and direction coming out of scene Connector.
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, .5f);
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 2));
        //
    }


    //This function runs when the game intially starts. It finds all active scene connectors in the scene then
    //  finds the default and creates a player at that scene connectors position.
    void InitializePlayer()
    {
        //Find Default SceneConnector to find level entry point.
        SceneConnector[] sceneConnectors = Object.FindObjectsOfType<SceneConnector>();

        foreach(SceneConnector connector in sceneConnectors)
        {
            if (connector.isDefault)
            {
                //Get position and rotation of default player start for the scene.
                _startPosition = connector.GetComponentInChildren<PlayerStart>().transform.position;
                _startRotation = Vector3.zero + new Vector3(0,connector.GetComponentInChildren<PlayerStart>().transform.rotation.eulerAngles.y,0);
                break;
            }
        }
        //
    }
}
