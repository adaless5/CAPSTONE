using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSaveScripts : MonoBehaviour
{
    GameObject _autoSaveIcon;

    private void Awake()
    {
        _autoSaveIcon = transform.GetChild(0).gameObject;
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
