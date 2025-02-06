using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControler : MonoBehaviour
{
    public GameObject canvasDefault; // Canvas Screen Space
    public GameObject canvasWorldSpace; // Canvas World Space
    public Camera mainCamera; // La caméra principale utilisée pour le rendu

    public void ToggleCanvas()
    {
        if (canvasDefault.activeSelf)
        {
            // Basculer vers le Canvas en World Space
            canvasDefault.SetActive(false);
            canvasWorldSpace.SetActive(true);

            // Désactiver la caméra (si elle n'est pas utilisée dans World Space)
            if (mainCamera != null)
            {
                mainCamera.enabled = false;
            }
        }
        else
        {
            // Basculer vers le Canvas par défaut
            canvasDefault.SetActive(true);
            canvasWorldSpace.SetActive(false);

            // Activer la caméra pour rendre le Canvas Default
            if (mainCamera != null)
            {
                mainCamera.enabled = true;
            }
        }
    }
    public void ChangeScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
