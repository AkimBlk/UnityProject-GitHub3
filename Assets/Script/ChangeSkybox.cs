using UnityEngine;

public class SChangeSkybox : MonoBehaviour
{
    public Material Skybox2; // skybox a changer

    private void OnTriggerEnter(Collider other)
    {
        // Verif si c'est le player qui trigger
        if (other.CompareTag("Player"))
        {
            RenderSettings.skybox = Skybox2;
        }
    }
}
