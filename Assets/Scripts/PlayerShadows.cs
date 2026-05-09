using System;
using UnityEngine;

public class PlayerShadows : MonoBehaviour
{
    [Tooltip("Health component whose damage/heal events drive shadow count.")]
    [SerializeField] private PlayerHealth health;

    [Tooltip("Stats asset providing MaxShadows.")]
    [SerializeField] private PlayerStats stats;

    public event Action<int> ShadowAdded;
    public event Action<int> ShadowRemoved;

    public int CurrentCount { get; private set; }

    private void OnEnable()
    {
        if (health == null) return;
        health.DamageTaken += OnHealthChanged;
        health.Healed += OnHealthChanged;
    }

    private void OnDisable()
    {
        if (health == null) return;
        health.DamageTaken -= OnHealthChanged;
        health.Healed -= OnHealthChanged;
    }

    private void OnHealthChanged(int _) => Reconcile();

    private void Reconcile()
    {
        if (health == null) return;

        int missing = health.MaxHP - health.CurrentHP;
        int cap = stats != null ? stats.MaxShadows : missing;
        int target = Mathf.Clamp(missing, 0, cap);

        if (target == CurrentCount) return;

        if (target > CurrentCount)
        {
            GameLog.Shadow($"Added: target {CurrentCount} -> {target}", this);
            ShadowAdded?.Invoke(target);
        }
        else
        {
            GameLog.Shadow($"Removed: target {CurrentCount} -> {target}", this);
            ShadowRemoved?.Invoke(target);
        }

        CurrentCount = target;
    }
}
