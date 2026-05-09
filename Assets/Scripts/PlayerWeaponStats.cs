using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWeaponStats", menuName = "Tojam/Player Weapon Stats", order = 2)]
public class PlayerWeaponStats : ScriptableObject
{
    [Tooltip("Base damage dealt per hit, before the damage multiplier is applied.")]
    [SerializeField] private int baseDamage = 1;

    [Tooltip("Seconds between auto-attacks.")]
    [SerializeField] private float cooldown = 1f;

    [Tooltip("Effect prefab requested from the effect manager on each shot.")]
    [SerializeField] private PlayerWeaponEffect effectPrefab;

    [Tooltip("Number of effects spawned per attack burst.")]
    [SerializeField] private int quantity = 1;

    [Tooltip("Seconds between each effect spawn within a burst.")]
    [SerializeField] private float spawnDelay = 0.1f;

    public int BaseDamage => baseDamage;
    public float Cooldown => cooldown;
    public PlayerWeaponEffect EffectPrefab => effectPrefab;
    public int Quantity => quantity;
    public float SpawnDelay => spawnDelay;
}
