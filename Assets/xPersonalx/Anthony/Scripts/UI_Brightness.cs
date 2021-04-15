using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class UI_Brightness : MonoBehaviour
{
    Image[] _imagesInCanvas;

    // Start is called before the first frame update
    void Awake()
    {
        _imagesInCanvas = GetComponentsInChildren<Image>();

        foreach (Image img in _imagesInCanvas)
        {
            img.color = new Color(0.75f, 0.75f, 0.75f, 1.0f);
        }
    }

    public void SetBrightness(Color newcolor)
    {
        foreach (Image img in _imagesInCanvas)
        {
            img.color = newcolor;
        }
    }
}
