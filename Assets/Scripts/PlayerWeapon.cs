using System.Collections;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Tooltip("Stats asset defining this weapon's damage, cooldown, effect prefab, quantity, and spawn delay.")]
    [SerializeField] private PlayerWeaponStats stats;

    [Tooltip("Effect manager that spawns and recycles PlayerWeaponEffect instances.")]
    [SerializeField] private EffectManager effectManager;

    private Transform effectsParent;
    private PlayerStatsCurrent playerStats;

    private void Awake()
    {
        playerStats = GetComponentInParent<PlayerStatsCurrent>();
    }

    public void SetEffectManager(EffectManager manager) => effectManager = manager;
    public void SetEffectsParent(Transform parent) => effectsParent = parent;

    public float DamageMultiplier { get; set; } = 1f;
    public Vector2 MoveDirection { get; set; } = Vector2.right;

    public int CurrentDamage => stats != null ? Mathf.RoundToInt(stats.BaseDamage * DamageMultiplier) : 0;
    public float CurrentKnockback => stats != null ? stats.Knockback : 0f;
    public float CurrentStunDuration => stats != null ? stats.StunDurationSeconds : 0f;

    private void OnEnable()
    {
        transform.right = MoveDirection;
        StartCoroutine(AutoAttack());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        transform.right = MoveDirection;
    }

    private IEnumerator AutoAttack()
    {
        while (true)
        {
            yield return StartCoroutine(FireBurst());
            float attackRate = playerStats != null ? Mathf.Max(0.01f, playerStats.AttackRate) : 1f;
            yield return new WaitForSeconds((stats != null ? stats.Cooldown : 1f) / attackRate);
        }
    }

    private IEnumerator FireBurst()
    {
        if (stats == null) yield break;
        for (int i = 0; i < stats.Quantity; i++)
        {
            SpawnEffect();
            if (i < stats.Quantity - 1)
                yield return new WaitForSeconds(stats.SpawnDelay);
        }
    }

    private void SpawnEffect()
    {
        if (effectManager == null || stats == null || stats.EffectPrefab == null) return;
        GameLog.Weapon($"Fired ({CurrentDamage} dmg, {CurrentKnockback} kb)", this);
        effectManager.Spawn(stats.EffectPrefab, transform.position, transform.rotation, effectsParent, CurrentDamage, CurrentKnockback, CurrentStunDuration);
    }
}
