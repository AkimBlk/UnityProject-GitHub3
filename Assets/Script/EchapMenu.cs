using UnityEngine;
using UnityEngine.UI;

public class MenuParamètres : MonoBehaviour
{
    [Header("Références")]
    public GameObject panneauParamètres;
    public Slider curseurSensibilité;
    public Slider curseurVolume;
    public Button boutonExit; // Bouton Exit ajouté

    [Header("Contrôleurs")]
    public ContrôleurCaméra contrôleurCaméra;
    public ContrôleurJoueur contrôleurJoueur;

    private bool menuActif = false;

    void Start()
    {
        panneauParamètres.SetActive(false);
        ChargerParamètres();

        // Configuration des écouteurs
        curseurSensibilité.onValueChanged.AddListener(DéfinirSensibilité);
        curseurVolume.onValueChanged.AddListener(DéfinirVolume);
        boutonExit.onClick.AddListener(QuitterJeu); // Listener pour Exit
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BasculerMenuParamètres();
        }
    }

    public void BasculerMenuParamètres()
    {
        menuActif = !menuActif;
        panneauParamètres.SetActive(menuActif);

        Time.timeScale = menuActif ? 0 : 1;

        Cursor.lockState = menuActif ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = menuActif;

        contrôleurCaméra.enabled = !menuActif;
        contrôleurJoueur.enabled = !menuActif;
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

    void ChargerParamètres()
    {
        float sensibilité = PlayerPrefs.GetFloat("Sensibilité", 200f);
        float volume = PlayerPrefs.GetFloat("Volume", 1f);

        curseurSensibilité.value = sensibilité;
        curseurVolume.value = volume;

        DéfinirSensibilité(sensibilité);
        DéfinirVolume(volume);
    }

    public void FermerMenu()
    {
        BasculerMenuParamètres();
    }

    // Méthode pour quitter le jeu
    public void QuitterJeu()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}