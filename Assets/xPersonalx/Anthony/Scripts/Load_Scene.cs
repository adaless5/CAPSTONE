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
        
    }

    IEnumerator LoadAsyncOperation()
    {
        _MinimumLoadTime += Time.deltaTime;

        AsyncOperation firstLevel = SceneManager.LoadSceneAsync(3);
        
        while (firstLevel.progress < 1f)
        {
            _fillMeter.fillAmount = firstLevel.progress;
            yield return new WaitForEndOfFrame();
        }
    }
}
