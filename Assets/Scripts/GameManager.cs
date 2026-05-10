using UnityEngine;
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
    [Header( "UI" )]
    [SerializeField] List<UIScreen> screens = new();

    [Header("Audio")]
    [Tooltip("AudioSource to be used during title screen.")]
    [SerializeField] private AudioSource titleAudio;
    [Tooltip("AudioSource to be used during gameplay.")]
    [SerializeField] private AudioSource gameplayAudio;

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
        
        titleAudio.PlayDelayed(1);

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

        playerStart?.Restart();
    }

    private void OnPlayerDied()
    {
        gameplayAudio.Stop();
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
        titleAudio.Stop();
        gameplayAudio.PlayDelayed(1);
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
        foreach(UIScreen screen in screens)
        {
            if(_screenType == screen.screenType)
            {
                screen.Activate();
            }
            else
            {
                screen.Deactivate();
            }
        }
    }
}