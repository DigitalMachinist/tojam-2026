using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class AnchoredMover : MonoBehaviour
{
    [Tooltip("Transform this mover targets.")]
    [SerializeField] protected Transform anchor;

    public Transform Anchor => anchor;

    public virtual void SetAnchor(Transform t) => anchor = t;
}
