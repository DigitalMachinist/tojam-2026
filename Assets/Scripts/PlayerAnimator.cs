using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [Tooltip("Current stats component providing AnimatorController and MovementSpeed.")]
    [SerializeField] private PlayerStatsCurrent currentStats;

    [Tooltip("Controller whose MoveInput drives the directional animation parameters.")]
    [SerializeField] private PlayerController controller;

    [Tooltip("Health component whose iframe events drive the IsInvulnerable parameter.")]
    [SerializeField] private PlayerHealth health;

    private Animator animator;

    private static readonly int IsMovingLeftHash   = Animator.StringToHash("IsMovingLeft");
    private static readonly int IsMovingRightHash  = Animator.StringToHash("IsMovingRight");
    private static readonly int MovementSpeedHash  = Animator.StringToHash("MovementSpeed");
    private static readonly int IsInvulnerableHash = Animator.StringToHash("IsInvulnerable");
    private static readonly int IsDeadHash         = Animator.StringToHash("IsDead");

    public bool IsMovingLeft   { get; private set; }
    public bool IsMovingRight  { get; private set; }
    public float MovementSpeed { get; private set; }
    public bool IsInvulnerable { get; private set; }
    public bool IsDead         { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (currentStats != null && currentStats.AnimatorController != null)
            animator.runtimeAnimatorController = currentStats.AnimatorController;
    }

    private void Start()
    {
        if (currentStats != null)
        {
            MovementSpeed = currentStats.MovementSpeed;
            animator.SetFloat(MovementSpeedHash, MovementSpeed);
        }
        IsDead = false;
        animator.SetBool(IsDeadHash, false);
    }

    private void OnEnable()
    {
        if (health != null)
        {
            health.BeganIframes += OnBeganIframes;
            health.EndedIframes += OnEndedIframes;
            health.Died         += OnDied;
        }
        if (currentStats != null)
            currentStats.MovementSpeedChanged += OnMovementSpeedChanged;
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.BeganIframes -= OnBeganIframes;
            health.EndedIframes -= OnEndedIframes;
            health.Died         -= OnDied;
        }
        if (currentStats != null)
            currentStats.MovementSpeedChanged -= OnMovementSpeedChanged;
    }

    private void OnDied()
    {
        IsDead = true;
        animator.SetBool(IsDeadHash, true);
    }

    private void OnBeganIframes()
    {
        IsInvulnerable = true;
        animator.SetBool(IsInvulnerableHash, true);
    }

    private void OnEndedIframes()
    {
        IsInvulnerable = false;
        animator.SetBool(IsInvulnerableHash, false);
    }

    private void OnMovementSpeedChanged(float speed)
    {
        MovementSpeed = speed;
        animator.SetFloat(MovementSpeedHash, MovementSpeed);
    }

    private void Update()
    {
        Vector2 input = controller != null ? controller.MoveInput : Vector2.zero;

        IsMovingLeft  = input.x < 0f;
        IsMovingRight = input.x > 0f;

        animator.SetBool(IsMovingLeftHash,  IsMovingLeft);
        animator.SetBool(IsMovingRightHash, IsMovingRight);
    }

    // AnimationEvent handlers
    public void OnFootstep() { }
}
