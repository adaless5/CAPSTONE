using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeapingEnemyState : State
{
    protected LeapingEnemyAI _thisLeapingEnemy;
    protected GameObject _thisEnemyObject;

    public LeapingEnemyState(GameObject thisEnemyObject, LeapingEnemyAI thisLeapingEnemy,  Transform playerposition)
    {
        _thisEnemyObject = thisEnemyObject;
        _playerPos = playerposition;
        _thisLeapingEnemy = thisLeapingEnemy;
        _stage = EVENT.ENTER;
    }
}
