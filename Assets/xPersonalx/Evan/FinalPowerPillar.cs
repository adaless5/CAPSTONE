using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPowerPillar : MonoBehaviour, ISaveable
{
    // Start is called before the first frame update
    public PowerPillar[] _pillars;
    public ParticleSystem[] _connectingBeams;
    public GameObject _stonesParent;
    public Rigidbody _rigidbody;
    public CultLight _hover;
    public PillarDeath _death;
    public PuzzleSwitch _triggerSwitch;
    bool _isDefeated;
    int pillarsLeft;

    void PillarBeams()
    {
        if(_pillars[0]._hasFallen)
        { _connectingBeams[2].Stop(); _connectingBeams[3].Stop(); }

        if (_pillars[1]._hasFallen)
        { _connectingBeams[3].Stop(); _connectingBeams[4].Stop(); }

        if (_pillars[2]._hasFallen)
        { _connectingBeams[4].Stop(); _connectingBeams[0].Stop(); }

        if (_pillars[3]._hasFallen)
        { _connectingBeams[0].Stop(); _connectingBeams[1].Stop(); }

        if (_pillars[4]._hasFallen)
        { _connectingBeams[1].Stop(); _connectingBeams[2].Stop(); }
    }
    void RotateStones()
    {
        _stonesParent.transform.Rotate(0,Time.deltaTime * 3,0);
        for(int i = 0; i < _stonesParent.transform.childCount; i++)
        { _stonesParent.transform.GetChild(i).transform.Rotate(0, Time.deltaTime*10, 0); }
    }
    public void PillarBreak()
    {
        if (CheckIsDefeated())
        {
            Die();
        }
        _hover.BobSpeed = 6 - (pillarsLeft);
        _hover.pieceOneSpinSpeed = (6 - (pillarsLeft)) * 7;
        PillarBeams();
    }
    bool CheckIsDefeated()
    {
        pillarsLeft = 0;
        bool noPillarsLeft = true;
        for (int i = 0; i < _pillars.Length; i++)
        {
            if (_pillars[i] && !_pillars[i]._hasFallen)
            {
                noPillarsLeft = false;
                pillarsLeft++;
            }
        }
        return noPillarsLeft;
    }

    void Die() 
    {
        if (_rigidbody)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;
            _rigidbody.velocity = new Vector3();
        }
        if (_hover)
        {
            _hover.bIsActive = false;
        }
        for (int i = 0; i < _stonesParent.transform.childCount; i++)
        {
            _stonesParent.transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
            _stonesParent.transform.GetChild(i).GetComponent<Rigidbody>().useGravity = true;
        }
        _isDefeated = true;
        _stonesParent.transform.DetachChildren();
        SaveData();
        _death.Die();
    }

    private void Update()
    {
        if (!_isDefeated)
        { RotateStones(); }
    }
    void SaveData() 
    {
     SaveSystem.Save(gameObject.name, "isDefeated", gameObject.scene.name, _isDefeated);
    }

    private void Awake()
    {
        LoadDataOnSceneEnter();
    }
    // Update is called once per frame
    public void LoadDataOnSceneEnter() // loads the has fallen bool, and if the pillar has fallen, grabs the position and rotation and calls fall on the pillar.
    {
        _isDefeated = SaveSystem.LoadBool(gameObject.name, "isDefeated", gameObject.scene.name);
        PillarBeams();
        _triggerSwitch.bIsActive = true;
        if (_isDefeated)
        {
            for(int i = 0; i< _connectingBeams.Length;i++)
            { _connectingBeams[i].Stop(); Destroy(_connectingBeams[i]);}
            _triggerSwitch.bIsActive = false;
            Destroy(_stonesParent);
            Destroy(gameObject);
        }
    }
}
