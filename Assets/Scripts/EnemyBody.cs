using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyBody : MonoBehaviour
{
    public event Action<PlayerWeaponEffect> WeaponContactEntered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var effect = other.GetComponentInParent<PlayerWeaponEffect>();
        if (effect == null) return;
        GameLog.Weapon($"Hit enemy: {effect.name} ({effect.Damage} dmg)", effect);
        WeaponContactEntered?.Invoke(effect);
    }
}
