using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    private PlayerController _playerController;
    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
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
}
