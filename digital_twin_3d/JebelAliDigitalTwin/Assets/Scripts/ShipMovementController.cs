using UnityEngine;
using System.Collections;
using System;

public class ShipMovementController : MonoBehaviour
{
    public Transform berthPoint;
    public float speed = 2f;

    public CraneController[] cranes;   // cranes to activate
    public Action onDeparture;

    private Vector3 startPoint;

    public void StartMovement()
    {
        startPoint = transform.position;
        StartCoroutine(ShipLifecycle());
    }

    IEnumerator ShipLifecycle()
    {
        // ARRIVAL
        yield return StartCoroutine(MoveTo(berthPoint.position));

        // ACTIVATE CRANES (ship docked)
        foreach (var crane in cranes)
        {
            crane.isActive = true;
        }

        // SIMULATE LOADING / UNLOADING
        yield return new WaitForSeconds(5f);

        // DEACTIVATE CRANES (ship leaving)
        foreach (var crane in cranes)
        {
            crane.isActive = false;
        }

        // DEPARTURE
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
