using UnityEngine;

public class DiagonalBounceDownRight : MonoBehaviour
{
    public float distance = 2f; // Distance du mouvement (en diagonale)
    public float speed = 2f;    // Vitesse du mouvement

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * speed, distance);
        float newX = startPos.x + offset; // Vers la droite
        float newY = startPos.y - offset; // Vers le bas
        transform.position = new Vector3(newX, newY, startPos.z);
    }
}
