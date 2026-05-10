using UnityEngine;

public class Hazard : MonoBehaviour
{
    [Tooltip("Current stats component on this enemy.")]
    [SerializeField] private EnemyStatsCurrent stats;

    [Tooltip("Fallback damage if no stats component is assigned.")]
    [SerializeField] private int damage = 1;

    [Tooltip("Fallback damage interval if no stats component is assigned.")]
    [SerializeField] private float damageInterval = 1f;

    public int Damage => stats != null ? stats.ContactDamage : damage;
    public float DamageInterval => Mathf.Max(0.01f, stats != null ? stats.ContactDamageIntervalSeconds : damageInterval);

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (stats == null || stats.Knockback <= 0f) return;
        if (!other.CompareTag("Player")) return;
        if (!other.TryGetComponent<Rigidbody2D>(out var playerRb)) return;

        Vector2 dir = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;
        if (dir == Vector2.zero) dir = Vector2.right;

        playerRb.AddForce(dir * stats.Knockback, ForceMode2D.Impulse);
        if (rb != null) rb.AddForce(-dir * stats.Knockback, ForceMode2D.Impulse);
    }
}
