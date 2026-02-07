using UnityEngine;
using System.Collections;

public class ContainerMover : MonoBehaviour
{
    public Vector3 targetPosition;
    public float speed = 3f;

    public void StartMove(Vector3 target)
    {
        targetPosition = target;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );
            yield return null;
        }
    }
}
