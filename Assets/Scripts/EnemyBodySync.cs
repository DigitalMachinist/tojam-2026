using UnityEngine;

public class EnemyBodySync : MonoBehaviour
{
    [Tooltip("Rigidbody2D to pin to this transform's world position each FixedUpdate.")]
    [SerializeField] private Rigidbody2D physicsBody;

    private void FixedUpdate()
    {
        if (physicsBody != null)
            physicsBody.MovePosition(transform.position);
    }
}
