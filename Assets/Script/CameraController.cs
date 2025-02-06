using UnityEngine;

public class ContrôleurCaméra : MonoBehaviour
{
    [Header("Paramètres de Caméra")]
    public float sensibilitéSouris = 300f;
    public float angleVerticalMax = 90f;
    public float angleVerticalMin = -90f;

    private float rotationX = 0f;

    void Update()
    {
        GérerRotationCaméra();
    }

    void GérerRotationCaméra()
    {
        float entréeSourisX = Input.GetAxis("Mouse X") * sensibilitéSouris * Time.deltaTime;
        float entréeSourisY = Input.GetAxis("Mouse Y") * sensibilitéSouris * Time.deltaTime;

        // Rotation horizontale (parent)
        transform.parent.Rotate(Vector3.up * entréeSourisX);

        // Rotation verticale (caméra)
        rotationX -= entréeSourisY;
        rotationX = Mathf.Clamp(rotationX, angleVerticalMin, angleVerticalMax);
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }
}