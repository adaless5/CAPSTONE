using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Gland Gun animations logic
public class GG_Animations : Weapon_Animations
{

    public Animator _GGAnimator;
    float _GGreloadAnimClipLength = 2.0f;

    private void Awake()
    {
        _GGAnimator = GetComponentInChildren<Animator>();
        Initialize(_GGAnimator, _GGreloadAnimClipLength);    
    } 
}
