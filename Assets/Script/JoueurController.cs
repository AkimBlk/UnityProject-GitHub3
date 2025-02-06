using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ContrôleurJoueur : MonoBehaviour
{
    [Header("Paramètres de Mouvement")]
    public float vitesseMarche = 5f;
    public float vitesseCourse = 10f;
    public float gravité = -9.81f;
    public float hauteurSaut = 1.5f;

    [Header("Accroupissement")]
    public float hauteurAccroupi = 1f;
    public float vitesseAccroupi = 2.5f;
    public float hauteurDebout = 2f;
    public float décalageCaméraAccroupi = 0.5f;

    private CharacterController contrôleurPersonnage;
    private Vector3 vélocité;
    private float vitesseActuelle;
    private bool estAccroupi;
    private Vector3 centreOriginal;
    private Transform caméraJoueur;

    void Start()
    {
        contrôleurPersonnage = GetComponent<CharacterController>();
        caméraJoueur = GetComponentInChildren<Camera>().transform;
        vitesseActuelle = vitesseMarche;
        hauteurDebout = contrôleurPersonnage.height;
        centreOriginal = contrôleurPersonnage.center;

        VerrouillerCurseur();
    }

    void Update()
    {
        GérerDéplacement();
        GérerGravité();
        GérerSaut();
        GérerAccroupissement();
        GérerCourse();
    }

    void GérerDéplacement()
    {
        float entréeX = Input.GetAxis("Horizontal");
        float entréeZ = Input.GetAxis("Vertical");

        Vector3 déplacement = transform.TransformDirection(entréeX, 0f, entréeZ);
        contrôleurPersonnage.Move(déplacement * vitesseActuelle * Time.deltaTime);
    }

    void GérerGravité()
    {
        if (contrôleurPersonnage.isGrounded && vélocité.y < 0)
            vélocité.y = -2f;

        vélocité.y += gravité * Time.deltaTime;
        contrôleurPersonnage.Move(vélocité * Time.deltaTime);
    }

    void GérerSaut()
    {
        if (Input.GetButtonDown("Jump") && contrôleurPersonnage.isGrounded && !estAccroupi)
            vélocité.y = Mathf.Sqrt(hauteurSaut * -2f * gravité);
    }

    void GérerAccroupissement()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            CommencerAccroupissement();
        else if (Input.GetKeyUp(KeyCode.LeftControl))
            ArrêterAccroupissement();
    }

    void GérerCourse()
    {
        if (!estAccroupi && contrôleurPersonnage.isGrounded)
        {
            vitesseActuelle = Input.GetKey(KeyCode.LeftShift) ? vitesseCourse : vitesseMarche;
        }
    }

    void CommencerAccroupissement()
    {
        estAccroupi = true;
        contrôleurPersonnage.height = hauteurAccroupi;
        contrôleurPersonnage.center = new Vector3(0, hauteurAccroupi / 2, 0);
        caméraJoueur.localPosition -= new Vector3(0, décalageCaméraAccroupi, 0);
        vitesseActuelle = vitesseAccroupi;
    }

    void ArrêterAccroupissement()
    {
        estAccroupi = false;
        contrôleurPersonnage.height = hauteurDebout;
        contrôleurPersonnage.center = centreOriginal;
        caméraJoueur.localPosition += new Vector3(0, décalageCaméraAccroupi, 0);
        vitesseActuelle = vitesseMarche;
    }

    void VerrouillerCurseur()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}