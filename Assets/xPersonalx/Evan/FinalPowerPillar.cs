using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPowerPillar : MonoBehaviour, ISaveable
{
    // Start is called before the first frame update
    public PowerPillar[] _pillars;

    public Rigidbody _rigidbody;
    public CultLight _hover;
    public PillarDeath _death;
    bool _isDefeated;
    int pillarsLeft;

    bool CheckIsDefeated()
    {
        pillarsLeft = 0;
        bool noPillarsLeft = true;
        for (int i = 0; i < _pillars.Length; i++)
        {
            if (_pillars[i] && !_pillars[i].GetIsDefeated())
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
        _isDefeated = true;
      SaveData();
        _death.Die();
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
    void Update()
    {
        if (CheckIsDefeated() && !_isDefeated)
        {
            Die();
        }
        _hover.BobSpeed = 6 - (pillarsLeft);
        _hover.pieceOneSpinSpeed = (6 - (pillarsLeft)) * 7;
    }
    public void LoadDataOnSceneEnter() // loads the has fallen bool, and if the pillar has fallen, grabs the position and rotation and calls fall on the pillar.
    {
        _isDefeated = SaveSystem.LoadBool(gameObject.name, "isDefeated", gameObject.scene.name);

        if (_isDefeated)
        {
            Destroy(gameObject);
        }
    }
}
