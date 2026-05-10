using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyBody : MonoBehaviour
{
    public event Action<PlayerWeaponEffect> WeaponContactEntered;

    private Rigidbody2D rb;
    private ConstantSpeedMover mover;

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        mover = GetComponentInChildren<ConstantSpeedMover>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var effect = other.GetComponentInParent<PlayerWeaponEffect>();
        if (effect == null) return;

        GameLog.Weapon($"Hit enemy: {effect.name} ({effect.Damage} dmg)", effect);

        if (rb != null && effect.Knockback > 0f)
        {
            Vector2 dir = ((Vector2)transform.position - (Vector2)other.transform.position).normalized;
            if (dir == Vector2.zero) dir = Vector2.right;
            rb.AddForce(dir * effect.Knockback, ForceMode2D.Impulse);
            mover?.Stun(effect.StunDuration);
        }

        WeaponContactEntered?.Invoke(effect);
    }
}
