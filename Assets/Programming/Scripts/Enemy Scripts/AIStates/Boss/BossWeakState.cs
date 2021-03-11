using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakState : BossState
{
    float _weakTimer = 3.0f;
    GameObject _bossWeakPoint;
    public BossWeakState(GameObject boss) : base(boss)
    {
        Debug.Log("Weak State Activated");
        foreach (Transform b in boss.transform)
        {
            if (b.gameObject.name == "Weak")
            {
                _bossWeakPoint = b.gameObject;
            }
        }
    }

    public override void Enter()
    {
        base.Enter();
        _bossWeakPoint.SetActive(true);

    }

    public override void Update()
    {
        base.Update();
        _weakTimer -= Time.deltaTime;
        if (_weakTimer <= 0.00f)
        {
            _nextState = new UCState(_currentEnemy);
            _stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
        _currentEnemy.GetComponent<BossAI>().CallOnWeakStateEnded();
        _bossWeakPoint.SetActive(false);
    }
}
