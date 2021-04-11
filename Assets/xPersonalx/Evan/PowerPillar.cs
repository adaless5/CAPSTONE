using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPillar : MonoBehaviour, ISaveable
{
    // Start is called before the first frame update
    public PuzzleSwitch[] _switches;
    public FinalPowerPillar _finalPillar;
    public ParticleSystem _livingEffect;
    public ParticleSystem _deadEffect;

    Rigidbody _rigidbody;
    CultLight _hover;

    float saveTime = 15;

    public bool _hasFallen;

    bool IsDefeated()
    {
        for(int i = 0; i < _switches.Length; i ++)
        {
            if(!_switches[i].GetIsActive())
            {
                return false;
            }
        }
        return true;
    }

    void Fall() // called when it is time for the pillar to fall to stop the pillar from floating and flip the hasFallen bool. 
    {
        if(_rigidbody)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;
        }

        if(_hover)
        {
            _hover.enabled = false;
        }

        _hasFallen = true;

        GetComponentInChildren<MeshRenderer>().materials[1].SetColor("_EmissiveColor", new Color(0,0,0));
        GetComponentInChildren<MeshRenderer>().materials[1].SetColor("_Color", new Color(0, 0, 0));

        GetComponentInChildren<MeshRenderer>().materials[3].SetColor("_EmissiveColor", new Color(0,0,0));
        GetComponentInChildren<MeshRenderer>().materials[3].SetColor("_Color", new Color(0, 0, 0));

        SaveData(transform.position, transform.forward);

        _finalPillar.PillarBreak();
        _livingEffect.Stop();
        _deadEffect.Play();
    }

    void CountLandedSave() // counts for ten seconds to give the pillar some time to fall, then saves the position again so the pillar loads up in the same place it landed.
    {
        if(_hasFallen && saveTime > 0)
        {
            saveTime -= Time.deltaTime;
            if(saveTime <= 0)
            {
              SaveData(transform.position,transform.forward);
            }
        }
    }

    void SaveData(Vector3 pos, Vector3 rot) // saves the position and rotation of the pillar, as well as the hasFallen bool
    {
        SaveSystem.Save(gameObject.name, "hasFallen", gameObject.scene.name, _hasFallen);
        
        SaveSystem.Save(gameObject.name, "posX", gameObject.scene.name, pos.x);
        SaveSystem.Save(gameObject.name, "posY", gameObject.scene.name, pos.y);
        SaveSystem.Save(gameObject.name, "posZ", gameObject.scene.name, pos.z);

        SaveSystem.Save(gameObject.name, "rotX", gameObject.scene.name, rot.x);
        SaveSystem.Save(gameObject.name, "rotY", gameObject.scene.name, rot.y);
        SaveSystem.Save(gameObject.name, "rotZ", gameObject.scene.name, rot.z);
    }

    public bool GetIsDefeated()
    {
        return _hasFallen;
    }
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _hover = GetComponent<CultLight>();
        LoadDataOnSceneEnter();
    }
    // Update is called once per frame
    void Update()
    {
        if(IsDefeated())
        {
            if (!_hasFallen)
            { 
                Fall(); 
            }
            CountLandedSave();
        }
    }
    public void LoadDataOnSceneEnter() // loads the has fallen bool, and if the pillar has fallen, grabs the position and rotation and calls fall on the pillar.
    {
        _hasFallen = SaveSystem.LoadBool(gameObject.name, "hasFallen", gameObject.scene.name);

        if (_hasFallen)
        {
            Vector3 startPos = new Vector3();
            startPos.x = SaveSystem.LoadFloat(gameObject.name, "posX", gameObject.scene.name);
            startPos.y = SaveSystem.LoadFloat(gameObject.name, "posY", gameObject.scene.name);
            startPos.z = SaveSystem.LoadFloat(gameObject.name, "posZ", gameObject.scene.name);

            Vector3 startRot = new Vector3();
            startRot.x = SaveSystem.LoadFloat(gameObject.name, "rotX", gameObject.scene.name);
            startRot.y = SaveSystem.LoadFloat(gameObject.name, "rotY", gameObject.scene.name);
            startRot.z = SaveSystem.LoadFloat(gameObject.name, "rotZ", gameObject.scene.name);

            transform.position = startPos;
            transform.forward = startRot;
            _livingEffect.Stop();
            Fall();
        }
        else _livingEffect.Play();
    }
}
