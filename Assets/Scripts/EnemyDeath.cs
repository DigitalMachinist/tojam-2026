using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    [Tooltip("Health component whose Died event triggers cleanup.")]
    [SerializeField] private EnemyHealth health;

    [Tooltip("Hazard to disable on death.")]
    [SerializeField] private Hazard hazard;

    [Tooltip("Mover to disable on death.")]
    [SerializeField] private ConstantSpeedMover mover;

    [Tooltip("Rigidbody to freeze on death.")]
    [SerializeField] private Rigidbody2D rb;

    private void OnEnable()
    {
        if (health != null) health.Died += OnDied;
    }

    private void OnDisable()
    {
        if (health != null) health.Died -= OnDied;
    }

    private void OnDied()
    {
        if (hazard != null) hazard.enabled = false;
        if (mover != null) mover.enabled = false;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
