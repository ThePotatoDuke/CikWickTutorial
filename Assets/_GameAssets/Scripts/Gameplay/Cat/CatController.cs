using System;
using System.Threading;
using Unity.Multiplayer.Center.Common;
using UnityEngine;
using UnityEngine.AI;

public class CatController : MonoBehaviour
{
    public event Action OnCatCaught;
    private bool _hasCaughtPlayer = false;

    [Header("Referencees")]
    [SerializeField]
    private PlayerController _playerController;

    [SerializeField]
    private Transform _playerTransform;

    [Header("Settings")]
    private bool _isWaiting;

    [SerializeField]
    private float _defaultSpeeed = 5;

    [SerializeField]
    private float _chaseSpeeed = 7;

    [Header("Navigation Settings")]
    [SerializeField]
    private float _waitTime = 2f;

    [SerializeField]
    private int _maxDestinationAttempts = 10;

    [SerializeField]
    private float _chaseDistanceThreshold = 1.5f;

    [SerializeField]
    private float _chaseDistance = 2f;

    [SerializeField]
    private float _patrolRadius = 10f;
    private CatStateController _catStateController;
    private NavMeshAgent _catAgent;
    private float _timer;
    private Vector3 _initialPosition;
    private bool _isChasing;

    void Awake()
    {
        _catAgent = GetComponent<NavMeshAgent>();
        _catStateController = GetComponent<CatStateController>();
    }

    void Start()
    {
        _initialPosition = transform.position;
        SetRandomDestination();
    }

    void Update()
    {
        if (_playerController.CanCatChase())
        {
            SetChaseMovement();
        }
        else
        {
            SetPatrolMovement();
        }
    }

    private void SetChaseMovement()
    {
        _isChasing = true;

        Vector3 directionToPlayer = (_playerTransform.position - transform.position).normalized;
        Vector3 offsetPosition =
            _playerTransform.position - directionToPlayer * _chaseDistanceThreshold;
        _catAgent.SetDestination(offsetPosition); //this is confusing lmao
        _catAgent.speed = _chaseSpeeed;

        _catStateController.ChangeState(CatState.Running);
        if (
            Vector3.Distance(transform.position, _playerTransform.position) <= _chaseDistance // CAUGHT THE CHICK
            && _isChasing
            && !_hasCaughtPlayer
        )
        {
            _catStateController.ChangeState(CatState.Attacking);
            _isChasing = false;
            _hasCaughtPlayer = true; // prevent further calls

            OnCatCaught?.Invoke();
        }
    }

    private void SetPatrolMovement()
    {
        _catAgent.speed = _defaultSpeeed;
        if (!_catAgent.pathPending && _catAgent.remainingDistance <= _catAgent.stoppingDistance)
        {
            if (!_isWaiting)
            {
                _isWaiting = true;
                _timer = _waitTime;
                _catStateController.ChangeState(CatState.Idle);
            }
        }
        if (_isWaiting)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _isWaiting = false;
                // SET RANDOM DESTINATION
                SetRandomDestination();
                _catStateController.ChangeState(CatState.Walking);
            }
        }
    }

    private void SetRandomDestination()
    {
        int attempts = 0;
        bool destinationSet = false;

        while (attempts <= _maxDestinationAttempts && !destinationSet)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * _patrolRadius;
            randomDirection += _initialPosition;

            if (
                NavMesh.SamplePosition(
                    randomDirection,
                    out NavMeshHit hit,
                    _patrolRadius,
                    NavMesh.AllAreas
                )
            )
            {
                Vector3 finalPosition = hit.position;

                if (!IsPositionBlocked(finalPosition))
                {
                    _catAgent.SetDestination(finalPosition);
                    destinationSet = true;
                }
                else
                {
                    attempts++;
                }
            }
            else
            {
                attempts++;
            }
        }

        if (!destinationSet)
        {
            Debug.LogWarning("failed to find a valid destination");
            _isWaiting = true;
            _timer = _waitTime;
        }
    }

    private bool IsPositionBlocked(Vector3 position)
    {
        if (NavMesh.Raycast(transform.position, position, out NavMeshHit hit, NavMesh.AllAreas))
        {
            return true;
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Vector3 pos = (_initialPosition != Vector3.zero) ? _initialPosition : transform.position;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pos, _patrolRadius);
    }
}
