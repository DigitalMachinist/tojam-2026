using UnityEngine;

public class RotateAroundAxis : MonoBehaviour
{
    [Tooltip("Axis of rotation in local space.")]
    [SerializeField] private Vector3 axis = Vector3.up;

    [Tooltip("Rotation speed in degrees per second.")]
    [SerializeField] private float degreesPerSecond = 90f;

    [Tooltip("If true, applies a random rotation around the axis on Start.")]
    [SerializeField] private bool randomizeOnStart = false;

    private void Start()
    {
        if (randomizeOnStart)
        {
            transform.Rotate(axis, Random.Range(0f, 360f), Space.Self);
        }
    }

    private void Update()
    {
        transform.Rotate(axis, degreesPerSecond * Time.deltaTime, Space.Self);
    }
}
