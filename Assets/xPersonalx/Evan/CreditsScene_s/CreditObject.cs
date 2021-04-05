using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditObject : MonoBehaviour
{
    // Start is called before the first frame update

    public float _fadeSpeed;
    public GameObject _startImage;
    public GameObject _hitImage;
    public ParticleSystem _hitEffects;

    float _currentAlpha;
    public bool _isActive;

    bool _isHit;
    public bool _isVisible;
    bool _startFadeIn;
    public bool _startFadeOut;

    public void StartFadeIn()
    {
        _startFadeOut = false;
        _startFadeIn = true;
        _isActive = true;
    }
    public void StartFadeOut()
    {
        _startFadeOut = true;
        _startFadeIn = false;
    }
    void FadeIn(GameObject image)
    {
        //_currentAlpha = image.GetComponent<MeshRenderer>().material.color.a;
        _currentAlpha += Time.deltaTime * _fadeSpeed;
        if (_currentAlpha > 1)
        { _currentAlpha = 1; _isVisible = true;}
        image.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, _currentAlpha);
    }

    void FadeOut(GameObject image)
    {
        //_currentAlpha = image.GetComponent<MeshRenderer>().material.color.a;x
        _currentAlpha -= Time.deltaTime * _fadeSpeed;
        if (_currentAlpha < 0)
        { _currentAlpha = 0; _isVisible = false; _isActive = false; }
        gameObject.layer = 2;
        image.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, _currentAlpha);
    }
    private void Awake()
    {
        _startImage.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 0);
        _hitImage.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 0);
        _startImage.SetActive(true);
        _hitImage.SetActive(false);
        StartFadeOut();
        _hitEffects.Stop();
    }

    public void Hit(Vector3 Position)
    {
        if (!_isHit && _isVisible)
        {
            _hitEffects.transform.position = Position;
            _hitEffects.Play();
            _startImage.SetActive(false);
            _hitImage.SetActive(true);
            _isHit = true;
            gameObject.layer = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if(_isVisible && _startFadeOut)
        {
            FadeOut(_startImage);
            FadeOut(_hitImage);
        }
        else if (!_isVisible && _startFadeIn)
        {
            FadeIn(_startImage);
            FadeIn(_hitImage);
        }
        else if(!_isVisible && _startFadeOut)
        {
            _startImage.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 0);
            _hitImage.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 0);

        }

    }
}
