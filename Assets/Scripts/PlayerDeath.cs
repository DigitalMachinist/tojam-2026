using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [Tooltip("Health component whose Died event triggers cleanup.")]
    [SerializeField] private PlayerHealth health;

    [Tooltip("Controller to disable on death.")]
    [SerializeField] private PlayerController playerController;

    [Tooltip("Weapons to disable on death.")]
    [SerializeField] private PlayerWeapons playerWeapons;

    [Tooltip("Mover to disable on death.")]
    [SerializeField] private PlayerMover playerMover;

    [Tooltip("Rigidbody to freeze on death.")]
    [SerializeField] private Rigidbody2D rb;

    [Tooltip("Shadow manager whose shadow movers are disabled on death.")]
    [SerializeField] private ShadowManager shadowManager;

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
        if (playerController != null) playerController.enabled = false;
        if (playerWeapons != null) playerWeapons.enabled = false;
        if (playerMover != null) playerMover.enabled = false;
        playerWeapons?.SetWeaponsEnabled(false);
        shadowManager?.SetShadowMoversEnabled(false);
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
