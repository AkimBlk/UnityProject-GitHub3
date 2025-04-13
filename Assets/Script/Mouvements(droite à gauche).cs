using UnityEngine;

public class Moving2Target : MonoBehaviour
{
    public float speed = 2f;       // Vitesse du mouvement
    public float distance = 3f;    // Distance maximale à parcourir

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * speed, distance);
        Vector3 newPos = startPosition - Vector3.forward * offset; // ← Remarquez le **moins**
        transform.position = newPos;
    }
}
