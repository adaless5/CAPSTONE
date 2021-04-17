using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EndGameManager : MonoBehaviour
{
    PlayableDirector endGameDirector;
    public GameObject cinematicPlayer;
    GameObject _player;
    private void Awake()
    {
        EventBroker.OnGameEnd += PlayEndGameCutscene;
        endGameDirector = GetComponent<PlayableDirector>();
        EventBroker.OnPlayerSpawned += PlayerSpawn;
        _player = ALTPlayerController.instance.gameObject;

    }

    void PlayEndGameCutscene()
    {
        _player.SetActive(false);
        //Camera cam = _player.GetComponentInChildren<Camera>();
        //cam = cinematicPlayer.GetComponent<Camera>();
        Debug.Log("Sequence Playing");
        endGameDirector.Play();
    }

    void PlayerSpawn(GameObject player)
    {
        //_player = player;
        //cinematicPlayer.transform.position = player.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        //if (ALTPlayerController.instance.CheckForInteract())
        //{
        //    //EventBroker.CallOnGameEnd();
        //    //PlayEndGameCutscene();
        //}

    }
}
