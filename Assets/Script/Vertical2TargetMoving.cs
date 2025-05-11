using UnityEngine;

public class VerticalBounceUp : MonoBehaviour
{
    public float height = 2f;    // Combien de haut on veut aller
    public float speed = 2f;     // Vitesse de déplacement

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Déplace la boule de façon fluide vers le haut et revient
        float offset = Mathf.PingPong(Time.time * speed, height);
        transform.position = new Vector3(startPos.x, startPos.y + offset, startPos.z);
    }
}
