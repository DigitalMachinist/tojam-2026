using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Reference to the Move action (Value/Vector2) from the Input Actions asset.")]
    [SerializeField] private InputActionReference moveAction;

    [Tooltip("Reference to the Ultimate action (Button) from the Input Actions asset.")]
    [SerializeField] private InputActionReference ultimateAction;

    [Tooltip("Reference to the Pause action (Button) from the Input Actions asset.")]
    [SerializeField] private InputActionReference pauseAction;

    public event Action<Vector2> Move;
    public event Action UltimatePerformed;
    public event Action PausePerformed;

    public Vector2 MoveInput { get; private set; }

    private void OnEnable()
    {
        if (moveAction != null) moveAction.action.Enable();
        if (ultimateAction != null)
        {
            ultimateAction.action.performed += OnUltimateInput;
            ultimateAction.action.Enable();
        }
        if (pauseAction != null)
        {
            pauseAction.action.performed += OnPauseInput;
            pauseAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
        if (ultimateAction != null)
        {
            ultimateAction.action.performed -= OnUltimateInput;
            ultimateAction.action.Disable();
        }
        if (pauseAction != null)
        {
            pauseAction.action.performed -= OnPauseInput;
            pauseAction.action.Disable();
        }
    }

    private void Update()
    {
        MoveInput = moveAction != null ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;
        if (MoveInput.sqrMagnitude > 0f) Move?.Invoke(MoveInput);
    }

    private void OnUltimateInput(InputAction.CallbackContext ctx)
    {
        GameLog.Input("Ultimate performed", this);
        UltimatePerformed?.Invoke();
    }

    private void OnPauseInput(InputAction.CallbackContext ctx)
    {
        GameLog.Input("Pause performed", this);
        PausePerformed?.Invoke();
    }
}
