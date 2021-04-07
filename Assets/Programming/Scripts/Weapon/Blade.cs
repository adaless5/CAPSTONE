using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Blade : Equipment
{
    [SerializeField] int Damage { get; } = 25;
    [SerializeField] float KnockBackForce = 1500f;
    public ALTPlayerController playerController;

    bool bHasHit;
    public float _bladeDamage;
    Animator _animator;
    BoxCollider _hitbox;
    bool _bisAttacking;
    GameObject prevHit;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        if (playerController == null)
        {
            playerController = FindObjectOfType<ALTPlayerController>();
        }

        _bisAttacking = false;
        _animator = GetComponentInChildren<Animator>();
        _hitbox = GetComponentInChildren<BoxCollider>();

        _animator.enabled = false;
        _hitbox.enabled = false;
        _hitbox.isTrigger = true;
        MeshRenderer[] meshs = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer obj in meshs)
        {
            obj.enabled = false;
        }
        SkinnedMeshRenderer arm = GetComponentInChildren<SkinnedMeshRenderer>();
        arm.enabled = false;
    }

    void Awake()
    {
        LoadDataOnSceneEnter();
        MeshRenderer[] meshs = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer obj in meshs)
        {
            obj.enabled = false;
        }
    }

    // Update is called once per frame
    public override void Update()
    {

        if (bIsActive && bIsObtained)
        {
            MeshRenderer[] meshs = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer obj in meshs)
            {
                obj.enabled = true;
            }
            SkinnedMeshRenderer arm = GetComponentInChildren<SkinnedMeshRenderer>();
            arm.enabled = true;

            _animator.enabled = true;
            _animator.SetBool("IsOut", true);


            UseTool();
        }
        else if (!bIsActive)
        {
            _animator.SetBool("IsOut", false);
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("SwapOut") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .90f)
            {
            _animator.enabled = false;
            MeshRenderer[] meshs = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer obj in meshs)
            {
                obj.enabled = false;
            }
                SkinnedMeshRenderer arm = GetComponentInChildren<SkinnedMeshRenderer>();
                arm.enabled = false;
                _hitbox.enabled = false;
            }
        }

        if (ALTPlayerController.instance.GetIsWalking() && ALTPlayerController.instance.CheckForSprintInput() == false)
        {
            _animator.SetBool("IsWalking", true);
            _animator.SetBool("IsSprinting", false);
            _animator.SetBool("IsIdle", false);

        }
        else if (ALTPlayerController.instance.CheckForSprintInput() == true)
        {
            _animator.SetBool("IsWalking", false);
            _animator.SetBool("IsSprinting", true);
            _animator.SetBool("IsIdle", false);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
            _animator.SetBool("IsSprinting", false);
            _animator.SetBool("IsIdle", true);
        }
    }

    public override void UseTool()
    {
        //if (playerController.CheckForUseEquipmentInput() && _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        //{
        //    _animator.SetBool("attacking", true);
        //    _hitbox.enabled = true;
        //    bHasHit = false;
        //}
        //if (_animator.GetCurrentAnimatorStateInfo(0).IsName("BladeAttacking"))
        //{
        //    _animator.SetBool("attacking", false);
        //    _hitbox.enabled = false;
        //}
        if (ALTPlayerController.instance.CheckForUseEquipmentInput())
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
            {
                Debug.Log("Attack");
                int rand = Random.Range(1, 3);
                _animator.SetBool("Attack" + rand.ToString(), true);
                _hitbox.enabled = true;
                bHasHit = false;
            }
        }
        else 
        {
            _animator.SetBool("Attack1", false);
            _animator.SetBool("Attack2", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {

            if (other.GetComponentInParent<Health>())
            {
                Health target = other.transform.GetComponent<Health>();
                if (target != null)
                {
                    target.TakeDamage(_bladeDamage);
                    bHasHit = true;
                }

                if (other.GetComponent<Rigidbody>())
                {
                    Rigidbody bodytarget = other.transform.GetComponent<Rigidbody>();

                    if (bodytarget != null)
                    {
                        Vector3 hitDir = playerController.transform.position - bodytarget.transform.position;
                        bodytarget.AddForce(hitDir.normalized * -KnockBackForce);
                    }
                }
            }
            if (!bHasHit && other.GetComponentInParent<DestructibleObject>())
            {
                DestructibleObject obj = other.GetComponentInParent<DestructibleObject>();
                if (obj)
                {
                    obj.Break(gameObject.tag);
                    bHasHit = true;
                    return;
                }
            }
            if (other.GetComponentInParent<EyeLight>())
            {
                EyeLight obj = other.GetComponentInParent<EyeLight>();
                if (obj)
                {
                    obj.Hit();
                    return;
                }
            }


            if (other.GetComponentInParent<ItemContainer>())
            {
                ItemContainer obj = other.GetComponentInParent<ItemContainer>();
                if (obj)
                {
                    obj.Break(gameObject.tag);
                    bHasHit = true;
                    return;

                }
            }
            _hitbox.enabled = false;
        }
    }
}
