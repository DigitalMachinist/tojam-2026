using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Tooltip("Current stats component on this enemy.")]
    [SerializeField] private EnemyStatsCurrent stats;

    [Tooltip("Body component whose weapon contact events drive damage.")]
    [SerializeField] private EnemyBody body;

    public event Action Died;

    public int CurrentHP { get; private set; }
    public int MaxHP { get; private set; }
    public int XPValue { get; private set; }
    public bool IsDead => CurrentHP <= 0;

    private void Awake()
    {
        if (stats != null)
        {
            stats.MaxHPChanged   += val => MaxHP   = val;
            stats.XPValueChanged += val => XPValue = val;
            stats.Initialized    += () => { CurrentHP = MaxHP; Died = null; };
            MaxHP   = stats.MaxHP;
            XPValue = stats.XPValue;
        }
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
