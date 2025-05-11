using UnityEngine;

public class VerticalBounceDown : MonoBehaviour
{
    public float distance = 2f;     // Distance vers le bas
    public float speed = 2f;        // Vitesse du mouvement

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // PingPong génère une valeur entre 0 et "distance"
        float offset = Mathf.PingPong(Time.time * speed, distance);
        float newY = startPos.y - offset; // on soustrait pour descendre depuis startPos
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
