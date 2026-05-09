using UnityEngine;

public class PlayerUltimate : MonoBehaviour
{
    [Tooltip("Player controller whose Ultimate events trigger this ultimate.")]
    [SerializeField] private PlayerController controller;

    [Tooltip("Shadow manager whose orbit shadows will retarget on ultimate.")]
    [SerializeField] private ShadowManager shadowManager;

    [Tooltip("Transform that all orbit shadows will retarget to during the ultimate.")]
    [SerializeField] private Transform target;

    [Tooltip("Seconds the retarget lasts before reverting.")]
    [SerializeField] private float duration = 1f;

    private void OnEnable()
    {
        if (controller != null) controller.UltimatePerformed += OnUltimate;
    }

    private void OnDisable()
    {
        if (controller != null) controller.UltimatePerformed -= OnUltimate;
    }

    private void OnUltimate()
    {
        if (shadowManager == null || target == null) return;
        shadowManager.RetargetTemporarily(target, duration);
    }
}
