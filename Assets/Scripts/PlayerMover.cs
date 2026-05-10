using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    [Tooltip("Player controller whose Move input drives movement.")]
    [SerializeField] private PlayerController controller;

    [Tooltip("Current stats component providing MovementSpeed.")]
    [SerializeField] private PlayerStatsCurrent stats;

    private Rigidbody2D rb;
    private float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (stats != null) speed = stats.MovementSpeed;
    }

    private void OnEnable()
    {
        if (stats != null) stats.MovementSpeedChanged += OnMovementSpeedChanged;
    }

    private void OnDisable()
    {
        if (stats != null) stats.MovementSpeedChanged -= OnMovementSpeedChanged;
    }

    private void OnMovementSpeedChanged(float newSpeed) => speed = newSpeed;

    private void FixedUpdate()
    {
        Vector2 input = controller != null ? controller.MoveInput : Vector2.zero;
        rb.linearVelocity = input * speed;
    }
}
