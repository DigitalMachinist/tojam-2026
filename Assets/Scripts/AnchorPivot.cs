using UnityEngine;

public class AnchorPivot : MonoBehaviour
{
    [Tooltip("Child transform that this pivot exposes.")]
    [SerializeField] private Transform anchorTransform;

    [Tooltip("Minimum distance from the parent transform.")]
    [SerializeField] private float minDistance = 1f;

    [Tooltip("Maximum distance from the parent transform.")]
    [SerializeField] private float maxDistance = 5f;

    [Tooltip("Oscillations per second along the radial axis.")]
    [SerializeField] private float frequency = 1f;

    [Tooltip("Phase offset in radians. Lets sibling pivots stagger.")]
    [SerializeField] private float phase = 0f;

    [Tooltip("Orbit speed around the parent's Z axis in degrees per second.")]
    [SerializeField] private float orbitDegreesPerSecond = 90f;

    [Tooltip("If true, applies a random rotation around the local Z axis on Start.")]
    [SerializeField] private bool randomizeLocalRotationOnStart = false;

    public Transform AnchorTransform => anchorTransform;
    public float MinDistance => minDistance;

    private Vector3 initialLocalDirection;

    public void SetInitialDirection(Vector3 direction)
    {
        initialLocalDirection = direction.sqrMagnitude > 0f ? direction.normalized : Vector3.right;
    }

    private void Awake()
    {
        Vector3 local = transform.localPosition;
        initialLocalDirection = local.sqrMagnitude > 0f ? local.normalized : Vector3.right;
    }

    private void Start()
    {
        if (randomizeLocalRotationOnStart)
        {
            transform.Rotate(Vector3.forward, Random.Range(0f, 360f), Space.Self);
        }
    }

    private void Update()
    {
        float mid = (minDistance + maxDistance) * 0.5f;
        float amplitude = (maxDistance - minDistance) * 0.5f;
        float distance = mid + amplitude * Mathf.Sin(Time.time * frequency * 2f * Mathf.PI + phase);
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        Quaternion orbit = Quaternion.AngleAxis(Time.time * orbitDegreesPerSecond, Vector3.forward);
        Vector3 direction = orbit * initialLocalDirection;

        transform.localPosition = direction * distance;
    }
}
