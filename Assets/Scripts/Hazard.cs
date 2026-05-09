using UnityEngine;

public class Hazard : MonoBehaviour
{
    [Tooltip("Damage dealt to the player on contact and on each stay-tick.")]
    [SerializeField] private int damage = 1;

    [Tooltip("Seconds between damage ticks while the player remains in contact.")]
    [SerializeField] private float damageInterval = 1f;

    public int Damage => damage;
    public float DamageInterval => Mathf.Max(0.01f, damageInterval);
}
