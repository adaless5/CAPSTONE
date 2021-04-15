using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSaveScripts : MonoBehaviour
{
    public GameObject _autoSaveIcon;

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
        try
        {
            _autoSaveIcon.SetActive(true);
            StartCoroutine(HideAutoSaveIcon());

        }
        catch
        { Debug.Log("Reference of AutoSaveIcon not found"); }
    }

    public IEnumerator HideAutoSaveIcon()
    {
        yield return new WaitForSeconds(4.0f);
        _autoSaveIcon.SetActive(false);

    }
}
