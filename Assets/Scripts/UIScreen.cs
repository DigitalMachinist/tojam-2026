using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIScreen : MonoBehaviour
{
    [SerializeField] public GameManager.ScreenType screenType;
    [SerializeField] Canvas canvas;
    [SerializeField] InputActionReference confirmInput;
    [SerializeField] InputActionReference cancelInput;
    [SerializeField] InputActionReference navigationInput;
    [SerializeField] GameManager gameManager;
    [SerializeField] List<UISelectable> selectables = new();
    private bool isInteractable = false;
    private int currentIndex = 0;

    public void Activate()
    {
        isInteractable = true;
        canvas.enabled = true;
        confirmInput.action.performed += OnConfirmInput;
        cancelInput.action.performed += OnCancelInput;
        navigationInput.action.performed += OnNavInput;

        currentIndex = 0;
        foreach ( UISelectable sel in selectables )
        {
            if ( selectables.Count > 0 )
                sel.LoseFocus();
        }
        if (selectables.Count > 0)
            selectables[ 0 ].GainFocus();
    }

    public void Deactivate()
    {
        isInteractable = false;
        canvas.enabled = false;
        currentIndex = 0;
        foreach ( UISelectable sel in selectables )
        {
            if ( selectables.Count > 0 )
                sel.LoseFocus();
        }
        confirmInput.action.performed -= OnConfirmInput;
        cancelInput.action.performed -= OnCancelInput;
        navigationInput.action.performed -= OnNavInput;
    }

    void OnConfirmInput( InputAction.CallbackContext context )
    {
        if ( !isInteractable )
            return;
        switch ( screenType )
        {
            case GameManager.ScreenType.Title:
            selectables[ currentIndex ].GetComponentInChildren<Button>().onClick.Invoke();
            break;

            case GameManager.ScreenType.Pause:
            selectables[ currentIndex ].GetComponentInChildren<Button>().onClick.Invoke();
            break;

            case GameManager.ScreenType.GameOver:
            gameManager.Reset();
            Deactivate();
            break;
        }
    }

    void OnCancelInput( InputAction.CallbackContext context )
    {
        if ( !isInteractable )
            return;
        switch ( screenType )
        {
            case GameManager.ScreenType.Title:
            break;

            case GameManager.ScreenType.Pause:
            gameManager.OnResumeSelected();
            Deactivate();
            break;

            case GameManager.ScreenType.GameOver:
            gameManager.Reset();
            Deactivate();
            break;
        }
    }

    void OnNavInput( InputAction.CallbackContext context )
    {
        if ( !isInteractable )
            return;

        if ( selectables.Count == 0 )
            return;

            int direction = Mathf.RoundToInt( context.ReadValue<Vector2>().y );
        if ( direction == 0 )
            return;

        currentIndex = ( currentIndex + selectables.Count + direction ) % selectables.Count;

        foreach (UISelectable sel in selectables)
        {
            sel.LoseFocus();
        }

        selectables[ currentIndex ].GainFocus();
    }
}