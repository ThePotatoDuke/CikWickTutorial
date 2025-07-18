using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    private PlayerController _playerController;
    private Rigidbody _playerRigidBody;

    [SerializeField]
    private Transform _playerVisualTransform;

    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerRigidBody = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<ICollectible>(out var collectible))
        {
            collectible.Collect();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<Iboostable>(out var boostable))
        {
            boostable.Boost(_playerController);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent<IDamagable>(out var damageable))
        {
            damageable.GiveDamage(_playerRigidBody, _playerVisualTransform);
            CameraShake.Instance.ShakeCamera(1.5f, 0.5f);
        }
    }
}
