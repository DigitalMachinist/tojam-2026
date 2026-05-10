using UnityEngine;

public class Hazard : MonoBehaviour
{
    [Tooltip("Stats asset to pull contact damage and interval from. Overrides the fields below when set.")]
    [SerializeField] private EnemyStats stats;

    [Tooltip("Damage dealt to the player on contact and on each stay-tick.")]
    [SerializeField] private int damage = 1;

    [Tooltip("Seconds between damage ticks while the player remains in contact.")]
    [SerializeField] private float damageInterval = 1f;

    public int Damage => damage;
    public float DamageInterval => Mathf.Max(0.01f, damageInterval);

    public void Initialize(EnemyStats newStats)
    {
        stats = newStats;
        if (stats == null) return;
        damage = stats.ContactDamage;
        damageInterval = stats.ContactDamageIntervalSeconds;
    }

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void Start()
    {
        if (stats == null) return;
        damage = stats.ContactDamage;
        damageInterval = stats.ContactDamageIntervalSeconds;
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
