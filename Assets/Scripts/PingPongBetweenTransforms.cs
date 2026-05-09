using UnityEngine;

public class PingPongBetweenTransforms : MonoBehaviour
{
    [Tooltip("First endpoint of the ping-pong motion.")]
    [SerializeField] private Transform pointA;

    [Tooltip("Second endpoint of the ping-pong motion.")]
    [SerializeField] private Transform pointB;

    [Tooltip("Seconds for one full A -> B traversal.")]
    [SerializeField] private float duration = 1f;

    [Tooltip("If true, eases in/out at the endpoints. If false, motion is linear.")]
    [SerializeField] private bool smooth = true;

    private void Update()
    {
        if (pointA == null || pointB == null || duration <= 0f) return;

        float t = Mathf.PingPong(Time.time / duration, 1f);
        if (smooth) t = Mathf.SmoothStep(0f, 1f, t);

        transform.position = Vector3.Lerp(pointA.position, pointB.position, t);
    }
}
