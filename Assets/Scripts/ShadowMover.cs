using UnityEngine;

public class ShadowMover : AnchoredMover
{
    [Tooltip("Speed constant. Velocity magnitude = speed * distance.")]
    [SerializeField] private float speed = 1f;

    [Tooltip("If true, draws a debug line from this object to its anchor each frame.")]
    [SerializeField] private bool drawDebugRayToAnchor = false;

    [Tooltip("Color of the debug line drawn to the anchor.")]
    [SerializeField] private Color debugRayColor = Color.cyan;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (anchor != null) transform.position = anchor.position;
    }

    private void FixedUpdate()
    {
        if (anchor == null) return;

        Vector2 diff = (Vector2)anchor.position - rb.position;
        Vector2 direction = diff.normalized;
        //rb.linearVelocity = direction * diff.sqrMagnitude * speed;
        rb.linearVelocity = direction * diff.magnitude * speed;
    }

    private void Update()
    {
        if (drawDebugRayToAnchor && anchor != null)
        {
            Debug.DrawLine(transform.position, anchor.position, debugRayColor);
        }
    }
}
