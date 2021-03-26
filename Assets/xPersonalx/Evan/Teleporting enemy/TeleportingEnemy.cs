using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportingEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public enum BehaviourState { Waiting, Follow, Attack };

    public ParticleSystem LocalTeleportParticles;
    public ParticleSystem TargetTeleportParticles;

    public float _AcceptiblePlayerDistance;
    public Vector2 _TeleportTimeRange;
    public float _LookSpeed;
    public float _ReappearTime;
    public float _TeleportDistance;
    public bool bIsTeleporting;
    float teleporterTime;
    float reappearTime;
    Vector3 TeleportLocation;
    GameObject player;
    bool bPlayerClose;
    bool bHasDisappeared;
    public BehaviourState _behaviourState;
    void Start()
    {
        teleporterTime = Random.Range(_TeleportTimeRange.x, _TeleportTimeRange.y);
        reappearTime = _ReappearTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent != null)
        {
            if (other.gameObject.transform.parent.tag == "Player")
            {

                bIsTeleporting = true;

            }
        }
    }

    void AttackState()
    {

        if (teleporterTime < 0.0f)
        {
            if (!bHasDisappeared)
            {
                Disappear();
            }
            Teleport();
        }
        else
        {
            teleporterTime -= Time.deltaTime;
        }
    }
    void Teleport()
    {

        if (reappearTime < 0.0f)
        {
            RaycastHit hit;
            if (Vector3.Distance(player.transform.position, LocalTeleportParticles.transform.position) < _AcceptiblePlayerDistance)
            {
                TeleportLocation = player.transform.position + (Random.insideUnitSphere * 10.0f);
                TeleportLocation.y = player.transform.position.y + 2.0f;
                bPlayerClose = true;
            }
            else
            {
                TeleportLocation = (LocalTeleportParticles.transform.position + (transform.forward * _AcceptiblePlayerDistance)) + (Random.insideUnitSphere * 5.0f);
                TeleportLocation.y = player.transform.position.y + 2.0f;
                bPlayerClose = false;
            }
            if (Physics.SphereCast(TeleportLocation, 2.0f, Vector3.down, out hit, 2.0f))
            {
                transform.position = TeleportLocation;
                TargetTeleportParticles.transform.position = TeleportLocation;
                TargetTeleportParticles.Play();

                if (bPlayerClose)
                { teleporterTime = Random.Range(_TeleportTimeRange.x, _TeleportTimeRange.y); }
                else
                { teleporterTime = _TeleportTimeRange.x; }

                reappearTime = _ReappearTime;
                bHasDisappeared = false;
            }
        }
        else
        {
            reappearTime -= Time.deltaTime;
        }

    }
    void Disappear()
    {

        LocalTeleportParticles.transform.position = transform.position;
        LocalTeleportParticles.Play();
        transform.position = new Vector3(transform.position.x, -100.0f, transform.position.z);
        bHasDisappeared = true;


    }
    public void LookTowards(Transform thisTransform, Vector3 target, float turnspeed)
    {

        Quaternion targetRotation = Quaternion.LookRotation(target - thisTransform.position);
        float str;
        str = Mathf.Min(turnspeed * Time.deltaTime, 1);
        thisTransform.rotation = Quaternion.Lerp(thisTransform.rotation, targetRotation, str);

    }
    // Update is called once per frame
    void Update()
    {
        if (bIsTeleporting)
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            else
            {
                AttackState();
                LookTowards(transform, player.transform.position, _LookSpeed);
            }
        }

    }
}
