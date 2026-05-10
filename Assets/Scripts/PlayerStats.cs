using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Tojam/Player Stats", order = 0)]
public class PlayerStats : ScriptableObject
{
    [Serializable]
    public struct ProgressionEntry
    {
        [Tooltip("Player level this entry unlocks at.")]
        public int Level;

        [Tooltip("XP required to reach this level.")]
        public int XPCost;

        [Tooltip("Powerup applied when this level is reached.")]
        public Powerup Powerup;
    }

    [Serializable]
    public struct ShadowDamageEntry
    {
        [Tooltip("Number of shadows this entry applies to.")]
        public int Shadows;

        [Tooltip("Damage multiplier when the player has this many shadows.")]
        public float DamageMultiplier;
    }

    [Tooltip("Default maximum HP the player starts with.")]
    [SerializeField] private int maxHP = 5;

    [Tooltip("Duration in seconds after taking damage during which the player cannot take damage again.")]
    [SerializeField] private float iframeDurationSeconds = 1f;

    [Tooltip("Maximum number of weapon types the player can wield at once.")]
    [SerializeField] private int maxWeaponSlots = 1;

    [Tooltip("Damage multiplier per shadow count.")]
    [SerializeField] private ShadowDamageEntry[] shadowDamageScaling = new ShadowDamageEntry[0];

    [Tooltip("Animator controller assigned to the player's Animator component on Awake.")]
    [SerializeField] private RuntimeAnimatorController animatorController;

    [Tooltip("Base movement speed in units per second.")]
    [SerializeField] private float movementSpeed = 1f;

    [Tooltip("Multiplier applied to weapon attack cooldowns (higher = faster attacks).")]
    [SerializeField] private float attackRate = 1f;

    [Tooltip("Weapon prefab equipped on start.")]
    [SerializeField] private PlayerWeapon startingWeapon;

    [Tooltip("XP and powerup awarded at each level threshold.")]
    [SerializeField] private ProgressionEntry[] progressionTable = new ProgressionEntry[0];

    public int MaxHP => maxHP;
    public float IframeDurationSeconds => iframeDurationSeconds;
    public int MaxWeaponSlots => maxWeaponSlots;
    public ShadowDamageEntry[] ShadowDamageScaling => shadowDamageScaling;
    public RuntimeAnimatorController AnimatorController => animatorController;
    public float MovementSpeed => movementSpeed;
    public float AttackRate => attackRate;
    public PlayerWeapon StartingWeapon => startingWeapon;
    public ProgressionEntry[] ProgressionTable => progressionTable;
}
