using UnityEngine;

public class CraneController : MonoBehaviour
{
    [Header("Crane State")]
    public bool isActive = false;

    [Header("Movement Settings")]
    public float liftSpeed = 2f;
    public float liftHeight = 0.5f;

    private Vector3 basePosition;

    void Start()
    {
        basePosition = transform.position;
    }

    void Update()
    {
        if (isActive)
        {
            // Simple up-down motion to simulate loading/unloading
            float yOffset = Mathf.Sin(Time.time * liftSpeed) * liftHeight;
            transform.position = new Vector3(
                basePosition.x,
                basePosition.y + yOffset,
                basePosition.z
            );
        }
        else
        {
            // Reset crane to rest position
            transform.position = basePosition;
        }
    }
}
