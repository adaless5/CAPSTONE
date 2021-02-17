using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterAnimations : MonoBehaviour
{
    public GameObject[] SpinningObject;
    public float rotationSpeed = 1.0f;
    bool UpDown;
    float currentTime;
    Vector3 _Position;
    // Start is called before the first frame update
    void Start()
    {
        _Position = transform.position;
    }
    void Timing()
    {
        if (UpDown)
        {
            currentTime += Time.deltaTime;
            if (currentTime > rotationSpeed)
            {
                UpDown = false;
            }
        }
        else
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0.0f)
            {
                UpDown = true;
            }
        }
    }
    void Spin()
    {
        for (int i = 0; i < SpinningObject.Length;i++)
        {
            SpinningObject[i].transform.Rotate((i+ 1) * 1.5f, (i + 1) * 2.7f, (i + 1) * 5.0f);
        }
        transform.position = new Vector3(_Position.x, _Position.y + Mathf.Lerp(0.0f, 1.0f, Mathf.Sin(currentTime / rotationSpeed)), _Position.z);
    }
    // Update is called once per frame
    void Update()
    {
        Timing();
        Spin();
    }
}
