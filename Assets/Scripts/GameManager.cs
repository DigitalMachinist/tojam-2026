using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public enum ScreenType
    {
        Title,
        Pause,
        HUD,
        GameOver
    }

    [Header("Player")]
    [Tooltip("Health component whose Died event triggers game over.")]
    [SerializeField] private PlayerHealth playerHealth;

    [Tooltip("Controller whose PausePerformed event opens the menu.")]
    [SerializeField] private PlayerController playerController;

    [Tooltip("Handles re-enabling all player components on reset.")]
    [SerializeField] private PlayerStart playerStart;

    [Tooltip("Shadow count reconciliation after player health is restored.")]
    [SerializeField] private PlayerShadows playerShadows;

    [Tooltip("Root transform repositioned on reset.")]
    [SerializeField] private Transform playerRoot;

    [Tooltip("Rigidbody teleported alongside the transform on reset.")]
    [SerializeField] private Rigidbody2D playerRigidbody;

    [Header("World")]
    [Tooltip("Enemy pool cleared on reset.")]
    [SerializeField] private EnemyManager enemyManager;

    [Tooltip("Shadow GameObjects cleared on reset.")]
    [SerializeField] private ShadowManager shadowManager;

    [Tooltip("World-space rect from which a random player spawn position is chosen on reset.")]
    [SerializeField] private Rect spawnBounds = new Rect(-8f, -8f, 16f, 16f);

    [Space( 10 )]
    [Header( "Player" )]
    [SerializeField] private Canvas UITitle;
    [SerializeField] private Canvas UIPause;
    [SerializeField] private Canvas UIHUD;
    [SerializeField] private Canvas UIGameOver;

    private void OnEnable()
    {
        if (playerHealth != null) playerHealth.Died += OnPlayerDied;
        if (playerController != null) playerController.PausePerformed += OnPauseRequested;
    }

    private void OnDisable()
    {
        if (playerHealth != null) playerHealth.Died -= OnPlayerDied;
        if (playerController != null) playerController.PausePerformed -= OnPauseRequested;
    }

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        //Show Title Screen
        Time.timeScale = 0f;
        ShowScreen( ScreenType.Title );

        enemyManager?.ResetAll();
        shadowManager?.SetTargetCount(0);

        if (playerHealth != null) playerHealth.Revive(playerHealth.MaxHP);
        playerShadows?.Reconcile();

        Vector2 spawnPos = new Vector2(
            Random.Range(spawnBounds.xMin, spawnBounds.xMax),
            Random.Range(spawnBounds.yMin, spawnBounds.yMax)
        );
        if (playerRoot != null) playerRoot.position = spawnPos;
        if (playerRigidbody != null)
        {
            playerRigidbody.position = spawnPos;
            playerRigidbody.linearVelocity = Vector2.zero;
        }

        Time.timeScale = 1f;
        playerStart?.Restart();
    }

    private void OnPlayerDied()
    {
        Time.timeScale = 0f;
        GameLog.Death("Game over", this);
        // TODO: show game over view
        ShowScreen( ScreenType.GameOver );
    }

    private void OnPauseRequested()
    {
        Time.timeScale = 0f;
        GameLog.Input("Pause requested", this);
        //Show Pause Screen
        ShowScreen( ScreenType.Pause );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnBounds.center, spawnBounds.size);
    }

    public void OnPlaySelected()
    {
        Time.timeScale = 1f;
        ShowScreen( ScreenType.HUD );
    }

    public void OnResumeSelected()
    {
        Time.timeScale = 1f;
        GameLog.Input( "Resume requested", this );
        //Show Pause Screen
        ShowScreen( ScreenType.HUD );
    }

    public void OnQuitSelected()
    {
        Application.Quit();
    }

    private void ShowScreen(ScreenType _screenType)
    {
        switch ( _screenType )
        {
            case ScreenType.Title:
            UITitle.enabled = true;
            UIHUD.enabled = false;
            UIGameOver.enabled = false;
            UIPause.enabled = false;
            break;

            case ScreenType.Pause:
            UITitle.enabled = false;
            UIHUD.enabled = false;
            UIGameOver.enabled = false;
            UIPause.enabled = true;
            break;

            case ScreenType.HUD:
            UITitle.enabled = false;
            UIHUD.enabled = true;
            UIGameOver.enabled = false;
            UIPause.enabled = false;
            break;

            case ScreenType.GameOver:
            UITitle.enabled = false;
            UIHUD.enabled = false;
            UIGameOver.enabled = true;
            UIPause.enabled = false;
            break;
        }
    }
}
