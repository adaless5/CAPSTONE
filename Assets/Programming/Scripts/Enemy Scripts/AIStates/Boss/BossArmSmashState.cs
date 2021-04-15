using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArmSmashState : BossState
{
    GameObject _armSmash;
    float _armTimer = 1.0f;
    float _coolDownTimer = 2.6f;
    float fadeSpeed = 0.6f;
    public BossArmSmashState(GameObject boss, GameObject armsmash) : base(boss)
    {
        _bossStateName = BOSSSTATENAME.ARMSMASH;
        _armSmash = armsmash;
    }

    public override void Enter()
    {
        Debug.Log("HE's GONNA SMASH");
        base.Enter();
        _armSmash.GetComponent<BossArm>().ShowArm();
        _currentEnemy.GetComponent<BossAI>().SetArmSmashAnimation();
    }

    public override void Update()
    {
        base.Update();
        _armTimer -= Time.deltaTime;
        if (_armTimer <= 0.0f)
        {
            _coolDownTimer -= Time.deltaTime;
            _armSmash.GetComponent<BoxCollider>().enabled = true;
            if (_coolDownTimer <= 0.0f)
            {
                _armTimer = 3.0f;
                _coolDownTimer = 2.6f;
                _armSmash.GetComponent<BoxCollider>().enabled = false;
                _armSmash.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 0);
                _armSmash.GetComponent<MeshRenderer>().enabled = false;
                _nextState = new UCState(_currentEnemy);
                _stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        _currentEnemy.GetComponent<BossAI>().CallOnMeleeAttackEnded();
    }
}
