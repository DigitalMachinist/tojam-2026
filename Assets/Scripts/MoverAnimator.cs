using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class MoverAnimator : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    private static readonly int IsMovingLeftHash  = Animator.StringToHash("IsMovingLeft");
    private static readonly int IsMovingRightHash = Animator.StringToHash("IsMovingRight");
    private static readonly int MovementSpeedHash = Animator.StringToHash("MovementSpeed");

    public bool IsMovingLeft   { get; private set; }
    public bool IsMovingRight  { get; private set; }
    public float MovementSpeed { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 velocity = rb.linearVelocity;

        IsMovingLeft  = velocity.x < 0f;
        IsMovingRight = velocity.x >= 0f;
        MovementSpeed = velocity.magnitude;

        animator.SetBool(IsMovingLeftHash,  IsMovingLeft);
        animator.SetBool(IsMovingRightHash, IsMovingRight);
        animator.SetFloat(MovementSpeedHash, MovementSpeed);
    }

    // AnimationEvent handlers
    public void OnFootstep() { }
}
