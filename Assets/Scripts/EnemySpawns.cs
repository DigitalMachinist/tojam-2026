using System;
using UnityEngine;

[Serializable]
public struct EnemySpawnEntry
{
    [Tooltip("Relative weight of this entry in the spawn table.")]
    public float Weight;

    [Tooltip("Stats to apply to the enemy when this entry is selected.")]
    public EnemyStats Stats;

    [Tooltip("Enemy prefab to spawn for this entry.")]
    public GameObject Prefab;
}

[CreateAssetMenu(fileName = "EnemySpawns", menuName = "Tojam/EnemySpawns", order = 3)]
public class EnemySpawns : ScriptableObject
{
    [Tooltip("Number of seconds between enemy spawns.")]
    [SerializeField] private float baseSpawnInterval = 2f;

    [Tooltip("Multiplier on maximum HP applied to spawned enemies.")]
    [SerializeField] private float maxHP;

    [Tooltip("Multiplier on scale applied to spawned enemies.")]
    [SerializeField] private float scale = 1f;

    [Tooltip("Multiplier on the time between spawns.")]
    [SerializeField] private float spawnInterval = 1f;

    [Tooltip("Multiplier on XP granted to the player when an enemy from this table is killed.")]
    [SerializeField] private float xpValue;

    [Tooltip("Multiplier on movement speed applied to spawned enemies.")]
    [SerializeField] private float movementSpeed;

    [Tooltip("Weighted list of enemy types to spawn.")]
    [SerializeField] private EnemySpawnEntry[] spawnTable;

    public float BaseSpawnInterval       => baseSpawnInterval;
    public float MaxHP                   => maxHP;
    public float MovementSpeed           => movementSpeed;
    public float Scale                   => scale;
    public float SpawnInterval           => spawnInterval;
    public float XPValue                 => xpValue;
    public EnemySpawnEntry[] SpawnTable  => spawnTable;
}
