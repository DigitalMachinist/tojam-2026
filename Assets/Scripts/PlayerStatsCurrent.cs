using System;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class PlayerStatsCurrent : MonoBehaviour
{
    [Tooltip("Stats asset used to initialize values on Awake.")]
    [SerializeField] private PlayerStats stats;

    public event Action<int>   MaxHPChanged;
    public event Action<float> IframeDurationChanged;
    public event Action<int>   MaxWeaponSlotsChanged;
    public event Action<float> MovementSpeedChanged;
    public event Action<float> AttackRateChanged;
    public event Action<int>   LevelChanged;
    public event Action<int>   XPChanged;

    public int MaxHP                   { get; private set; }
    public float IframeDurationSeconds { get; private set; }
    public int MaxWeaponSlots          { get; private set; }
    public float MovementSpeed         { get; private set; }
    public float AttackRate            { get; private set; }
    public int Level                   { get; private set; } = 1;
    public int XP                      { get; private set; } = 0;

    public RuntimeAnimatorController AnimatorController =>
        stats != null ? stats.AnimatorController : null;

    public PlayerStats.ShadowDamageEntry[] ShadowDamageScaling =>
        stats != null ? stats.ShadowDamageScaling : Array.Empty<PlayerStats.ShadowDamageEntry>();

    private void Awake()
    {
        if (stats == null) return;
        MaxHP = stats.MaxHP;
        IframeDurationSeconds = stats.IframeDurationSeconds;
        MaxWeaponSlots = stats.MaxWeaponSlots;
        MovementSpeed = stats.MovementSpeed;
        AttackRate = stats.AttackRate;
    }

    public void AwardXP(int amount)
    {
        if (amount <= 0 || stats == null) return;

        XP += amount;
        Debug.Log($"[XP] +{amount} XP → {XP} total");
        XPChanged?.Invoke(XP);

        foreach (var entry in stats.ProgressionTable)
        {
            if (entry.Level > Level && XP >= entry.XPCost)
            {
                Level = entry.Level;
                Debug.Log($"[XP] Level up! Now level {Level} (XP: {XP}/{entry.XPCost})");
                LevelChanged?.Invoke(Level);
                if (entry.Powerup != null)
                    ApplyPowerup(entry.Powerup);
            }
        }
    }

    public void ApplyPowerup(Powerup powerup)
    {
        if (powerup == null) return;

        if (powerup.MaxHPIncrease != 0)
        {
            MaxHP += powerup.MaxHPIncrease;
            MaxHPChanged?.Invoke(MaxHP);
        }
        if (powerup.IframeDurationBonus != 0f)
        {
            IframeDurationSeconds += powerup.IframeDurationBonus;
            IframeDurationChanged?.Invoke(IframeDurationSeconds);
        }
        if (powerup.WeaponSlotsIncrease != 0)
        {
            MaxWeaponSlots += powerup.WeaponSlotsIncrease;
            MaxWeaponSlotsChanged?.Invoke(MaxWeaponSlots);
        }
        if (powerup.MovementSpeedBonus != 0f)
        {
            MovementSpeed += powerup.MovementSpeedBonus;
            MovementSpeedChanged?.Invoke(MovementSpeed);
        }
        if (powerup.AttackRateBonus != 0f)
        {
            AttackRate += powerup.AttackRateBonus;
            AttackRateChanged?.Invoke(AttackRate);
        }
    }
}
