using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowManager : MonoBehaviour
{
    [Tooltip("Parent transform for anchor pivot prefab instances.")]
    [SerializeField] private Transform anchorPivotParent;

    [Tooltip("Anchor pivot prefab to instantiate around this transform. Parented to anchorPivotParent.")]
    [SerializeField] private AnchorPivot anchorPivotPrefab;

    [Tooltip("Parent transform for shadow prefab instances.")]
    [SerializeField] private Transform shadowParent;

    [Tooltip("Mover prefab spawned at the same position as the anchor, parented to shadowParent.")]
    [SerializeField] private AnchoredMover shadowPrefab;

    [Tooltip("Player shadows component whose ShadowAdded/ShadowRemoved events drive the count. Its transform is also used as the spawn position.")]
    [SerializeField] private PlayerShadows playerShadows;

    private readonly List<AnchorPivot> pivots = new List<AnchorPivot>();
    private readonly List<AnchoredMover> shadows = new List<AnchoredMover>();
    private Coroutine retargetRoutine;

    public int CurrentCount => shadows.Count;

    private void OnEnable()
    {
        if (playerShadows == null) return;
        playerShadows.ShadowAdded += OnShadowCountChanged;
        playerShadows.ShadowRemoved += OnShadowCountChanged;
    }

    private void OnDisable()
    {
        if (playerShadows == null) return;
        playerShadows.ShadowAdded -= OnShadowCountChanged;
        playerShadows.ShadowRemoved -= OnShadowCountChanged;
    }

    private void OnShadowCountChanged(int newCount) => SetTargetCount(newCount);

    public void SetTargetCount(int target)
    {
        if (anchorPivotParent == null || anchorPivotPrefab == null || shadowParent == null || shadowPrefab == null) return;

        target = Mathf.Max(0, target);
        if (target == shadows.Count) return;

        int previous = shadows.Count;
        while (shadows.Count < target) Spawn();
        while (shadows.Count > target) Despawn();
        Redistribute();
        GameLog.Shadow($"Count {previous} -> {target}", this);
    }

    public void RetargetTemporarily(Transform newTarget, float duration)
    {
        if (newTarget == null || retargetRoutine != null) return;
        GameLog.Shadow($"Retarget begin: {newTarget.name} for {duration:0.##}s ({shadows.Count} shadows)", this);
        retargetRoutine = StartCoroutine(RetargetRoutine(newTarget, duration));
    }

    private void Spawn()
    {
        Vector3 pivotCenter = anchorPivotParent.position;
        Vector3 shadowSpawn = playerShadows != null ? playerShadows.transform.position : pivotCenter;

        AnchorPivot pivot = Instantiate(anchorPivotPrefab, pivotCenter, Quaternion.identity, anchorPivotParent);
        AnchoredMover shadow = Instantiate(shadowPrefab, shadowSpawn, Quaternion.identity, shadowParent);
        shadow.SetAnchor(pivot.AnchorTransform);

        pivots.Add(pivot);
        shadows.Add(shadow);
    }

    private void Despawn()
    {
        int last = shadows.Count - 1;
        if (last < 0) return;

        if (shadows[last] != null) Destroy(shadows[last].gameObject);
        if (pivots[last] != null) Destroy(pivots[last].gameObject);
        shadows.RemoveAt(last);
        pivots.RemoveAt(last);
    }

    private void Redistribute()
    {
        int n = pivots.Count;
        if (n == 0) return;

        float spawnRadius = anchorPivotPrefab != null ? anchorPivotPrefab.MinDistance : 1f;
        float step = 360f / n;
        for (int i = 0; i < n; i++)
        {
            if (pivots[i] == null) continue;

            float rad = step * i * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
            pivots[i].SetInitialDirection(dir);
            pivots[i].transform.localPosition = dir * spawnRadius;
        }
    }

    private IEnumerator RetargetRoutine(Transform newTarget, float duration)
    {
        List<AnchoredMover> captured = new List<AnchoredMover>(shadows);
        Transform[] previousAnchors = new Transform[captured.Count];
        for (int i = 0; i < captured.Count; i++)
        {
            previousAnchors[i] = captured[i].Anchor;
            captured[i].SetAnchor(newTarget);
        }

        yield return new WaitForSeconds(duration);

        for (int i = 0; i < captured.Count; i++)
        {
            if (captured[i] != null) captured[i].SetAnchor(previousAnchors[i]);
        }

        GameLog.Shadow("Retarget end: anchors restored", this);
        retargetRoutine = null;
    }
}
