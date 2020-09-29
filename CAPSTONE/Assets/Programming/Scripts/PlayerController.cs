using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera _camera { get; private set; }
    public CharacterController _controller { get; private set; }
    public AnimationCurve _jumpFallOff; //Adjusts the force parameter over the life of the jump.

    //Private members [included in GameSettings]
    KeyCode _jumpKey;
    float _jumpMultiplier;
    float _lookSensitivity;
    KeyCode _runKey;
    float _movementSpeed;
    float _walkSpeed;
    float _runSpeed;
    float _runBuildupMultiplier;
    //

    //Private Members
    bool _isJumping = false;
    float _xAxisClamp = .0f;
    float _slopeForce = 5.0f;
    float _slopeCheckRayLength = 1.5f;
    float _verticalInput;
    float _horizontalInput;
    float _mouseX;
    float _mouseY;

    // Members Anthony's added
    Vector3 _Momentum;
    public GrappleHook _GrappleHook;


    private void Awake()
    {
        //Lock The cursor to the center of the screen.
        Cursor.lockState = CursorLockMode.Locked;

        //Get required components
        _camera = GetComponentInChildren<Camera>(true);
        _controller = GetComponent<CharacterController>();

        //This prevents controller from clipping with object while jumping.
        _controller.skinWidth = .2f;

    }

    private void Start()
    {
        //Apply GameSettings to player controller
        LoadGameSettings();
    }

    private void Update()
    {
        PlayerRotation();
        PlayerMovement();
        Jump();

        if (_controller.isGrounded) _isJumping = false;
    }

    //Handle player head rotation.
    public void PlayerRotation()
    {
        //Get Rotation Inputs
        _mouseX = Input.GetAxis("Mouse X") * _lookSensitivity * Time.deltaTime;
        _mouseY = Input.GetAxis("Mouse Y") * _lookSensitivity * Time.deltaTime;

        //Clamp X axis rotation
        _xAxisClamp += _mouseY;
        if (_xAxisClamp > 90.0f) { _xAxisClamp = 90.0f; _mouseY = 0.0f; ClampXAxisRotationToValue(270.0f); }
        else if (_xAxisClamp < -90.0f) { _xAxisClamp = -90.0f; _mouseY = 0.0f; ClampXAxisRotationToValue(90.0f); }

        //Handle rotation
        _camera.transform.Rotate(Vector3.left * _mouseY); // up/down
        transform.Rotate(Vector3.up * _mouseX);           // left/right
    }

    //Handle player directional movement.
    public void PlayerMovement()
    {
        //Get Movement Inputs
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        //Hand Movement Input
        _controller.SimpleMove(Vector3.ClampMagnitude(
            (transform.forward * _verticalInput) + (transform.right * _horizontalInput), 1.0f) * _movementSpeed);

        //Add Downward force while walking on a slope to prevent character from bouncing.
        if (isMoving() && isOnSlope())
        {
            _controller.Move(Vector3.down * _controller.height * .5f * _slopeForce * Time.deltaTime);
        }

        SetMovementSpeed();
    }

    //Handles the players movement speed. lerps between walk/run speed according to inputs.
    private void SetMovementSpeed()
    {
        if (Input.GetKey(_runKey)) _movementSpeed = Mathf.Lerp(_movementSpeed, _runSpeed, Time.deltaTime * _runBuildupMultiplier);
        else _movementSpeed = Mathf.Lerp(_movementSpeed, _walkSpeed, Time.deltaTime * _runBuildupMultiplier);
    }

    //Triggers a jump event when jump input is pressed
    private void Jump()
    {
        if ((_controller.collisionFlags & CollisionFlags.Below) != 0) _isJumping = false;

        if (Input.GetKeyDown(_jumpKey) && !_isJumping)
        {
            _isJumping = true;
            StartCoroutine(JumpEvent_Coroutine());
        }
    }

    //Handles a jump event.
    //doing this via coroutine alows for the event to last multiple frames without locking the main thread.
    private IEnumerator JumpEvent_Coroutine()
    {
        _controller.slopeLimit = 90.0f; //Increasing slope limit prevents character from cliping into object when jumping while hugging object.
        float timeInAir = 0.0f;

        do
        { //Apply jump force until player hits ceiling or returns to ground

            float jumpForce = _jumpFallOff.Evaluate(timeInAir);

            //Apply Jump force

            if (_GrappleHook.m_PlayerState == GrappleHook.PlayerState.Idle || _GrappleHook.m_PlayerState == GrappleHook.PlayerState.GrappleDeployed)
            {
                _controller.Move(Vector3.up * jumpForce * _jumpMultiplier * Time.deltaTime);
            }
            else if (_GrappleHook.m_PlayerState == GrappleHook.PlayerState.Grappling)
            {
                _controller.Move(_camera.transform.forward * jumpForce * _jumpMultiplier * Time.deltaTime);
            }

                //Break jump if player hits ceiling, and apply downward force to prevent player from sticking to the ceiling for a second.
                if ((_controller.collisionFlags & CollisionFlags.Above) != 0)
            {
                _controller.Move(Vector3.down * _jumpMultiplier * Time.deltaTime);
                break;
            }

            timeInAir += Time.deltaTime;

            yield return null;

        } while (!_controller.isGrounded && _isJumping);

        _controller.slopeLimit = 45.0f; //Reset slope limit to default 
    }

    //Clamps the X axis to max/min rotation value. Prevents camera from rotating past its clamp at higher velocities.
    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 rot = _camera.transform.eulerAngles;
        rot.x = value;
        _camera.transform.eulerAngles = rot;
    }

    //Returns true if the player is on a sloped surface.
    private bool isOnSlope()
    {
        if (_isJumping) return false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _controller.height * .5f * _slopeCheckRayLength))
        {
            if (hit.normal != Vector3.up) return true; //if normal from raycast doesnt point straight up, then player is on a sloped surface.
        }
        return false;
    }

    //Returns true if movement input active.
    private bool isMoving()
    {
        if (_verticalInput != 0 || _horizontalInput != 0) return true;
        return false;
    }

    //Loads game settings from Editor Prefs.
    private void LoadGameSettings()
    {
        _jumpKey = GameSettings.jumpKey;
        _jumpMultiplier = GameSettings.jumpMultiplier;
        _lookSensitivity = GameSettings.lookSensitivity;
        _runKey = GameSettings.runKey;
        _movementSpeed = GameSettings.walkSpeed;
        _walkSpeed = GameSettings.walkSpeed;
        _runSpeed = GameSettings.runSpeed;
        _runBuildupMultiplier = GameSettings.runBuildupMultiplier;
    }

    public bool CheckForGrappleInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public bool CheckForJumpInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public void ApplyMomentum(Vector3 direction, float speed, float grappleMultiplier, float jumpMultiplier)
    {
        _Momentum = direction * speed * grappleMultiplier;
        _Momentum += Vector3.up * jumpMultiplier;
    }
}
