using UnityEngine;

public class DiagonalBounceDownLeft : MonoBehaviour
{
    public float distance = 2f; // Distance diagonale
    public float speed = 2f;    // Vitesse du mouvement

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * speed, distance);
        float newX = startPos.x - offset; // Vers la gauche
        float newY = startPos.y - offset; // Vers le bas
        transform.position = new Vector3(newX, newY, startPos.z);
    }
}
