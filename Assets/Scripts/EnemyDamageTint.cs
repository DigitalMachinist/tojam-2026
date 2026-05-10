using UnityEngine;

public class EnemyDamageTint : MonoBehaviour
{
    [SerializeField] private EnemyHealth health;
    [SerializeField] private EnemyStart enemyStart;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (health != null) health.DamageTaken += UpdateTint;
        if (enemyStart != null) enemyStart.Restarted += ResetTint;
    }

    private void OnDisable()
    {
        if (health != null) health.DamageTaken -= UpdateTint;
        if (enemyStart != null) enemyStart.Restarted -= ResetTint;
    }

    private void ResetTint()
    {
        if (spriteRenderer == null) return;
        spriteRenderer.color = Color.white;
    }

    private void UpdateTint()
    {
        if (spriteRenderer == null || health.MaxHP <= 0) return;
        float t = (float)health.CurrentHP / health.MaxHP;
        spriteRenderer.color = Color.Lerp(Color.red, Color.white, t);
    }
}
