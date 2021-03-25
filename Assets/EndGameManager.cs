using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EndGameManager : MonoBehaviour
{
    PlayableDirector endGameDirector;
    public GameObject cinematicPlayer;

    private void Awake()
    {
        endGameDirector = GetComponent<PlayableDirector>();
        EventBroker.OnPlayerSpawned += PlayerSpawn;
        //cinematicPlayer.transform.position = player.transform.position;
        Debug.Log("Sequence Playing");
        endGameDirector.Play();
    }

    void PlayerSpawn(GameObject player)
    {


    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
