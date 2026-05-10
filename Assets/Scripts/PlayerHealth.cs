using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Tooltip("Stats asset that provides the default maximum HP.")]
    [SerializeField] private PlayerStats stats;

    [Tooltip("Body component whose hazard contact events drive damage.")]
    [SerializeField] private PlayerBody body;

    public event Action<int> DamageTaken;
    public event Action<int> Healed;
    public event Action Died;
    public event Action FullyHealed;
    public event Action BeganIframes;
    public event Action EndedIframes;

    public int CurrentHP { get; private set; }
    public int MaxHP { get; private set; }
    public bool IsDead => CurrentHP <= 0;
    public bool IsFull => CurrentHP >= MaxHP;
    public bool IsInvulnerable { get; private set; }

    private float iframeDuration;
    private float iframeEndTime;

    private void Awake()
    {
        MaxHP = stats != null ? stats.DefaultMaxHP : 1;
        iframeDuration = stats != null ? stats.IframeDurationSeconds : 0f;
        CurrentHP = MaxHP;
    }

    private void OnEnable()
    {
        if (body == null) return;
        body.HazardContactEntered += OnHazardContact;
        body.HazardContactTicked += OnHazardContact;
    }

    private void OnDisable()
    {
        if (body == null) return;
        body.HazardContactEntered -= OnHazardContact;
        body.HazardContactTicked -= OnHazardContact;
    }

    private void OnHazardContact(Hazard hazard)
    {
        if (hazard != null) TakeDamage(hazard.Damage);
    }

    private void Update()
    {
        if (IsInvulnerable && Time.time >= iframeEndTime)
        {
            IsInvulnerable = false;
            GameLog.Iframe("Iframes ended", this);
            EndedIframes?.Invoke();
        }
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || IsDead || IsInvulnerable) return;

        int applied = Mathf.Min(amount, CurrentHP);
        CurrentHP -= applied;

        GameLog.Damage($"Took {applied} damage ({CurrentHP}/{MaxHP})", this);
        DamageTaken?.Invoke(applied);

        if (CurrentHP == 0)
        {
            GameLog.Death("Player died", this);
            Died?.Invoke();
            return;
        }

        if (iframeDuration > 0f)
        {
            IsInvulnerable = true;
            iframeEndTime = Time.time + iframeDuration;
            GameLog.Iframe($"Iframes started ({iframeDuration:0.##}s)", this);
            BeganIframes?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || IsFull) return;

        int applied = Mathf.Min(amount, MaxHP - CurrentHP);
        CurrentHP += applied;

        GameLog.Heal($"Healed {applied} HP ({CurrentHP}/{MaxHP})", this);
        Healed?.Invoke(applied);

        if (CurrentHP == MaxHP)
        {
            GameLog.Full("Fully healed", this);
            FullyHealed?.Invoke();
        }
    }

    public void Revive(int hp)
    {
        CurrentHP = Mathf.Clamp(hp, 1, MaxHP);
        GameLog.Health($"Revived at {CurrentHP}/{MaxHP}", this);
    }
}
