using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRigidBody;
    private CapsuleCollider _capsuleCollider;

    private float _horizontalInput, _verticalInput;
    private Vector3 _movementDirection;

    [Header("References")]
    [SerializeField] private Transform _orientationTransform;

    [Header("Movement settings")]
    [SerializeField] private float _movementSpeed;

    [Header("Jump Settings")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _jumpCooldown = 0.5f;
    private bool _canJump = true;

    [SerializeField] private float _jumpBufferTime = 0.2f;
    private float _jumpBufferCounter = 0f;

    [Header("Ground Check Settings")]
    [SerializeField] private float _playerHeight = 1f;
    [SerializeField] private LayerMask _groundLayer;

    private void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _playerRigidBody.freezeRotation = true;

        _playerHeight = _capsuleCollider.height;

        // Auto-assign height based on collider
        _playerHeight = _capsuleCollider.height;
    }

    private void Update()
    {
        SetInputs();
    }

    private void FixedUpdate()
    {
        SetPlayerMovement();
        HandleBufferedJump();
    }

    private void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        // Jump buffering: store jump request for a short time
        if (Input.GetKeyDown(_jumpKey))
        {
            _jumpBufferCounter = _jumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
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
        _playerRigidBody.AddForce(_movementDirection.normalized * _movementSpeed, ForceMode.Force);
    }

    private void SetPlayerJumping()
    {
        // Fix: use correct property name
        _playerRigidBody.linearVelocity = new Vector3(_playerRigidBody.linearVelocity.x, 0f, _playerRigidBody.linearVelocity.z);
        _playerRigidBody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    private void ResetJumping()
    {
        _canJump = true;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _playerHeight / 2.2f, _groundLayer);
    }
}
