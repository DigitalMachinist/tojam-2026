using System.Collections;
using UnityEngine;

public class PlayerWeaponEffect : MonoBehaviour
{
    [Tooltip("AnimationClip to play. Leave null to use coroutine-driven mode.")]
    [SerializeField] private AnimationClip attackClip;

    public int Damage { get; private set; }
    public float Knockback { get; private set; }
    public float StunDuration { get; private set; }
    public PlayerWeaponEffect SourcePrefab { get; private set; }

    private Animation _animation;

    private void Awake()
    {
        _animation = GetComponent<Animation>();
    }

    public void Play(Vector3 position, Quaternion rotation, EffectManager manager, int damage, float knockback, float stunDuration, PlayerWeaponEffect sourcePrefab)
    {
        Damage = damage;
        Knockback = knockback;
        StunDuration = stunDuration;
        SourcePrefab = sourcePrefab;
        transform.SetPositionAndRotation(position, rotation);
        gameObject.SetActive(true);
        StartCoroutine(PlayRoutine(manager));
    }

    private IEnumerator PlayRoutine(EffectManager manager)
    {
        if (_animation != null && attackClip != null)
        {
            yield return null;
            _animation.AddClip(attackClip, attackClip.name);
            _animation.clip = attackClip;
            _animation.Play(attackClip.name);
            yield return new WaitForSeconds(attackClip.length);
        }
        else
        {
            yield return PlayAttackCoroutine();
        }
        manager.Return(this, SourcePrefab);
    }

    protected virtual IEnumerator PlayAttackCoroutine()
    {
        yield break;
    }
}
