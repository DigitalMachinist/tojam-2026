using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Tooltip("Parent transform that holds pooled (disabled) enemy instances.")]
    [SerializeField] private Transform spawnParent;

    [Tooltip("Player transform — enemies chase this and spawn radius is centred here.")]
    [SerializeField] private Transform player;

    [Tooltip("Camera used to determine the off-screen spawn ring. Falls back to Camera.main.")]
    [SerializeField] private Camera spawnCamera;

    [Tooltip("Spawn table and enemy scaling configuration.")]
    [SerializeField] private EnemySpawns enemySpawns;

    [Tooltip("Seconds an enemy remains visible after dying before returning to the pool.")]
    [SerializeField] private float deathLingerDuration = 2f;

    [Tooltip("World-space rect that constrains valid spawn positions.")]
    [SerializeField] private Rect worldBounds = new Rect(-10f, -10f, 20f, 20f);

    [Tooltip("How many world units outside the camera edge enemies are allowed to spawn.")]
    [SerializeField] private float spawnMargin = 2f;

    private readonly List<GameObject> active = new();
    private readonly Dictionary<GameObject, List<GameObject>> pool = new();

    private Coroutine spawnCoroutine;
    private PlayerStatsCurrent playerStats;

    private void Awake()
    {
        if (spawnCamera == null) spawnCamera = Camera.main;
        if (player != null) playerStats = player.GetComponent<PlayerStatsCurrent>();
    }

    private void OnEnable()
    {
        if (enemySpawns == null || enemySpawns.SpawnTable == null || enemySpawns.SpawnTable.Length == 0) return;
        spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }

    private void OnDisable()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    public void ResetAll()
    {
        StopAllCoroutines();
        foreach (var go in active)
            if (go != null) Destroy(go);
        active.Clear();
        spawnCoroutine = null;
        if (isActiveAndEnabled && enemySpawns != null && enemySpawns.SpawnTable != null && enemySpawns.SpawnTable.Length > 0)
            spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(enemySpawns.SpawnInterval);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (!TryPickEntry(out var entry)) return;
        if (entry.Prefab == null || entry.Stats == null) return;

        var go = GetFromPool(entry.Prefab);

        int playerLevel  = playerStats != null ? playerStats.Level : 1;

        var statsCurrent = go.GetComponentInChildren<EnemyStatsCurrent>();
        var mover        = go.GetComponentInChildren<ConstantSpeedMover>();
        var enemyStart   = go.GetComponentInChildren<EnemyStart>();

        statsCurrent?.Initialize(entry.Stats, enemySpawns, playerLevel);
        enemyStart?.Restart();

        go.transform.localScale = entry.Prefab.transform.localScale * (statsCurrent != null ? statsCurrent.Scale : 1f);

        if (mover != null)
        {
            mover.Speed = statsCurrent != null ? statsCurrent.MovementSpeed : entry.Stats.MovementSpeed;
            mover.SetAnchor(player);
        }

        Vector2 spawnPos = PickSpawnPosition();
        go.transform.position = spawnPos;
        var rb = go.GetComponentInChildren<Rigidbody2D>();
        if (rb != null) rb.position = spawnPos;

        go.SetActive(true);
        active.Add(go);
        GameLog.Enemy($"Spawned {entry.Stats.name} at {spawnPos} ({active.Count} active)", this);

        var capturedGo     = go;
        var capturedPrefab = entry.Prefab;
        var health         = go.GetComponentInChildren<EnemyHealth>();
        if (health != null)
            health.Died += () =>
            {
                playerStats?.AwardXP(health.XPValue);
                OnEnemyDied(capturedGo, capturedPrefab);
            };
    }

    private void OnEnemyDied(GameObject go, GameObject prefab)
    {
        StartCoroutine(LingerThenReturn(go, prefab));
    }

    private IEnumerator LingerThenReturn(GameObject go, GameObject prefab)
    {
        yield return new WaitForSeconds(deathLingerDuration);
        ReturnToPool(go, prefab);
    }

    private void ReturnToPool(GameObject go, GameObject prefab)
    {
        active.Remove(go);
        var rb = go.GetComponentInChildren<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;
        go.SetActive(false);
        go.transform.SetParent(spawnParent);

        if (!pool.TryGetValue(prefab, out var list))
        {
            list = new List<GameObject>();
            pool[prefab] = list;
        }
        list.Add(go);
    }

    private GameObject GetFromPool(GameObject prefab)
    {
        if (pool.TryGetValue(prefab, out var list) && list.Count > 0)
        {
            int last = list.Count - 1;
            var instance = list[last];
            list.RemoveAt(last);
            return instance;
        }

        var newInstance = Instantiate(prefab, spawnParent);
        newInstance.SetActive(false);
        return newInstance;
    }

    private bool TryPickEntry(out EnemySpawnEntry result)
    {
        var table = enemySpawns.SpawnTable;
        float total = 0f;
        foreach (var e in table) total += Mathf.Max(0f, e.Weight);

        if (total <= 0f) { result = default; return false; }

        float roll = UnityEngine.Random.Range(0f, total);
        float cumulative = 0f;
        foreach (var e in table)
        {
            cumulative += Mathf.Max(0f, e.Weight);
            if (roll < cumulative) { result = e; return true; }
        }

        result = table[table.Length - 1];
        return true;
    }

    private Vector2 PickSpawnPosition()
    {
        Vector2 camMin = spawnCamera.ViewportToWorldPoint(Vector3.zero);
        Vector2 camMax = spawnCamera.ViewportToWorldPoint(Vector3.one);

        // Valid area: worldBounds intersected with the band just outside the camera edge.
        float outerMinX = Mathf.Max(camMin.x - spawnMargin, worldBounds.xMin);
        float outerMinY = Mathf.Max(camMin.y - spawnMargin, worldBounds.yMin);
        float outerMaxX = Mathf.Min(camMax.x + spawnMargin, worldBounds.xMax);
        float outerMaxY = Mathf.Min(camMax.y + spawnMargin, worldBounds.yMax);

        // Rejection-sample: pick a random point in the outer rect, keep it only if it
        // falls outside the camera view (i.e. in the ring, not the visible interior).
        for (int i = 0; i < 30; i++)
        {
            float x = UnityEngine.Random.Range(outerMinX, outerMaxX);
            float y = UnityEngine.Random.Range(outerMinY, outerMaxY);
            if (x <= camMin.x || x >= camMax.x || y <= camMin.y || y >= camMax.y)
                return new Vector2(x, y);
        }

        // Fallback if rejection keeps hitting the camera interior (e.g. margin is very small).
        return new Vector2(outerMinX, outerMinY);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(worldBounds.center, worldBounds.size);
    }
}
