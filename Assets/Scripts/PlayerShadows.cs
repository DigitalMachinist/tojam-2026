using System;
using UnityEngine;

public class PlayerShadows : MonoBehaviour
{
    [Tooltip("Health component whose damage/heal events drive shadow count.")]
    [SerializeField] private PlayerHealth health;

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

    public void Reconcile()
    {
        if (health == null) return;

        int missing = health.MaxHP - health.CurrentHP;
        int target = Mathf.Clamp(missing, 0, health.MaxHP - 1);

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
