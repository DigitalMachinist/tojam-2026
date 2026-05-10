using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [Tooltip("Stats asset whose AnimatorController is assigned to the Animator on Awake.")]
    [SerializeField] private PlayerStats stats;

    [Tooltip("Controller whose MoveInput drives the animation parameters.")]
    [SerializeField] private PlayerController controller;

    private Animator animator;

    private static readonly int IsMovingLeftHash  = Animator.StringToHash("isMovingLeft");
    private static readonly int IsMovingRightHash = Animator.StringToHash("isMovingRight");
    private static readonly int MovementSpeedHash = Animator.StringToHash("MovementSpeed");

    public bool IsMovingLeft   { get; private set; }
    public bool IsMovingRight  { get; private set; }
    public float MovementSpeed { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (stats != null && stats.AnimatorController != null)
            animator.runtimeAnimatorController = stats.AnimatorController;
    }

    private void Update()
    {
        Vector2 input = controller != null ? controller.MoveInput : Vector2.zero;

        IsMovingLeft  = input.x < 0f;
        IsMovingRight = input.x > 0f;
        MovementSpeed = input.magnitude;

        animator.SetBool(IsMovingLeftHash,  IsMovingLeft);
        animator.SetBool(IsMovingRightHash, IsMovingRight);
        animator.SetFloat(MovementSpeedHash, MovementSpeed);
    }

    // AnimationEvent handlers
    public void OnFootstep() { }
}
