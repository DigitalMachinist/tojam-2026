using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [Tooltip("Stats asset providing DefaultMaxWeaponSlots and ShadowDamageScaling.")]
    [SerializeField] private PlayerStats stats;

    [Tooltip("Shadow manager whose spawn/despawn events drive weapon instance lifecycle on shadows.")]
    [SerializeField] private ShadowManager shadowManager;

    [Tooltip("Effect manager pushed into every spawned weapon instance.")]
    [SerializeField] private EffectManager effectManager;

    [Tooltip("Controller providing MoveInput for weapon direction.")]
    [SerializeField] private PlayerController playerController;

    [Tooltip("Ordered weapon prefabs. Up to DefaultMaxWeaponSlots non-null entries are used.")]
    [SerializeField] private PlayerWeapon[] weaponPrefabs;

    private PlayerWeapon[] validPrefabs;
    private readonly List<PlayerWeapon> allInstances = new();
    private readonly Dictionary<AnchoredMover, List<PlayerWeapon>> shadowInstances = new();
    private Vector2 lastDirection = Vector2.right;

    private void Awake()
    {
        if (stats == null || weaponPrefabs == null)
        {
            validPrefabs = new PlayerWeapon[0];
            return;
        }

        int limit = stats.DefaultMaxWeaponSlots;
        var valid = new List<PlayerWeapon>();
        foreach (var prefab in weaponPrefabs)
        {
            if (valid.Count >= limit) break;
            if (prefab != null) valid.Add(prefab);
        }
        validPrefabs = valid.ToArray();
    }

    private void OnEnable()
    {
        if (shadowManager == null) return;
        shadowManager.ShadowSpawned += OnShadowSpawned;
        shadowManager.ShadowDespawned += OnShadowDespawned;
    }

    private void OnDisable()
    {
        if (shadowManager == null) return;
        shadowManager.ShadowSpawned -= OnShadowSpawned;
        shadowManager.ShadowDespawned -= OnShadowDespawned;
    }

    private void Start()
    {
        foreach (var prefab in validPrefabs)
            allInstances.Add(SpawnWeapon(prefab, transform));
    }

    private void Update()
    {
        if (playerController != null && playerController.MoveInput.sqrMagnitude > 0.001f)
            lastDirection = playerController.MoveInput.normalized;

        float multiplier = ComputeMultiplier(shadowManager != null ? shadowManager.CurrentCount : 0);

        foreach (var w in allInstances)
        {
            if (w == null) continue;
            w.DamageMultiplier = multiplier;
            w.MoveDirection = lastDirection;
        }
    }

    private void OnShadowSpawned(AnchoredMover shadow)
    {
        var weapons = new List<PlayerWeapon>();
        foreach (var prefab in validPrefabs)
        {
            var instance = SpawnWeapon(prefab, shadow.transform);
            allInstances.Add(instance);
            weapons.Add(instance);
        }
        shadowInstances[shadow] = weapons;
    }

    private void OnShadowDespawned(AnchoredMover shadow)
    {
        if (!shadowInstances.TryGetValue(shadow, out var weapons)) return;
        foreach (var w in weapons)
        {
            allInstances.Remove(w);
            if (w != null) Destroy(w.gameObject);
        }
        shadowInstances.Remove(shadow);
    }

    private Transform effectsParent;

    private PlayerWeapon SpawnWeapon(PlayerWeapon prefab, Transform parent)
    {
        var instance = Instantiate(prefab, parent);
        instance.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        instance.SetEffectManager(effectManager);
        instance.SetEffectsParent(GetOrCreateEffectsParent(parent));
        return instance;
    }

    private Transform GetOrCreateEffectsParent(Transform parent)
    {
        if (parent == transform)
        {
            if (effectsParent == null)
            {
                effectsParent = new GameObject("WeaponEffects").transform;
                effectsParent.SetParent(transform);
                effectsParent.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }
            return effectsParent;
        }

        // Shadow — create a dedicated effects child on the shadow transform
        var existing = parent.Find("WeaponEffects");
        if (existing != null) return existing;
        var shadowEffects = new GameObject("WeaponEffects").transform;
        shadowEffects.SetParent(parent);
        shadowEffects.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        return shadowEffects;
    }

    public void SetWeaponsEnabled(bool value)
    {
        foreach (var w in allInstances)
            if (w != null) w.enabled = value;
    }

    private float ComputeMultiplier(int shadowCount)
    {
        if (stats == null) return 1f;
        float multiplier = 1f;
        int bestMatch = -1;
        foreach (var entry in stats.ShadowDamageScaling)
        {
            if (entry.Shadows <= shadowCount && entry.Shadows > bestMatch)
            {
                bestMatch = entry.Shadows;
                multiplier = entry.DamageMultiplier;
            }
        }
        return multiplier;
    }
}
