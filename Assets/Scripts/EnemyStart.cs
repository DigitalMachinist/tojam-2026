using System;
using UnityEngine;

public class EnemyStart : MonoBehaviour
{
    public event Action Restarted;
    [Tooltip("Hazard to re-enable on restart.")]
    [SerializeField] private Hazard hazard;

    [Tooltip("Mover to re-enable on restart.")]
    [SerializeField] private ConstantSpeedMover mover;

    [Tooltip("Body to re-enable on restart so weapon contacts register again.")]
    [SerializeField] private EnemyBody body;

    [Tooltip("Rigidbody to unfreeze on restart.")]
    [SerializeField] private Rigidbody2D rb;

    private RigidbodyConstraints2D originalConstraints;

    private void Awake()
    {
        if (rb != null) originalConstraints = rb.constraints;
    }

    public void Restart()
    {
        if (hazard != null) hazard.enabled = true;
        if (mover != null) mover.enabled = true;
        if (body != null) body.enabled = true;
        if (rb != null) rb.constraints = originalConstraints;
        Restarted?.Invoke();
    }
}
