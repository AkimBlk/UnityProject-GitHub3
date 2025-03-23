using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Pour utiliser les éléments UI

public class MenuController : MonoBehaviour
{
    public GameObject canvasDefault; // Canvas principal
    public GameObject canvasWorldSpace; // Canvas des paramètres
    public Image volumeButtonImage; // Référence à l'image du bouton de volume
    public Sprite soundOnIcon; // Icône pour le son activé
    public Sprite soundOffIcon; // Icône pour le son désactivé
    private bool isMuted = false; // Indicateur si le son est coupé

    void Start()
    {
        // Assurer que le Canvas principal est actif au démarrage
        canvasDefault.SetActive(true);
        canvasWorldSpace.SetActive(false); // Cachez le Canvas des paramètres
        AudioListener.volume = 0.30f; // Assurez-vous que le volume commence à 50 %
        UpdateVolumeIcon(); // Mettre à jour l'icône du volume au démarrage
    }

    public void ToggleCanvas()
    {
        // Alternance entre le Canvas principal et le Canvas des paramètres
        if (canvasDefault.activeSelf)
        {
            canvasDefault.SetActive(false);
            canvasWorldSpace.SetActive(true);
        }
        else
        {
            canvasDefault.SetActive(true);
            canvasWorldSpace.SetActive(false);
        }
    }

    public void ChangeScene(string sceneName)
    {
        // Changer de scène
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        // Quitter l'application
        Application.Quit();
    }

    public void ToggleVolume()
    {
        if (isMuted)
        {
            // Définir le volume à 50 % si le son était coupé
            AudioListener.volume = 0.5f;
            isMuted = false; // Met à jour l'état
        }
        else
        {
            // Couper le son
            AudioListener.volume = 0f;
            isMuted = true; // Met à jour l'état
        }

        // Mise à jour de l'icône de volume
        UpdateVolumeIcon();
    }

    private void UpdateVolumeIcon()
    {
        // Changez l'image du bouton en fonction de l'état du son
        if (isMuted)
        {
            volumeButtonImage.sprite = soundOffIcon; // Icône pour le son désactivé
        }
        else
        {
            volumeButtonImage.sprite = soundOnIcon; // Icône pour le son activé
        }
    }
}
