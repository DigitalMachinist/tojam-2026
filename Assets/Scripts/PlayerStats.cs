using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Tojam/Player Stats", order = 0)]
public class PlayerStats : ScriptableObject
{
    [Serializable]
    public struct ShadowDamageEntry
    {
        [Tooltip("Number of shadows this entry applies to.")]
        public int Shadows;

        [Tooltip("Damage multiplier when the player has this many shadows.")]
        public float DamageMultiplier;
    }

    [Tooltip("Default maximum HP the player starts with.")]
    [SerializeField] private int defaultMaxHP = 5;

    [Tooltip("Duration in seconds after taking damage during which the player cannot take damage again.")]
    [SerializeField] private float iframeDurationSeconds = 1f;

    [Tooltip("Maximum number of shadows the player can have at once.")]
    [SerializeField] private int maxShadows = 4;

    [Tooltip("Maximum number of weapon types the player can wield at once.")]
    [SerializeField] private int defaultMaxWeaponSlots = 1;

    [Tooltip("Damage multiplier per shadow count.")]
    [SerializeField] private ShadowDamageEntry[] shadowDamageScaling = new ShadowDamageEntry[0];

    [Tooltip("Animator controller assigned to the player's Animator component on Awake.")]
    [SerializeField] private RuntimeAnimatorController animatorController;

    public int DefaultMaxHP => defaultMaxHP;
    public float IframeDurationSeconds => iframeDurationSeconds;
    public int MaxShadows => maxShadows;
    public int DefaultMaxWeaponSlots => defaultMaxWeaponSlots;
    public ShadowDamageEntry[] ShadowDamageScaling => shadowDamageScaling;
    public RuntimeAnimatorController AnimatorController => animatorController;
}
