using System;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class EnemyStatsCurrent : MonoBehaviour
{
    [Tooltip("Stats asset used to initialize values on Awake.")]
    [SerializeField] private EnemyStats stats;

    public event Action<int>   MaxHPChanged;
    public event Action<float> MovementSpeedChanged;
    public event Action<float> KnockbackChanged;
    public event Action<float> WeightChanged;
    public event Action<int>   ContactDamageChanged;
    public event Action<float> ContactDamageIntervalChanged;
    public event Action<int>   XPValueChanged;
    public event Action<float> ScaleChanged;
    public event Action        Initialized;

    public int MaxHP                          { get; private set; }
    public float MovementSpeed                { get; private set; }
    public float Knockback                    { get; private set; }
    public float Weight                       { get; private set; }
    public int ContactDamage                  { get; private set; }
    public float ContactDamageIntervalSeconds { get; private set; }
    public int XPValue                        { get; private set; }
    public float Scale                        { get; private set; } = 1f;

    public RuntimeAnimatorController AnimatorController =>
        stats != null ? stats.AnimatorController : null;

    private void Awake()
    {
        if (stats == null) return;
        MaxHP                        = stats.MaxHP;
        MovementSpeed                = stats.MovementSpeed;
        Knockback                    = stats.Knockback;
        Weight                       = stats.Weight;
        ContactDamage                = stats.ContactDamage;
        ContactDamageIntervalSeconds = stats.ContactDamageIntervalSeconds;
        XPValue                      = stats.XPValue;
        Scale                        = stats.Scale;
    }

    public void Initialize(EnemyStats baseStats, EnemySpawns spawns, int playerLevel)
    {
        stats = baseStats;

        int levelsAboveOne = Mathf.Max(0, playerLevel - 1);

        MaxHP                        = Mathf.Max(1, Mathf.RoundToInt(baseStats.MaxHP * Mathf.Pow(spawns.MaxHP, levelsAboveOne)));
        MovementSpeed                = baseStats.MovementSpeed * (float)Math.Pow(spawns.MovementSpeed, levelsAboveOne);
        Knockback                    = baseStats.Knockback;
        Weight                       = baseStats.Weight;
        ContactDamage                = baseStats.ContactDamage;
        ContactDamageIntervalSeconds = baseStats.ContactDamageIntervalSeconds;
        XPValue                      = Mathf.Max(0, Mathf.RoundToInt(baseStats.XPValue * Mathf.Pow(spawns.XPValue, levelsAboveOne)));
        Scale                        = baseStats.Scale * Mathf.Pow(spawns.Scale, levelsAboveOne);

        MaxHPChanged?.Invoke(MaxHP);
        MovementSpeedChanged?.Invoke(MovementSpeed);
        KnockbackChanged?.Invoke(Knockback);
        WeightChanged?.Invoke(Weight);
        ContactDamageChanged?.Invoke(ContactDamage);
        ContactDamageIntervalChanged?.Invoke(ContactDamageIntervalSeconds);
        XPValueChanged?.Invoke(XPValue);
        ScaleChanged?.Invoke(Scale);
        Initialized?.Invoke();
    }
}
