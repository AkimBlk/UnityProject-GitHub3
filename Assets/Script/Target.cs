using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject explosionEffect; // à assigner dans l'inspector

    public void TakeHit()
    {
        if (explosionEffect != null)
        {
            GameObject fx = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(fx, 2f); // Supprime l'effet après 2 secondes 
        }

        Destroy(gameObject);
    }
}
