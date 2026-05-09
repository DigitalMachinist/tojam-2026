using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private readonly Dictionary<PlayerWeaponEffect, List<PlayerWeaponEffect>> available = new();

    public void Spawn(PlayerWeaponEffect prefab, Vector3 position, Quaternion rotation, Transform effectsParent, int damage, float knockback = 0f, float stunDuration = 0f)
    {
        var effect = Pop(prefab) ?? Instantiate(prefab, transform);
        effect.transform.SetParent(effectsParent != null ? effectsParent : transform);
        effect.Play(position, rotation, this, damage, knockback, stunDuration, prefab);
    }

    public void Return(PlayerWeaponEffect effect, PlayerWeaponEffect prefab)
    {
        effect.gameObject.SetActive(false);
        effect.transform.SetParent(transform);
        if (!available.TryGetValue(prefab, out var list))
        {
            list = new List<PlayerWeaponEffect>();
            available[prefab] = list;
        }
        list.Add(effect);
    }

    private PlayerWeaponEffect Pop(PlayerWeaponEffect prefab)
    {
        if (!available.TryGetValue(prefab, out var list) || list.Count == 0) return null;
        int last = list.Count - 1;
        var effect = list[last];
        list.RemoveAt(last);
        return effect;
    }
}
