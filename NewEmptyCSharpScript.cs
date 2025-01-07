using UnityEngine;

using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform[] noGoZones;
    [SerializeField] private Transform finishZone;
    [SerializeField] private float noGoZoneRadius = 2f;
    [SerializeField] private float finishZoneRadius = 2f;
    [SerializeField] private GameObject winUI;

    private Vector3 moveDirection;
    private bool isNearNoGoZone = false;

    void Update()
    {
        HandleMovement();
        CheckNoGoZones();
        CheckFinishZone();
    }

    void HandleMovement()
    {
        moveDirection = new Vector3(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        ).normalized;

        // Move the player
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

void CheckNoGoZones()
{
    isNearNoGoZone = false;

    foreach (Transform noGoZone in noGoZones)
    {
        Vector3 directionToNoGo = noGoZone.position - transform.position;
        float distanceToNoGo = directionToNoGo.magnitude;

        if (distanceToNoGo < noGoZoneRadius)
        {
            isNearNoGoZone = true;

            // Shake and gradually change the No-Go Zone color
            float proximityFactor = (noGoZoneRadius - distanceToNoGo) / noGoZoneRadius;
            noGoZone.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.red, proximityFactor);

            ShakeNoGoZone(noGoZone, proximityFactor);

            // Restart if very close
            if (distanceToNoGo < noGoZoneRadius * 0.2f)
            {
                RestartScene();
                break;
            }
        }
        else
        {
            // Reset to default color if far enough
            noGoZone.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}

void ShakeNoGoZone(Transform noGoZone, float proximityFactor)
{
    // ProximityFactor (0 to 1) controls the intensity of shaking
    float shakeStrength = Mathf.Lerp(0.5f, 0.8f, proximityFactor);
    Vector3 shakeOffset = Random.insideUnitSphere * shakeStrength;

    // Apply shaking
    noGoZone.position += shakeOffset * Time.deltaTime;
}


    void CheckFinishZone()
    {
        Vector3 directionToFinish = finishZone.position - transform.position;
        float distanceToFinish = directionToFinish.magnitude;

        if (distanceToFinish < finishZoneRadius)
        {
            winUI.SetActive(true);
        }
        else
        {
            winUI.SetActive(false);
        }
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
