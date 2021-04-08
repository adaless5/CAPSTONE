using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DG_Animations : Weapon_Animations
{
    public Animator _DGAnimator;
    float _DGreloadAnimClipLength = 2.2f;

    private void Awake()
    {
        _DGAnimator = GetComponentInChildren<Animator>();
        Initialize(_DGAnimator, _DGreloadAnimClipLength);       
    }
}
