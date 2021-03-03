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

        if (MainMenuUI.bNewGame)
        {
            AsyncOperation firstLevel = SceneManager.LoadSceneAsync(3);
            AsyncOperation secondlevel = SceneManager.LoadSceneAsync(4,LoadSceneMode.Additive);


            while (firstLevel.progress < 1f)
            {
                _fillMeter.fillAmount = firstLevel.progress;
                yield return new WaitForEndOfFrame();
            }

            while (secondlevel.progress < 1f)
            {
                _fillMeter.fillAmount = secondlevel.progress;
                yield return new WaitForEndOfFrame();
            }
        }
        else if (!MainMenuUI.bNewGame)
        {
            SaveSystem.RespawnInfo_Data data = new SaveSystem.RespawnInfo_Data();
            data.FromString(FileIO.FetchRespawnInfo());

            if (data.sceneName == "")
            {
                AsyncOperation firstLevel = SceneManager.LoadSceneAsync(3);

                while (firstLevel.progress < 1f)
                {
                    _fillMeter.fillAmount = firstLevel.progress;
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                AsyncOperation firstLevel = SceneManager.LoadSceneAsync(data.sceneName);

                while (firstLevel.progress < 1f)
                {
                    _fillMeter.fillAmount = firstLevel.progress;
                    yield return new WaitForEndOfFrame();
                }
            }

            
        }
    }
}
