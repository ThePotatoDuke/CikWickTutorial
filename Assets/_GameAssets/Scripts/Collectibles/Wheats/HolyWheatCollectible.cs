using UnityEngine;

public class HolyWheatCollectible : MonoBehaviour
{

    [SerializeField] private PlayerController _playercontroller;
    [SerializeField] float _forceIncrease;
    [SerializeField] private float _resetBoostDuration;
    public void Collect()
    {
        _playercontroller.SetJumpForce(_forceIncrease, _resetBoostDuration);
        Destroy(gameObject);
    }
}
