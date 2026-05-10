using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    [Tooltip("Stats to reset on restart.")]
    [SerializeField] private PlayerStatsCurrent playerStats;

    [Tooltip("Health to reset on restart.")]
    [SerializeField] private PlayerHealth playerHealth;

    [Tooltip("Controller to re-enable on restart.")]
    [SerializeField] private PlayerController playerController;

    [Tooltip("Weapons to re-enable on restart.")]
    [SerializeField] private PlayerWeapons playerWeapons;

    [Tooltip("Mover to re-enable on restart.")]
    [SerializeField] private PlayerMover playerMover;

    [Tooltip("Rigidbody to unfreeze on restart.")]
    [SerializeField] private Rigidbody2D rb;

    [Tooltip("Shadow manager whose shadow movers are re-enabled on restart.")]
    [SerializeField] private ShadowManager shadowManager;

    private RigidbodyConstraints2D originalConstraints;

    private void Awake()
    {
        if (rb != null) originalConstraints = rb.constraints;
    }

    public void Restart()
    {
        if (playerStats != null) playerStats.Restart();
        if (playerWeapons != null) playerWeapons.Restart();
        if (playerHealth != null) playerHealth.Restart();
        if (playerController != null) playerController.enabled = true;
        if (playerWeapons != null) playerWeapons.enabled = true;
        if (playerMover != null) playerMover.enabled = true;
        playerWeapons?.SetWeaponsEnabled(true);
        shadowManager?.SetShadowMoversEnabled(true);
        if (rb != null) rb.constraints = originalConstraints;
    }
}
