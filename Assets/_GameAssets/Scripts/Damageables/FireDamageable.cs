using UnityEngine;

public class FireDamagable : MonoBehaviour, IDamagable
{
    [SerializeField] private float _force = 20f;
    public void GiveDamage(Rigidbody playerRigidBody, Transform playerVisualTransform)
    {
        HealthManager.Instance.Damage(1);
        playerRigidBody.AddForce(-playerVisualTransform.forward * _force, ForceMode.Impulse);
        Destroy(gameObject);
    }
}
