using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportingEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public enum BehaviourState { Waiting, Follow, Attack };

    public ParticleSystem LocalTeleportParticles;
    public ParticleSystem TargetTeleportParticles;

    public Vector2 TeleportTimeRange;
    public float _LookSpeed;
    public float _ReappearTime;
    public bool bIsTeleporting;
    float teleporterTime;
    Vector3 TeleportLocation;
    GameObject player;
    bool bIsFollowing;
    public BehaviourState _behaviourState;
    void Start()
    {
        teleporterTime = Random.Range(TeleportTimeRange.x, TeleportTimeRange.y);
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
    void Teleport()
    {
        teleporterTime -= Time.deltaTime;

        if (teleporterTime < 0.0f)
        {
            RaycastHit hit;
            TeleportLocation = player.transform.position + (Random.insideUnitSphere * 10.0f);
            TeleportLocation.y = player.transform.position.y + 2.0f;

            if (Physics.SphereCast(TeleportLocation, 2.0f, Vector3.down, out hit, 2.0f))
            {
                LocalTeleportParticles.transform.position = transform.position;
                LocalTeleportParticles.Play();
                transform.position = TeleportLocation;
                TargetTeleportParticles.transform.position = TeleportLocation;
                TargetTeleportParticles.Play();
                teleporterTime = Random.Range(TeleportTimeRange.x, TeleportTimeRange.y);
            }

        }
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
                Teleport();
            LookTowards(transform, player.transform.position, _LookSpeed);
            }
        }

    }
}
