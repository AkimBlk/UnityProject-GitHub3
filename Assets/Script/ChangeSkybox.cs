using UnityEngine;

public class SChangeSkybox : MonoBehaviour
{
    public Material Skybox2;  // Le nouveau skybox à afficher

    private void OnTriggerEnter(Collider other)
    {
        // Vérifier si c'est le joueur qui entre dans le trigger
        if (other.CompareTag("Player"))
        {
            // Changer le skybox
            RenderSettings.skybox = Skybox2;
        }
    }
}
