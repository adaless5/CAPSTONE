using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ScreenShotManager : MonoBehaviour
{
    int ssIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.fKey.IsPressed())
        {
            Debug.Log("Screenshot Captured");
            ScreenCapture.CaptureScreenshot("PreviewShot" + ssIndex + ".jpeg");
            ssIndex++;
        }
    }
}
