using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;

public class Blade : Equipment
{
    [SerializeField] int Damage { get; } = 25;
    [SerializeField] float KnockBackForce = 250f;
    public ALTPlayerController playerController;

    bool bHasHit;
    public float _bladeDamage;
    Animator _animator;
    bool _bisAttacking;
    GameObject prevHit;

    bool _bClickSet = false;

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

        _animator.enabled = false;

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
        //LoadDataOnSceneEnter();
        MeshRenderer[] meshs = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer obj in meshs)
        {
            obj.enabled = false;
        }
        SkinnedMeshRenderer arm = GetComponentInChildren<SkinnedMeshRenderer>();
        arm.enabled = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (Mouse.current.rightButton.wasReleasedThisFrame) _bClickSet = false;

        if (bIsActive && bIsObtained)
        {
            
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
        

        if (ALTPlayerController.instance.CheckForUseEquipmentInput())
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
            {
                int rand = Random.Range(1, 3);
                _animator.SetBool("Attack" + rand.ToString(), true);
                bHasHit = false;

                StartCoroutine(OnAttack());
                
            }
        }
        else
        {
            _animator.SetBool("Attack1", false);
            _animator.SetBool("Attack2", false);
        }

    }

    IEnumerator OnAttack()
    {

        AudioManager_Sword audioManager = GetComponent<AudioManager_Sword>();
        if ((_animator.GetBool("Attack1") || _animator.GetBool("Attack2"))
            && !audioManager.isPlaying() && _bClickSet == false) audioManager.TriggerSwing();

        _bClickSet = true;
        _bisAttacking = true;

        yield return new WaitForSeconds(0.5f);


        Vector3 pos = ALTPlayerController.instance.GetComponentInChildren<Camera>().gameObject.transform.position;
        RaycastHit[] hit = null;
        hit = Physics.SphereCastAll(pos, 1f, transform.forward, 2f);

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            for (int i = 0; i < hit.Length; i++)
            {
                Collider other = hit[i].collider;
                if (other)
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
                                continue;
                            }
                        }
                        if (other.GetComponentInParent<EyeLight>())
                        {
                            EyeLight obj = other.GetComponentInParent<EyeLight>();
                            if (obj)
                            {
                                obj.Hit();
                                continue;
                            }
                        }


                        if (other.GetComponentInParent<ItemContainer>())
                        {
                            ItemContainer obj = other.GetComponentInParent<ItemContainer>();
                            if (obj)
                            {
                                obj.Break(gameObject.tag);
                                bHasHit = true;
                                continue;

                            }
                        }
                    }
                }
            }
        }
        _bisAttacking = false;
    }

    public override void Activate()
    {
        base.Activate();

        StartCoroutine(EnterBlade());
    }

    IEnumerator EnterBlade()
    {
        yield return new WaitForSeconds(0.55f);
        MeshRenderer[] meshs = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer obj in meshs)
        {
            obj.enabled = true;
        }
        SkinnedMeshRenderer arm = GetComponentInChildren<SkinnedMeshRenderer>();
        arm.enabled = true;
        _animator.enabled = true;
        _animator.SetBool("IsOut", true);
    }
}
