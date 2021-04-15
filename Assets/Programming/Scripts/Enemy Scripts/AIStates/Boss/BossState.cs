using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BOSSSTATENAME
{
    HOMINGATTACK, DRONESPAWNING, ARMSMASH, WEAK
};

public class BossState : State
{

    public BossState(GameObject boss)
    {
        _currentEnemy = boss;
    }

    public BOSSSTATENAME _bossStateName;

}
