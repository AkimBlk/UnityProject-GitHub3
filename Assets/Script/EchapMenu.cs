using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuParamètres : MonoBehaviour
{
    [Header("Références")]
    public GameObject panneauParamètres;
    public GameObject panneauNiveaux;
    public Slider curseurSensibilité;
    public Slider curseurVolume;

    [Header("Contrôleurs")]
    public ContrôleurCaméra contrôleurCaméra;
    public ContrôleurJoueur contrôleurJoueur;

    [Header("Téléportations")]
    public Transform positionLobby;
    public Transform[] positionsNiveaux;

    private bool menuActif = false;

    void Start()
    {
        // Cacher menus et initialiser valeurs
        panneauParamètres.SetActive(false);
        if (panneauNiveaux != null) panneauNiveaux.SetActive(false);

        curseurSensibilité.value = PlayerPrefs.GetFloat("Sensibilité", 200f);
        curseurVolume.value = PlayerPrefs.GetFloat("Volume", 1f);

        DéfinirSensibilité(curseurSensibilité.value);
        DéfinirVolume(curseurVolume.value);

        curseurSensibilité.onValueChanged.AddListener(DéfinirSensibilité);
        curseurVolume.onValueChanged.AddListener(DéfinirVolume);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Fermer panneau niveaux en priorité
            if (panneauNiveaux != null && panneauNiveaux.activeSelf)
            {
                panneauNiveaux.SetActive(false);
                return;
            }

            BasculerMenuParamètres();
        }
    }

    public void ResetMap()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }



    public void BasculerMenuParamètres()
    {
        menuActif = !menuActif;
        panneauParamètres.SetActive(menuActif);
        if (panneauNiveaux != null) panneauNiveaux.SetActive(false);

        // Gérer pause et contrôles
        Time.timeScale = menuActif ? 0 : 1;
        Cursor.visible = menuActif;
        Cursor.lockState = menuActif ? CursorLockMode.None : CursorLockMode.Locked;

        contrôleurCaméra.enabled = !menuActif;
        contrôleurJoueur.enabled = !menuActif;
    }

    public void BasculerPanneauNiveaux()
    {
        if (panneauNiveaux == null) return;

        bool nouveauÉtatNiveaux = !panneauNiveaux.activeSelf;
        panneauNiveaux.SetActive(nouveauÉtatNiveaux);
        panneauParamètres.SetActive(!nouveauÉtatNiveaux);
    }

    // Téléportation générique (utilisée par lobby et niveaux)
    private void TéléporterJoueur(Transform destination)
    {
        if (destination == null || contrôleurJoueur == null) return;

        // Désactiver physique temporairement
        CharacterController characterController = contrôleurJoueur.GetComponent<CharacterController>();
        if (characterController != null) characterController.enabled = false;

        // Téléporter
        contrôleurJoueur.transform.position = destination.position;
        contrôleurJoueur.transform.rotation = destination.rotation;

        // Réactiver physique
        if (characterController != null) characterController.enabled = true;

        FermerMenu();
    }

    public void TéléporterAuLobby()
    {
        TéléporterJoueur(positionLobby);
    }

    public void TéléporterAuNiveau(int index)
    {
        if (positionsNiveaux != null && index >= 0 && index < positionsNiveaux.Length)
            TéléporterJoueur(positionsNiveaux[index]);
    }

    void DéfinirSensibilité(float valeur)
    {
        contrôleurCaméra.sensibilitéSouris = valeur;
        PlayerPrefs.SetFloat("Sensibilité", valeur);
    }

    void DéfinirVolume(float valeur)
    {
        AudioListener.volume = valeur;
        PlayerPrefs.SetFloat("Volume", valeur);
    }

    public void FermerMenu()
    {
        menuActif = false;
        panneauParamètres.SetActive(false);
        if (panneauNiveaux != null) panneauNiveaux.SetActive(false);

        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        contrôleurCaméra.enabled = true;
        contrôleurJoueur.enabled = true;
    }

    public void QuitterJeu()
    {
    Application.Quit();
    }
}
