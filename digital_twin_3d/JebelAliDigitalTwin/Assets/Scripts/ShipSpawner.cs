using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ShipData
{
    public string ship_id;
    public string arrival_time;
    public string departure_time;
    public string berth_id;
    public int container_count;
}

[System.Serializable]
public class ShipList
{
    public List<ShipData> ships;
}

public class ShipSpawner : MonoBehaviour
{
    public string apiUrl = "http://127.0.0.1:8000/ships";
    public GameObject shipPrefab;
    public Transform berthPoint;

    public CraneController[] cranes;

    private Queue<GameObject> shipQueue = new Queue<GameObject>();
    private bool berthOccupied = false;

    void Start()
    {
        StartCoroutine(LoadShips());
    }

    IEnumerator LoadShips()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = "{\"ships\":" + request.downloadHandler.text + "}";
            ShipList shipList = JsonUtility.FromJson<ShipList>(json);

            float offsetZ = 0f;

            foreach (var ship in shipList.ships)
            {
                GameObject shipGO = Instantiate(
                    shipPrefab,
                    new Vector3(-20f, 0.5f, offsetZ),
                    Quaternion.identity
                );

                shipQueue.Enqueue(shipGO);
                offsetZ += 6f;
            }

            TryMoveNextShip();
        }
        else
        {
            Debug.LogError("Ship API Error: " + request.error);
        }
    }

    void TryMoveNextShip()
    {
        if (berthOccupied || shipQueue.Count == 0)
            return;

        GameObject ship = shipQueue.Dequeue();
        ShipMovementController movement = ship.GetComponent<ShipMovementController>();
        

        movement.cranes = cranes;
        movement.berthPoint = berthPoint;
        movement.onDeparture = OnShipDeparture;

        berthOccupied = true;
        movement.StartMovement();
    }

    void OnShipDeparture()
    {
        berthOccupied = false;
        TryMoveNextShip();
    }
}
