using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningManager : MonoBehaviour
{
    public GameObject _camera;
    public GameObject _eschaton;
    public GameObject _black;
    public GameObject _text;
    public AudioManager_Universal _audioManager;
    public float _CameraPanSpeed;
    float _blacknessCurrentAlpha = 1;
    float _textCurrentAlpha = 0;
    float _openingLength = 107.0f;
    // Start is called before the first frame update
    /// 1:22
    /// 
    void FadeIn(GameObject image, float speed,float _currentAlpha)
    {
        _currentAlpha = image.GetComponent<MeshRenderer>().material.color.a;
        _currentAlpha += Time.deltaTime * speed;
        if (_currentAlpha > 1)
        {
            _currentAlpha = 1;
        }
        image.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, _currentAlpha);
    }

    void FadeOut(GameObject image, float speed, float _currentAlpha)
    {
        _currentAlpha = image.GetComponent<MeshRenderer>().material.color.a;
        _currentAlpha -= Time.deltaTime * speed;
        if (_currentAlpha < 0)
        { _currentAlpha = 0;}

        image.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, _currentAlpha);
    }
    void OpeningFade()
    {
        FadeOut(_black,0.2f, _blacknessCurrentAlpha);
    }
    void ClosingFade()
    {
        FadeIn(_black, 0.2f, _blacknessCurrentAlpha);
    }
    void Start()
    {
        
    }
    void LoadGameScene()
    {

    }
    public void LookTowards(Transform thisTransform, Vector3 targetLocation, float turnspeed)
    {

        Quaternion targetRotation = Quaternion.LookRotation(targetLocation - thisTransform.position);
        float str;
        str = Mathf.Min(turnspeed * Time.deltaTime, 1);
        thisTransform.rotation = Quaternion.Lerp(thisTransform.rotation, targetRotation, str);

    }
    // Update is called once per frame
    void Update()
    {
        _camera.transform.position = Vector3.MoveTowards(_camera.transform.position,_eschaton.transform.position,_CameraPanSpeed * Time.deltaTime);
        LookTowards(_camera.transform,_eschaton.transform.position,0.02f);
        if (_openingLength > 0.0f)
        {
            if(_openingLength>91)
            {
                OpeningFade();
            }
            if(_openingLength < 103 && _openingLength > 96)
            {
                FadeIn(_text,0.33f, _textCurrentAlpha);
                try
                {
                   AudioManager_Universal audio = GetComponent<AudioManager_Universal>();
                    if(!audio._AudioSource.isPlaying)
                    {
                        audio.Play();
                    }
                }
                catch { }
            }
            if(_openingLength < 13 && _openingLength > 9)
            {
                FadeOut(_text, 0.33f, _textCurrentAlpha);
            }
            if(_openingLength<9)
            {
                ClosingFade();
            }
            _openingLength -= Time.deltaTime;
        }
        else
        {
            LoadGameScene();
        }
    }
}
