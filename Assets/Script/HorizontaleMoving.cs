using UnityEngine;

public class HorizontalBounceRight : MonoBehaviour
{
    public float distance = 2f; // Distance vers la droite
    public float speed = 2f;    // Vitesse du mouvement

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * speed, distance);
        transform.position = new Vector3(startPos.x + offset, startPos.y, startPos.z);
    }
}
