using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("Damage Settings")]
    public float m_damageAmount = 10.0f;

    [Header("Weapon Settings")]
    public int m_ammoAmount = 6; //not being used currently
    public float m_fireRate = 10.0f;
    public float m_hitImpact = 50.0f;
    public float m_weaponRange = 50.0f;

    private float m_fireStart = 0.0f;

    [Header("UI Elements - ParticleFX and Reticule")]
    public ParticleSystem muzzleFlash;
    public GameObject impactFX;
    public Animator reticuleAnimator;   
    

    [Header("Camera Settings")]
    public Camera gunCamera;


    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= m_fireStart)
        {
            m_fireStart = Time.time + 1.0f / m_fireRate;
            OnShoot();
        }

        OnTarget();
    }

    void OnShoot()
    {
        muzzleFlash.Play();

        RaycastHit hitInfo;
        if (Physics.Raycast(gunCamera.transform.position, gunCamera.transform.forward, out hitInfo, m_weaponRange))
        {
            Debug.Log(hitInfo.transform.name);

            //Only damages if asset has "Target" script
            Target target = hitInfo.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(m_damageAmount);
                reticuleAnimator.SetTrigger("isTargetted");
            }
            else
            {
                reticuleAnimator.SetTrigger("isTargetted");
            }


            //Force of impact on hit
            if (hitInfo.rigidbody != null)
            {
                hitInfo.rigidbody.AddForce(-hitInfo.normal * m_hitImpact);
            }

            //Particle effects on hit
            GameObject hitImpact = Instantiate(impactFX, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(hitImpact, 2.0f);
        }
    }

   //VR - Plays Animation to focus reticule on targetting
    void OnTarget()
    {
        RaycastHit targetInfo;
        if (Physics.Raycast(gunCamera.transform.position, gunCamera.transform.forward, out targetInfo, m_weaponRange))
        {
            Debug.Log(targetInfo.transform.name);

       
            Target target = targetInfo.transform.GetComponent<Target>();
            if (target != null)
            {
                reticuleAnimator.SetBool("isTargetted", true);
            }
            else
            {
                reticuleAnimator.SetBool("isTargetted", false);
            }
        }
    }
}
