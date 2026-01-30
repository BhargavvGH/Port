using UnityEngine;
using System.Collections;
using System;

public class ShipMovementController : MonoBehaviour
{
    public Transform berthPoint;
    public float speed = 2f;
    public Action onDeparture;

    private Vector3 startPoint;

    public void StartMovement()
    {
        startPoint = transform.position;
        StartCoroutine(ShipLifecycle());
    }

    IEnumerator ShipLifecycle()
    {
        yield return StartCoroutine(MoveTo(berthPoint.position));
        yield return new WaitForSeconds(5f);
        yield return StartCoroutine(MoveTo(startPoint));

        onDeparture?.Invoke();
        Destroy(gameObject);
    }

    IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                speed * Time.deltaTime
            );
            yield return null;
        }
    }
}
