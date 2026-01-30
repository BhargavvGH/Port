using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class YardStatusController : MonoBehaviour
{
    [Header("FastAPI Endpoint")]
    public string apiUrl = "http://127.0.0.1:8000/yard/status";

    private Renderer yardRenderer;

    void Start()
    {
        yardRenderer = GetComponent<Renderer>();
        StartCoroutine(PollYardStatus());
    }

    IEnumerator PollYardStatus()
    {
        while (true)
        {
            UnityWebRequest request = UnityWebRequest.Get(apiUrl);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;

                if (json.Contains("\"alert\":true"))
                {
                    yardRenderer.material.color = Color.red;
                }
                else
                {
                    yardRenderer.material.color = Color.green;
                }
            }
            else
            {
                Debug.LogError("API Error: " + request.error);
            }

            yield return new WaitForSeconds(5f);
        }
    }
}
