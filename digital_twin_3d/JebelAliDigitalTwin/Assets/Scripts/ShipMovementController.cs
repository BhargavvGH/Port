using UnityEngine;
using System.Collections;
using System;



public class ShipMovementController : MonoBehaviour
{
    public Transform berthPoint;
    public float speed = 2f;

    public GameObject containerPrefab;
    public Transform yardSlot;

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

        // ACTIVATE CRANES
        foreach (var crane in cranes)
        {
            crane.isActive = true;
        }

        // SPAWN CONTAINERS
        for (int i = 0; i < 5; i++)
        {
            SpawnContainer();
            yield return new WaitForSeconds(1f);
        }

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

    void SpawnContainer()
{
    Debug.Log("SpawnContainer called");

    if (containerPrefab == null)
    {
        Debug.LogError("containerPrefab is NULL");
        return;
    }

    if (yardSlot == null)
    {
        Debug.LogError("yardSlot is NULL");
        return;
    }

    Vector3 spawnPos = transform.position + new Vector3(0, 1f, 0);

    GameObject container = Instantiate(containerPrefab, spawnPos, Quaternion.identity);

    ContainerMover mover = container.GetComponent<ContainerMover>();

    if (mover == null)
    {
        Debug.LogError("ContainerMover component is MISSING on container prefab");
        return;
    }

    mover.StartMove(yardSlot.position);
}



}
