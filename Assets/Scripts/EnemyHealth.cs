using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Tooltip("Stats asset to read MaxHP from.")]
    [SerializeField] private EnemyStats stats;

    [Tooltip("Body component whose weapon contact events drive damage.")]
    [SerializeField] private EnemyBody body;

    public event Action Died;

    public int CurrentHP { get; private set; }
    public int MaxHP { get; private set; }
    public bool IsDead => CurrentHP <= 0;

    private void Awake()
    {
        MaxHP = stats != null ? stats.MaxHP : 1;
        CurrentHP = MaxHP;
    }

    private void OnEnable()
    {
        if (body == null) return;
        body.WeaponContactEntered += OnWeaponContact;
    }

    private void OnDisable()
    {
        if (body == null) return;
        body.WeaponContactEntered -= OnWeaponContact;
    }

    private void OnWeaponContact(PlayerWeaponEffect effect)
    {
        TakeDamage(effect.Damage);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || IsDead) return;

        int applied = Mathf.Min(amount, CurrentHP);
        CurrentHP -= applied;

        GameLog.Damage($"Enemy took {applied} damage ({CurrentHP}/{MaxHP})", this);

        if (CurrentHP == 0)
        {
            GameLog.Death("Enemy died", this);
            Died?.Invoke();
        }
    }
}
