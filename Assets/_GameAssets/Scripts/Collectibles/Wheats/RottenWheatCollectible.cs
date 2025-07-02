using UnityEngine;

public class RottenWheatCollectible : MonoBehaviour
{

    [SerializeField] private PlayerController _playercontroller;
    [SerializeField] float _movementDecreaseSpeed;
    [SerializeField] private float _resetBoostDuration;
    public void Collect()
    {
        _playercontroller.SetMovementSpeed(_movementDecreaseSpeed, _resetBoostDuration);
        Destroy(gameObject);
    }
}
