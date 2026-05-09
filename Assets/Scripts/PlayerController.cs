using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Tooltip("Reference to the Move action (Value/Vector2) from the Input Actions asset.")]
    [SerializeField] private InputActionReference moveAction;

    [Tooltip("Reference to the Attack action (Button) from the Input Actions asset.")]
    [SerializeField] private InputActionReference attackAction;

    [Tooltip("Reference to the Interact action (Button) from the Input Actions asset.")]
    [SerializeField] private InputActionReference interactAction;

    [Tooltip("Movement speed in world units per second.")]
    [SerializeField] private float speed = 5f;

    [Tooltip("Shadow manager whose orbit shadows will retarget on attack.")]
    [SerializeField] private ShadowManager shadowManager;

    [Tooltip("Transform that all orbit shadows will retarget to during an attack.")]
    [SerializeField] private Transform attackTarget;

    [Tooltip("The Shadow Anchors object's Shadow component. Its anchor will be swapped on interact.")]
    [SerializeField] private Shadow shadowAnchors;

    [Tooltip("Transform that the Shadow Anchors will retarget to during an interact.")]
    [SerializeField] private Transform interactTarget;

    [Tooltip("Seconds an attack/interact effect lasts before reverting.")]
    [SerializeField] private float actionDuration = 1f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Coroutine interactRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (moveAction != null) moveAction.action.Enable();
        if (attackAction != null)
        {
            attackAction.action.performed += OnAttackPerformed;
            attackAction.action.Enable();
        }
        if (interactAction != null)
        {
            interactAction.action.performed += OnInteractPerformed;
            interactAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
        if (attackAction != null)
        {
            attackAction.action.performed -= OnAttackPerformed;
            attackAction.action.Disable();
        }
        if (interactAction != null)
        {
            interactAction.action.performed -= OnInteractPerformed;
            interactAction.action.Disable();
        }
    }

    private void Update()
    {
        moveInput = moveAction != null ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;
    }

    private void FixedUpdate()
    {
        Vector3 velocity = new Vector3(moveInput.x, moveInput.y, 0f) * speed;
        rb.linearVelocity = velocity;
    }

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        if (shadowManager == null || attackTarget == null) return;
        shadowManager.RetargetTemporarily(attackTarget, actionDuration);
    }

    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Interact performed: attempting to retarget shadow anchors.");
        if (shadowAnchors == null || interactTarget == null || interactRoutine != null) return;
        interactRoutine = StartCoroutine(InteractRoutine());
    }

    private IEnumerator InteractRoutine()
    {
        Debug.Log("Interact performed: retargeting shadow anchors.");
        Transform previous = shadowAnchors.Anchor;
        shadowAnchors.SetAnchor(interactTarget);

        yield return new WaitForSeconds(actionDuration);

        shadowAnchors.SetAnchor(previous);
        interactRoutine = null;
    }
}
