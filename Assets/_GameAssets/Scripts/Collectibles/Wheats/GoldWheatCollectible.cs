using UnityEngine;

public class GoldWheatCollectible : MonoBehaviour
{
    [SerializeField] private PlayerController _playercontroller;
    [SerializeField] float _movementIncreaseSpeed;
    [SerializeField] private float _resetBoostDuration;
    public void Collect()
    {
        _playercontroller.SetMovementSpeed(_movementIncreaseSpeed, _resetBoostDuration);
        Destroy(gameObject);
    }
}
