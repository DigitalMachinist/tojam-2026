using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Shadow : MonoBehaviour
{
    [Tooltip("Transform to move toward.")]
    [SerializeField] private Transform anchor;

    [Tooltip("Speed constant. Velocity magnitude = speed * distance.")]
    [SerializeField] private float speed = 1f;

    [Tooltip("If true, draws a debug line from this object to its anchor each frame.")]
    [SerializeField] private bool drawDebugRayToAnchor = false;

    [Tooltip("Color of the debug line drawn to the anchor.")]
    [SerializeField] private Color debugRayColor = Color.cyan;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (anchor == null) return;

        Vector3 diff = anchor.position - rb.position;
        Vector3 direction = diff.normalized;
        rb.linearVelocity = direction * diff.sqrMagnitude * speed;
        //rb.linearVelocity = direction * diff.magnitude * speed;
    }

    private void Update()
    {
        if (drawDebugRayToAnchor && anchor != null)
        {
            Debug.DrawLine(transform.position, anchor.position, debugRayColor);
        }
    }

    public Transform Anchor => anchor;

    public void SetAnchor(Transform newAnchor)
    {
        anchor = newAnchor;
    }
}
