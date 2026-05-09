using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowManager : MonoBehaviour
{
    [Tooltip("Parent transform for anchor pivot prefab instances.")]
    [SerializeField] private Transform anchorPivotParent;

    [Tooltip("Anchor pivot prefab to instantiate around this transform. Parented to this transform.")]
    [SerializeField] private AnchorPivot anchorPivotPrefab;

    [Tooltip("Parent transform for shadow prefab instances.")]
    [SerializeField] private Transform shadowParent;

    [Tooltip("Mover prefab spawned at the same position as the anchor, parented to shadowParent.")]
    [SerializeField] private AnchoredMover shadowPrefab;

    [Tooltip("Number of instances to spawn evenly around the circle.")]
    [SerializeField] private int count = 8;

    [Tooltip("Radius of the circle in world units.")]
    [SerializeField] private float radius = 1f;

    private readonly List<AnchoredMover> shadows = new List<AnchoredMover>();
    private Coroutine retargetRoutine;

    private void Start()
    {
        if (anchorPivotParent == null || anchorPivotPrefab == null || count <= 0)
        {
            return;
        }

        Vector3 center = anchorPivotParent.position;
        float step = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angleDeg = step * i;
            float angleRad = angleDeg * Mathf.Deg2Rad;
            Vector3 position = center + new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0f) * radius;

            AnchorPivot anchorPivot = Instantiate(anchorPivotPrefab, position, Quaternion.identity, anchorPivotParent);

            if (shadowPrefab != null)
            {
                AnchoredMover shadow = Instantiate(shadowPrefab, position, Quaternion.identity, shadowParent);
                shadow.SetAnchor(anchorPivot.AnchorTransform);
                shadows.Add(shadow);
            }
        }
    }

    public void RetargetTemporarily(Transform newTarget, float duration)
    {
        if (newTarget == null || retargetRoutine != null) return;
        retargetRoutine = StartCoroutine(RetargetRoutine(newTarget, duration));
    }

    private IEnumerator RetargetRoutine(Transform newTarget, float duration)
    {
        Transform[] previousAnchors = new Transform[shadows.Count];
        for (int i = 0; i < shadows.Count; i++)
        {
            previousAnchors[i] = shadows[i].Anchor;
            shadows[i].SetAnchor(newTarget);
        }

        yield return new WaitForSeconds(duration);

        for (int i = 0; i < shadows.Count; i++)
        {
            shadows[i].SetAnchor(previousAnchors[i]);
        }

        retargetRoutine = null;
    }
}
