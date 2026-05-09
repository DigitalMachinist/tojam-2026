using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowManager : MonoBehaviour
{
    [Tooltip("Parent transform for leaf pivot prefab instances.")]
    [SerializeField] private Transform leafPivotParent;

    [Tooltip("Leaf pivot prefab to instantiate around this transform. Parented to this transform.")]
    [SerializeField] private LeafPivot leafPivotPrefab;

    [Tooltip("Parent transform for shadow prefab instances.")]
    [SerializeField] private Transform shadowParent;

    [Tooltip("Shadow prefab spawned at the same position as the anchor, parented to followerParent.")]
    [SerializeField] private Shadow shadowPrefab;

    [Tooltip("Number of instances to spawn evenly around the circle.")]
    [SerializeField] private int count = 8;

    [Tooltip("Radius of the circle in world units.")]
    [SerializeField] private float radius = 1f;

    private readonly List<Shadow> shadows = new List<Shadow>();
    private Coroutine retargetRoutine;

    private void Start()
    {
        if (leafPivotParent == null || leafPivotPrefab == null || count <= 0)
        {
            return;
        }

        Vector3 center = leafPivotParent.position;
        float step = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angleDeg = step * i;
            float angleRad = angleDeg * Mathf.Deg2Rad;
            Vector3 position = center + new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0f) * radius;

            LeafPivot leafPivot = Instantiate(leafPivotPrefab, position, Quaternion.identity, leafPivotParent);

            if (shadowPrefab != null)
            {
                Shadow shadow = Instantiate(shadowPrefab, position, Quaternion.identity, shadowParent);
                shadow.SetAnchor(leafPivot.AnchorTransform);
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
