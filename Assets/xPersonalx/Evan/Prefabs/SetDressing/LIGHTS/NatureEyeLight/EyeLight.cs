using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeLight : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Eyeball;  // The game object that will be rotated around to act as the eye.
    public GameObject Eyelid_1; // A game object that will be rotated around the eye Eyeball object to act as an eyelid.
    public GameObject Eyelid_2; // A game object that will be rotated around the eye Eyeball object to act as an eyelid. moves in the opposite direction of the Eyelid_1 object.

    public float WatchDistance; // If the player is within this distance of the eyeball it will automatically begin to watch the player.

    public Vector2 LookAroundTimeRange;  // The range of time that the eye may spend in the 'looking around' state.
    float LookAroundTime;

    public Vector2 WatchingPlayerTimeRange; // The range of time that the eye may spend in the 'watching player' state.
    float WatchingPlayerTime;

    public Vector2 LookAtSpotTimeRange;     // The range of time that the eye may spend looking at something when in the 'watching player' state or the 'looking around' state.
    float LookAtSpotTime;

    public Vector2 LookSpeedRange;          // The speed range of the eyeball when rotating to face its target.
    float LookSpeed;

    public Vector2 LidClosingSpeedRange;   // The speed range of the eyelids when the eyeball 'blinks'.
    float LidClosingSpeed;

    public Vector2 BlinkingTimeRange;       // The range of time that the eye may take before blinking again.
    float BlinkingTime;

    public Vector2 HitReactTimeRange;       // The range of time that the eye may spend in the hit react state after .
    float HitReactTime;

    float LidAngle;

    bool LidClosing;
    bool Blinking;
    bool bIsWatching;

    Vector3 LookPoint;

    enum EyeballState { WatchingPlayer, LookingAround, HitReacting };
    EyeballState currentState;

    GameObject Player;

    Vector3 StartLidAngle;

    void Start()
    {
        if (WatchDistance < 0.01f)
        {
            WatchDistance = 3.0f;
        }

        Player = GameObject.FindWithTag("Player");
        currentState = EyeballState.WatchingPlayer;

        LookAtSpotTime = Random.Range(LookAtSpotTimeRange.x, LookAtSpotTimeRange.y);
        LookAroundTime = Random.Range(LookAroundTimeRange.x, LookAroundTimeRange.y);
        WatchingPlayerTime = Random.Range(WatchingPlayerTimeRange.x, WatchingPlayerTimeRange.y);
        BlinkingTime = Random.Range(BlinkingTimeRange.x, BlinkingTimeRange.y);
        LookSpeed = Random.Range(LookSpeedRange.x, LookSpeedRange.y);
        LidClosingSpeed = Random.Range(LidClosingSpeedRange.x, LidClosingSpeedRange.y);
        HitReactTime = Random.Range(HitReactTimeRange.x, HitReactTimeRange.y);


    }
    void WatchPlayer()
    {
        if (WatchingPlayerTime > 0.0f)
        {
            LookTowards(Player.transform.position, LookSpeed);
            WatchingPlayerTime -= Time.deltaTime;
        }
        else
        {
            currentState = EyeballState.LookingAround;
            WatchingPlayerTime = Random.Range(WatchingPlayerTimeRange.x, WatchingPlayerTimeRange.y);
        }
    }
    void LookAround()
    {
        if (LookAroundTime > 0.0f)
        {
            LookTowards(LookPoint, LookSpeed);
            LookAroundTime -= Time.deltaTime;

            if (LookAtSpotTime > 0.0f)
            {

                LookAtSpotTime -= Time.deltaTime;
            }
            else
            {
                LookPoint = transform.position + (transform.up * 5) + Random.insideUnitSphere * 5;
                LookAtSpotTime = Random.Range(LookAtSpotTimeRange.x, LookAtSpotTimeRange.y); if (LookAtSpotTime > ((LookAtSpotTimeRange.y - LookAtSpotTimeRange.x) * 0.5f) + LookAtSpotTimeRange.x) { LookAtSpotTime = Random.Range(LookAtSpotTimeRange.x, LookAtSpotTimeRange.y); } // this is like this to raise odds of a short time spent on a spot to give the eye a more twitchy behavior
                LookSpeed = Random.Range(LookSpeedRange.x, LookSpeedRange.y);
            }

        }
        else
        {
            currentState = EyeballState.WatchingPlayer;
            LookSpeed = LookSpeedRange.y;
            LidClosingSpeed = LidClosingSpeedRange.y;
            LookAroundTime = Random.Range(LookAroundTimeRange.x, LookAroundTimeRange.y);
        }
    }
    public void Hit()
    {
        currentState = EyeballState.HitReacting;
        LidClosingSpeed = LidClosingSpeedRange.y;
        HitReactTime = Random.Range(HitReactTimeRange.x, HitReactTimeRange.y);

        try { GetComponent<AudioManager_Squash>().Play(); }
        catch { }
    }
    void HitReact()
    {
        if (HitReactTime > 0.0f)
        {
            HitReactTime -= Time.deltaTime;
            if (LidAngle < 90.0f)
            {
                LidAngle += LidClosingSpeed * Time.deltaTime;
            }
            else
            {
                LidAngle = 90.0f;
            }
            if (HitReactTime < 0.0f)
            {
                LidClosingSpeed = LidClosingSpeedRange.x;
            }
        }
        else if (LidAngle > 0.0f)
        {
            LidAngle -= LidClosingSpeed * Time.deltaTime;
        }
        else
        {
            LidAngle = 0.0f;
            currentState = EyeballState.WatchingPlayer;
        }
        Eyelid_1.transform.localEulerAngles = new Vector3(LidAngle - 180, 0.0f, 0.0f);
        Eyelid_2.transform.localEulerAngles = new Vector3(-LidAngle + 90, 0.0f, 0.0f);

    }
    public void Blink()
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
                    BlinkingTime = Random.Range(BlinkingTimeRange.x, BlinkingTimeRange.y);
                    LidAngle = 0.0f;
                }
            }

            Eyelid_1.transform.localEulerAngles = new Vector3(LidAngle - 180, 0.0f, 0.0f);
            Eyelid_2.transform.localEulerAngles = new Vector3(-LidAngle + 90, 0.0f, 0.0f);
        }
        else
        {
            if (BlinkingTime > 0.0f)
            {
                BlinkingTime -= Time.deltaTime;
            }
            else
            {

                LidClosingSpeed = Random.Range(LidClosingSpeedRange.x, LidClosingSpeedRange.y);
                Blinking = true;
            }
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


            if (currentState == EyeballState.WatchingPlayer)
            {
                WatchPlayer();
                Blink();
            }
            else if (currentState == EyeballState.LookingAround)
            {
                LookAround();
                Blink();
                if (Vector3.Distance(transform.position, Player.transform.position) < WatchDistance)
                {
                    currentState = EyeballState.WatchingPlayer;
                }
            }
            else if (currentState == EyeballState.HitReacting)
            {
                HitReact();
                LookTowards(Player.transform.position, LookSpeed);
            }


        }
    }
}
