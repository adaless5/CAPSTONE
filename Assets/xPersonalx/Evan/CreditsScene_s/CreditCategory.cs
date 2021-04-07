using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditCategory : MonoBehaviour
{
    // Start is called before the first frame update

    public CreditObject[] _names;
    public CreditObject _categoryTitle;

    public bool _allNamesFaded;
    public float _timeUntilNextName;
    public float _nameMovementSpeed;
    float _currentTimeUntilNextName;
    public bool _isActive;

    int activeIndex;

    void Start()
    {

    }

    bool AllNamesFaded()
    {
        bool areThey = true;
        for (int i = 0; i < _names.Length; i++)
        {
            if (_names[i]._isVisible || !_names[i]._startFadeOut)
            {
                areThey = false;
            }
        }
        return areThey;
    }
    public void Activate()
    {
        _isActive = true;
        _names[0].StartFadeIn();
        _categoryTitle.StartFadeIn();
        activeIndex = 1;
    }
    private void Awake()
    {
        _currentTimeUntilNextName = _timeUntilNextName;

    }
    void MoveCredits()
    {
        for (int i = 0; i < _names.Length; i++)
        {
            if (_names[i]._isActive)
            {
                _names[i].gameObject.transform.position = new Vector3(_names[i].gameObject.transform.position.x + (Time.deltaTime * _nameMovementSpeed), _names[i].gameObject.transform.position.y, _names[i].gameObject.transform.position.z);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isActive)
        {
            if (_currentTimeUntilNextName > 0.0f)
            {
                _currentTimeUntilNextName -= Time.deltaTime;
            }
            else if (activeIndex < _names.Length)
            {
                //_names[activeIndex].gameObject.SetActive(true);
                _names[activeIndex].StartFadeIn();
                _currentTimeUntilNextName = _timeUntilNextName;
                activeIndex++;
            }
            else if (AllNamesFaded())
            {
                _allNamesFaded = true;
                _categoryTitle.StartFadeOut();
            }
            MoveCredits();
        }
    }
}
