using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeLight : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Eyeball;
    public GameObject Eyelid_1;
    public GameObject Eyelid_2;

    public float WatchDistance;
    public float LookSpeed;

    public bool bIsWatching;

    public Vector3 LookPoint;

    public float LookAtSpotTime;

    public float LookAroundTime;
    public float LookAtPlayerTime;

    public float BlinkTimer;
    public float LidAngle;
    public bool LidClosing;

    public float LidClosingSpeed;

    public bool Blinking;

    GameObject Player;

    Vector3 StartLidAngle;

    void Start()
    {
        if (WatchDistance < 0.01f)
        {
            WatchDistance = 3.0f;
        }
        Player = GameObject.FindWithTag("Player");

        LookAtSpotTime = Random.Range(0.5f, 3.0f);

        LookAroundTime = Random.Range(3.0f, 10.0f);
        LookAtPlayerTime = Random.Range(2.0f, 7.0f);
        LidClosing = true;
        BlinkTimer = 0.01f;


    }
    void WatchPlayer()
    {
        if (LookAtPlayerTime > 0.0f)
        {
            LookTowards(Player.transform.position, LookSpeed);
            LookAtPlayerTime -= Time.deltaTime;
        }
        else
        {
            bIsWatching = false;
            LookAtPlayerTime = Random.Range(2.0f, 7.0f);
        }
    }
    void LookAround()
    {
        if (LookAroundTime > 0.0f)
        {
            LookAroundTime -= Time.deltaTime;
            LookTowards(LookPoint, LookSpeed);

            if (LookAtSpotTime > 0.0f)
            {

                LookAtSpotTime -= Time.deltaTime;
            }
            else
            {
                LookPoint = transform.position + (transform.up * 5) + Random.insideUnitSphere * 5;
                LookAtSpotTime = Random.Range(0.5f, 3.0f);
            }

        }
        else
        {
            bIsWatching = true;
            LookAroundTime = Random.Range(3.0f, 10.0f);
        }
    }

    void Blink()
    {


        if (Blinking)
        {
            if (LidClosing)
            {
                LidAngle += LidClosingSpeed * Time.deltaTime;
                if (LidAngle > 89.9f)
                {
                    LidClosing = false;
                    LidAngle = 90.0f;
                }
            }
            else
            {
                LidAngle -= LidClosingSpeed * Time.deltaTime;
                if (LidAngle <= 0.0f)
                {
                    LidClosing = true;
                    Blinking = false;
                    BlinkTimer = Random.Range(0.5f, 9.0f);
                    LidAngle = 0.0f;
                }
            }

        Eyelid_1.transform.localEulerAngles = new Vector3(LidAngle - 180, 0.0f, 0.0f);
        Eyelid_2.transform.localEulerAngles = new Vector3(-LidAngle + 90, 0.0f, 0.0f);
        }
        else
        {
            if (BlinkTimer > 0.0f)
            {
                BlinkTimer -= Time.deltaTime;
            }
            else Blinking = true;
        }

    }

    void LookTowards(Vector3 target, float turnspeed)
    {

        Quaternion targetRotation = Quaternion.LookRotation(target - Eyeball.transform.position);
        float str;
        str = Mathf.Min(turnspeed * Time.deltaTime, 1);
        Eyeball.transform.rotation = Quaternion.Lerp(Eyeball.transform.rotation, targetRotation, str);

    }
    // Update is called once per frame
    void Update()
    {
        if (Player == null)
        {
            Player = GameObject.FindWithTag("Player");
        }
        else
        {
            if (Vector3.Distance(transform.position, Player.transform.position) < WatchDistance)
            {
                bIsWatching = true;
            }
            if (bIsWatching)
            {
                WatchPlayer();
            }
            else
            {
                LookAround();
            }

            Blink();

        }
    }
}
