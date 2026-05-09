using UnityEngine;

public class LeafPivot : MonoBehaviour
{
    [Tooltip("Child transform that this pivot exposes.")]
    [SerializeField] private Transform anchorTransform;

    public Transform AnchorTransform => anchorTransform;
}
