using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakState : BossState
{
    float _weakTimer = 5.833f;
    GameObject _bossWeakPoint;
    public BossWeakState(GameObject boss) : base(boss)
    {
        _bossStateName = BOSSSTATENAME.WEAK;
        Debug.Log("Weak State Activated");
        //foreach (Transform b in boss.transform)
        //{
        //    if (b.gameObject.name == "Weak")
        //    {
        //        _bossWeakPoint = b.gameObject;
        //    }
        //}
        _bossWeakPoint = _currentEnemy.GetComponent<BossAI>()._weakPoint;
    }

    public override void Enter()
    {
        base.Enter();
        _weakTimer = 5.833f;
        _bossWeakPoint.SetActive(true);

    }

    public override void Update()
    {
        base.Update();
        _weakTimer -= Time.deltaTime;
        if (_weakTimer <= 0.00f)
        {
            _nextState = new BossArmSmashState(_currentEnemy, _currentEnemy.GetComponent<BossAI>().GetBossArm());
            _stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();        
        _bossWeakPoint.SetActive(false);
    }
}
