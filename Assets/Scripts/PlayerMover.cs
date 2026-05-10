using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    [Tooltip("Player controller whose Move input drives movement.")]
    [SerializeField] private PlayerController controller;

    [Tooltip("Movement speed in world units per second.")]
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 input = controller != null ? controller.MoveInput : Vector2.zero;
        rb.linearVelocity = input * speed;
    }
}
