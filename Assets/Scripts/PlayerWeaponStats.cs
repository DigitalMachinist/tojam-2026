using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWeaponStats", menuName = "Tojam/Player Weapon Stats", order = 2)]
public class PlayerWeaponStats : ScriptableObject
{
    [Tooltip("Display name of this weapon.")]
    [SerializeField] private string weaponName;

    [Tooltip("Icon representing this weapon in the HUD.")]
    [SerializeField] private Sprite sprite;

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

    [Tooltip("Impulse magnitude applied to enemies on hit.")]
    [SerializeField] private float knockback = 5f;

    [Tooltip("Seconds the enemy's movement is suppressed after being knocked back.")]
    [SerializeField] private float stunDurationSeconds = 0.15f;

    public string WeaponName => weaponName;
    public Sprite Sprite => sprite;
    public int BaseDamage => baseDamage;
    public float Cooldown => cooldown;
    public PlayerWeaponEffect EffectPrefab => effectPrefab;
    public int Quantity => quantity;
    public float SpawnDelay => spawnDelay;
    public float Knockback => knockback;
    public float StunDurationSeconds => stunDurationSeconds;
}
