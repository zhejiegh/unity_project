using UnityEngine;
using UnityEngine.SceneManagement;

public class BurgerPickup : MonoBehaviour
{
    [Header("Animation")]
    public float rotateSpeed = 90f;      // degrees/sec
    public float hoverAmplitude = 0.25f; // meters
    public float hoverFrequency = 1.5f;  // cycles/sec

    [Header("Pickup")]
    public int points = 1;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Rotate
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f, Space.World);

        // Hover (up/down)
        float yOffset = Mathf.Sin(Time.time * hoverFrequency * Mathf.PI * 1f) * hoverAmplitude + 1;
        transform.position = new Vector3(startPos.x, startPos.y + yOffset, startPos.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        ScoreManager.Add(points);
        Destroy(gameObject);
    }
}