
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnPlayerJumped;
    public event Action OnPlayerGrounded;
    public event Action<PlayerState> OnPlayerStateChanged;
    private StateController _stateController;
    private Rigidbody _playerRigidBody;
    private CapsuleCollider _capsuleCollider;

    private float _startingMovementSpeed, _startingJumpForce;

    private float _horizontalInput, _verticalInput;
    private Vector3 _movementDirection;

    [Header("References")]
    [SerializeField] private Transform _orientationTransform;

    [Header("Movement Settings")]
    [SerializeField] private float _accelerationForce = 25f;
    [SerializeField] private float _maxMoveSpeed = 6f;
    [SerializeField] private KeyCode _movementKey;

    [Header("Jump Settings")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _jumpCooldown = 0.5f;
    [SerializeField] private float _airMultiplier;
    [SerializeField] private float _airDrag;
    private bool _canJump = true;
    [SerializeField] private float _jumpBufferTime = 0.2f;
    private float _jumpBufferCounter = 0f;

    [Header("Slide Settings")]
    [SerializeField] private KeyCode _slideKey;
    [SerializeField] private float _slideMultiplier = 1.5f;
    [SerializeField] private float _slideDrag = 1f;
    private bool _isSliding;

    [Header("Ground Check Settings")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundDrag = 4f;

    private void Awake()
    {
        _stateController = GetComponent<StateController>();
        _playerRigidBody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _playerRigidBody.freezeRotation = true;

        _playerHeight = _capsuleCollider.height / 2.2f;
        _startingMovementSpeed = _accelerationForce;
        _startingJumpForce = _jumpForce;
    }

    private void Update()
    {
        SetInputs();
        SetStates();
        SetPlayerDrag();

    }

    private void FixedUpdate()
    {
        SetPlayerMovement();
        HandleBufferedJump();
        LimitPlayerSpeed(); // Now using _maxMoveSpeed
    }

    private void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(_slideKey))
        {
            _isSliding = true;
        }
        else if (Input.GetKeyDown(_movementKey))
        {
            _isSliding = false;
        }

        if (Input.GetKeyDown(_jumpKey))
        {
            _jumpBufferCounter = _jumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }
    }
    private void SetStates()
    {
        var _movementDirection = GetMovementDirection();
        var isGrounded = IsGrounded();
        var currentState = _stateController.GetCurrentState();
        var isSliding = IsSliding();


        var newState = currentState switch
        {
            _ when _movementDirection == Vector3.zero && isGrounded && !isSliding => PlayerState.Idle,
            _ when _movementDirection != Vector3.zero && isGrounded && !isSliding => PlayerState.Move,
            _ when _movementDirection != Vector3.zero && isGrounded && isSliding => PlayerState.Slide,
            _ when _movementDirection == Vector3.zero && isGrounded && isSliding => PlayerState.SlideIdle,
            _ when !_canJump && !isGrounded => PlayerState.Jump,
            _ => currentState
        };
        if (newState != currentState)
        {
            _stateController.ChangeState(newState);
            OnPlayerStateChanged?.Invoke(newState);
        }

    }

    private void HandleBufferedJump()
    {
        if (_jumpBufferCounter > 0f && _canJump && IsGrounded())
        {
            _canJump = false;
            _jumpBufferCounter = 0f;
            SetPlayerJumping();
            Invoke(nameof(ResetJumping), _jumpCooldown);
        }
    }

    private void SetPlayerMovement()
    {
        _movementDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;

        float forceMultiplier = _stateController.GetCurrentState() switch
        {
            PlayerState.Move => 1f,
            PlayerState.Slide => _slideMultiplier,
            PlayerState.Jump => _airMultiplier,
            _ => 1
        };

        _playerRigidBody.AddForce(_movementDirection.normalized * _accelerationForce * forceMultiplier, ForceMode.Force);
    }

    private void SetPlayerDrag()
    {
        _playerRigidBody.linearDamping = _stateController.GetCurrentState() switch
        {
            PlayerState.Move => _groundDrag,
            PlayerState.Slide => _slideDrag,
            PlayerState.Jump => _airDrag,
            _ => _playerRigidBody.linearDamping
        };
    }

    private void LimitPlayerSpeed()
    {
        Vector3 flatVelocity = new Vector3(_playerRigidBody.linearVelocity.x, 0f, _playerRigidBody.linearVelocity.z);
        if (flatVelocity.magnitude > _maxMoveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * _maxMoveSpeed;
            _playerRigidBody.linearVelocity = new Vector3(limitedVelocity.x, _playerRigidBody.linearVelocity.y, limitedVelocity.z);
        }
    }

    private void SetPlayerJumping()
    {
        OnPlayerJumped?.Invoke();
        _playerRigidBody.linearVelocity = new Vector3(_playerRigidBody.linearVelocity.x, 0f, _playerRigidBody.linearVelocity.z);
        _playerRigidBody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    private void ResetJumping()
    {
        _canJump = true;
        OnPlayerGrounded?.Invoke();

    }
    #region Helper Functions
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _playerHeight / 2f + 0.2f, _groundLayer);
    }
    private Vector3 GetMovementDirection()
    {
        return _movementDirection.normalized;
    }

    private bool IsSliding()
    {
        return _isSliding;
    }

    public void SetMovementSpeed(float speed, float duration)
    {
        _accelerationForce += speed;
        Invoke(nameof(ResetMovementSpeed), duration);
    }
    private void ResetMovementSpeed()
    {
        _accelerationForce = _startingMovementSpeed;
    }

    public void SetJumpForce(float force, float duration)
    {
        _jumpForce += force;
        Invoke(nameof(ResetJumpForce), duration);
    }
    private void ResetJumpForce()
    {
        _jumpForce = _startingJumpForce;
    }
    #endregion
    public Rigidbody GetPlayerRIgidbody()
    {
        return _playerRigidBody;
    }
}
