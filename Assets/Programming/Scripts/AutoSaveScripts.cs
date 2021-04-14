using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSaveScripts : MonoBehaviour
{
    GameObject _autoSaveIcon;

    private void Awake()
    {
        _autoSaveIcon = transform.GetChild(0).gameObject;
        EventBroker.OnAutoSave += DisplayAutoSaveIcon;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }


    void DisplayAutoSaveIcon()
    {
        _autoSaveIcon.SetActive(true);
        StartCoroutine(HideAutoSaveIcon());
    }

    public IEnumerator HideAutoSaveIcon()
    {
        yield return new WaitForSeconds(4.0f);
        _autoSaveIcon.SetActive(false);

    }
}
