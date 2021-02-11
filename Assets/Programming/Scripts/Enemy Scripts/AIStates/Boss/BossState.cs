using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : State
{
    public BossState(GameObject boss)
    {
        _currentEnemy = boss;
    }

}
