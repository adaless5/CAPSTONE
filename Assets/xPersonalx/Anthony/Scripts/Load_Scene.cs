using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load_Scene : MonoBehaviour
{
    [SerializeField]
    private Image _fillMeter;

    float _MinimumLoadTime = 0.0f;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(LoadAsyncOperation());

        if (MainMenuUI.bNewGame)
        {
            print("SHOULD START A NEW GAME!");
        }
        else if (!MainMenuUI.bNewGame)
        {
            print("DON'T START A NEW GAME!");
        }

    }

    IEnumerator LoadAsyncOperation()
    {
        _MinimumLoadTime += Time.deltaTime;

        if (!SceneManager.GetSceneByName("R3_0_Persistant").IsValid())
        {
            AsyncOperation firstLevel = SceneManager.LoadSceneAsync("R3_0_Persistant", LoadSceneMode.Additive);
            //while (firstLevel.progress < 1f)
            //{
            //    _fillMeter.fillAmount = firstLevel.progress;
            //    yield return new WaitForEndOfFrame();
            //}
        }
        
        if (MainMenuUI.bNewGame)
        {
            AsyncOperation secondlevel = SceneManager.LoadSceneAsync("R3_1_CrashSite", LoadSceneMode.Additive);
            while (secondlevel.progress < 1f)
            {
                _fillMeter.fillAmount = secondlevel.progress;
                yield return new WaitForEndOfFrame();
            }
            SceneManager.UnloadSceneAsync("Loading_Scene");
        }
        else if (!MainMenuUI.bNewGame)
        {
            SaveSystem.RespawnInfo_Data data = new SaveSystem.RespawnInfo_Data();
            data.FromString(FileIO.FetchRespawnInfo());


            //if (data.sceneName == "")
            //{
            //    AsyncOperation firstLevel = SceneManager.LoadSceneAsync(3);

            //    while (firstLevel.progress < 1f)
            //    {
            //        _fillMeter.fillAmount = firstLevel.progress;
            //        yield return new WaitForEndOfFrame();
            //    }
            //}
            //else
            {
                //AsyncOperation firstLevel = SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
                AsyncOperation secondLevel = SceneManager.LoadSceneAsync(data.sceneName, LoadSceneMode.Additive);

                //while (firstLevel.progress < 1f)
                //{
                //    _fillMeter.fillAmount = firstLevel.progress;
                //    yield return new WaitForEndOfFrame();
                //    SceneManager.UnloadSceneAsync(2);
                //    EventBroker.CallOnLoadingScreenFinished(data);
                //}


                while (secondLevel.progress < 1f)
                {
                    _fillMeter.fillAmount = secondLevel.progress;
                    yield return new WaitForEndOfFrame();
                }
            }
            EventBroker.CallOnLoadingScreenFinished(data);
            EventBroker.CallOnDataChange();
            SceneManager.UnloadSceneAsync("Loading_Scene");
        }
    }
}
