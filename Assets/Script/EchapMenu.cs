using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuParamètres : MonoBehaviour
{
    // Interface
    public GameObject panneauParamètres;
    public GameObject panneauNiveaux;
    public Slider curseurSensibilité;
    public Slider curseurVolume;

    // Gameplay
    public ContrôleurCaméra contrôleurCaméra;
    public ContrôleurJoueur contrôleurJoueur;
    public Transform positionLobby;
    public Transform[] positionsNiveaux;

    private bool menuActif = false;

    void Start()
    {
        // Configuration initiale
        panneauParamètres.SetActive(false);
        if (panneauNiveaux != null) panneauNiveaux.SetActive(false);

        // Configurer les sliders
        curseurSensibilité.onValueChanged.AddListener((value) => { contrôleurCaméra.sensibilitéSouris = value; });
        curseurVolume.onValueChanged.AddListener((value) => { AudioListener.volume = value; });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panneauNiveaux.activeSelf)
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

    public void TéléporterJoueur(Transform destination)
    {
        if (destination == null || contrôleurJoueur == null) return;

        CharacterController characterController = contrôleurJoueur.GetComponent<CharacterController>();
        if (characterController != null) characterController.enabled = false;

        contrôleurJoueur.transform.position = destination.position;
        contrôleurJoueur.transform.rotation = destination.rotation;

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
