using UnityEngine;

public class DebugRayToParent : MonoBehaviour
{
    [Tooltip("Color of the debug line drawn to the parent.")]
    [SerializeField] private Color color = Color.cyan;

    private void Update()
    {
        if (transform.parent == null) return;

        Debug.DrawLine(transform.position, transform.parent.position, color);
    }
}
