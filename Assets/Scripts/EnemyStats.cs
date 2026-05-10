using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Tojam/Enemy Stats", order = 1)]
public class EnemyStats : ScriptableObject
{
    [Tooltip("Maximum HP this enemy starts with.")]
    [SerializeField] private int maxHP = 3;

    [Tooltip("Movement speed in units per second.")]
    [SerializeField] private float movementSpeed = 2f;

    [Tooltip("Force magnitude applied to both the enemy and player in opposite directions on contact.")]
    [SerializeField] private float knockback = 5f;

    [Tooltip("Mass assigned to the enemy's Rigidbody2D at spawn.")]
    [SerializeField] private float weight = 1f;

    [Tooltip("Damage dealt to the player on contact.")]
    [SerializeField] private int contactDamage = 1;

    [Tooltip("Seconds between contact damage ticks while the player remains in contact.")]
    [SerializeField] private float contactDamageIntervalSeconds = 1f;

    [Tooltip("Experience points awarded when this enemy is killed.")]
    [SerializeField] private int xpValue = 1;

    [Tooltip("Uniform scale applied to this enemy at spawn.")]
    [SerializeField] private float scale = 1f;

    [Tooltip("Animator controller assigned to the enemy's Animator component at spawn.")]
    [SerializeField] private RuntimeAnimatorController animatorController;

    public int MaxHP => maxHP;
    public float MovementSpeed => movementSpeed;
    public float Knockback => knockback;
    public float Weight => weight;
    public int ContactDamage => contactDamage;
    public float ContactDamageIntervalSeconds => contactDamageIntervalSeconds;
    public int XPValue => xpValue;
    public float Scale => scale;
    public RuntimeAnimatorController AnimatorController => animatorController;
}
