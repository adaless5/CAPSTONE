using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade_Animations : Weapon_Animations
{
    public Animator _GrenadeAnimator;
    float _GGreloadAnimClipLength = 2.0f;

    private void Awake()
    {
        _GrenadeAnimator = GetComponentInChildren<Animator>();
        Initialize(_GrenadeAnimator, _GGreloadAnimClipLength);
    }
}
