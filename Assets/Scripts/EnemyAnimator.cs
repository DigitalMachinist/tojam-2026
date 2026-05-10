using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    [Tooltip("Current stats component providing AnimatorController and MovementSpeed.")]
    [SerializeField] private EnemyStatsCurrent stats;

    [Tooltip("Mover whose Rigidbody2D velocity drives the directional animation parameters.")]
    [SerializeField] private AnchoredMover mover;

    private Animator animator;
    private Rigidbody2D moverRb;

    private static readonly int IsMovingLeftHash  = Animator.StringToHash("IsMovingLeft");
    private static readonly int IsMovingRightHash = Animator.StringToHash("IsMovingRight");

    public bool IsMovingLeft   { get; private set; }
    public bool IsMovingRight  { get; private set; }
    public float MovementSpeed { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (mover != null) moverRb = mover.GetComponent<Rigidbody2D>();
        ApplyAnimatorController();
    }

    private void Start()
    {
        if (stats != null)
        {
            MovementSpeed = stats.MovementSpeed;
        }
    }

    private void OnEnable()
    {
        if (stats == null) return;
        stats.Initialized          += OnInitialized;
    }

    private void OnDisable()
    {
        if (stats == null) return;
        stats.Initialized          -= OnInitialized;
    }

    private void OnInitialized()
    {
        ApplyAnimatorController();
    }

    private void ApplyAnimatorController()
    {
        if (stats != null && stats.AnimatorController != null)
            animator.runtimeAnimatorController = stats.AnimatorController;
    }

    private void Update()
    {
        Vector2 velocity = moverRb != null ? moverRb.linearVelocity : Vector2.zero;

        IsMovingLeft  = velocity.x < 0f;
        IsMovingRight = velocity.x >= 0f;

        animator.SetBool(IsMovingLeftHash,  IsMovingLeft);
        animator.SetBool(IsMovingRightHash, IsMovingRight);
    }

    // AnimationEvent handlers
    public void OnFootstep() { }
}
