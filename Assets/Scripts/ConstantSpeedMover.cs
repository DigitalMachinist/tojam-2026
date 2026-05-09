using UnityEngine;

public class ConstantSpeedMover : AnchoredMover
{
    [Tooltip("Constant movement speed in units per second.")]
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
        if (anchor == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 anchorPos = anchor.position;
        Vector2 diff = anchorPos - rb.position;
        float distance = diff.magnitude;

        if (distance <= speed * Time.fixedDeltaTime)
        {
            rb.linearVelocity = Vector2.zero;
            rb.MovePosition(anchorPos);
            return;
        }

        rb.linearVelocity = (diff / distance) * speed;
    }

    private void Update()
    {
        if (drawDebugRayToAnchor && anchor != null)
        {
            Debug.DrawLine(transform.position, anchor.position, debugRayColor);
        }
    }
}
