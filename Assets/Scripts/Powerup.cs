using UnityEngine;

[CreateAssetMenu(fileName = "Powerup", menuName = "Tojam/Powerup", order = 2)]
public class Powerup : ScriptableObject
{
    [Tooltip("Display name shown to the player.")]
    [SerializeField] private string displayName;

    [Tooltip("Added to the player's maximum HP.")]
    [SerializeField] private int maxHPIncrease;

    [Tooltip("Added to the player's movement speed.")]
    [SerializeField] private float movementSpeedBonus;

    [Tooltip("Added to the player's attack rate multiplier.")]
    [SerializeField] private float attackRateBonus;

    [Tooltip("Added to the player's maximum weapon slots.")]
    [SerializeField] private int weaponSlotsIncrease;

    [Tooltip("Added to the player's iframe duration in seconds.")]
    [SerializeField] private float iframeDurationBonus;

    [Tooltip("Added to the player's knockback force.")]
    [SerializeField] private float knockback;

    [Tooltip("Added to the player's weapon damage.")]
    [SerializeField] private float weaponDamage;

    [Tooltip("Multiplied into the player's weapon scale.")]
    [SerializeField] private float weaponScale;

    [Tooltip("Added to the player's weapon attack rate.")]
    [SerializeField] private float weaponAttackRate;

    [Tooltip("Added to the player's weapon knockback force.")]
    [SerializeField] private float weaponKnockback;

    [Tooltip("Added to the player's weapon quantity.")]
    [SerializeField] private int weaponQuantity;

    [Tooltip("New weapon prefab granted to the player.")]
    [SerializeField] private PlayerWeapon newWeapon;

    public string DisplayName         => displayName;
    public int MaxHPIncrease          => maxHPIncrease;
    public float MovementSpeedBonus   => movementSpeedBonus;
    public float AttackRateBonus      => attackRateBonus;
    public int WeaponSlotsIncrease    => weaponSlotsIncrease;
    public float IframeDurationBonus  => iframeDurationBonus;
    public float Knockback            => knockback;
    public float WeaponDamage         => weaponDamage;
    public float WeaponScale          => weaponScale;
    public float WeaponAttackRate     => weaponAttackRate;
    public float WeaponKnockback      => weaponKnockback;
    public int WeaponQuantity          => weaponQuantity;
    public PlayerWeapon NewWeapon     => newWeapon;
}
