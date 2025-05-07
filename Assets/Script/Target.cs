using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject explosionEffect; // à assigner dans l'inspector
    private LevelManager levelManager; // Référence mise en cache

    private void Start()
    {
        // Trouver le LevelManager une seule fois au démarrage pour éviter 
        // de le chercher à chaque destruction de cible
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void TakeHit()
    {
        // Créer l'effet d'explosion
        if (explosionEffect != null)
        {
            GameObject fx = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(fx, 2f); // Supprime l'effet après 2 secondes 
        }

        // Notifier le LevelManager que cette cible est détruite
        if (levelManager != null)
        {
            levelManager.OnTargetDestroyed(this);
        }
        else
        {
            // Au cas où le levelManager n'a pas été trouvé au démarrage
            levelManager = FindObjectOfType<LevelManager>();
            if (levelManager != null)
            {
                levelManager.OnTargetDestroyed(this);
            }
        }

        // Détruire la cible
        Destroy(gameObject);
    }
}
