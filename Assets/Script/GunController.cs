using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    public float fireRate = 0.1f;
    public int clipSize = 30;
    public int reserveAmmoCapacity = 270;

    [Header("Gun State")]
    public bool canShoot;
    public int currentAmmoInClip;
    public int ammoInReserve;

    [Header("FX")]
    public Image muzzleFlashImage;
    public AudioSource gunSound;

    [Header("Recoil")]
    public Transform weaponTransform; // À lier dans l’inspecteur
    public Vector3 recoilOffset = new Vector3(0, 0, -0.05f); // Recul vers l’arrière
    public float recoilSmoothness = 10f;

    private Vector3 normalPosition;

    void Start()
    {
        currentAmmoInClip = clipSize;
        ammoInReserve = reserveAmmoCapacity;
        canShoot = true;

        if (muzzleFlashImage != null)
            muzzleFlashImage.color = new Color(1, 1, 1, 0);

        // Sauvegarder la position d'origine de l'arme
        if (weaponTransform != null)
            normalPosition = weaponTransform.localPosition;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && canShoot && currentAmmoInClip > 0)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmoInClip < clipSize && ammoInReserve > 0)
        {
            Reload();
        }

        ApplyRecoil();
    }

    void Shoot()
    {
        canShoot = false;
        currentAmmoInClip--;
        Debug.Log("PEW! Ammo left: " + currentAmmoInClip);

        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // rayon du centre de l'écran

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeHit(); // appelle l’explosion et détruit la cible
            }
        }


        if (gunSound != null)
            gunSound.Play();

        if (muzzleFlashImage != null)
            StartCoroutine(ShowMuzzleFlash());

        // Recul immédiat
        if (weaponTransform != null)
            weaponTransform.localPosition = normalPosition + recoilOffset;

        Invoke("ResetShoot", fireRate);
    }

    void ResetShoot()
    {
        canShoot = true;
    }

    void Reload()
    {
        int neededAmmo = clipSize - currentAmmoInClip;

        if (neededAmmo >= ammoInReserve)
        {
            currentAmmoInClip += ammoInReserve;
            ammoInReserve = 0;
        }
        else
        {
            currentAmmoInClip = clipSize;
            ammoInReserve -= neededAmmo;
        }

        Debug.Log("Reloaded. Clip: " + currentAmmoInClip + " | Reserve: " + ammoInReserve);
    }

    System.Collections.IEnumerator ShowMuzzleFlash()
    {
        muzzleFlashImage.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.05f);
        muzzleFlashImage.color = new Color(1, 1, 1, 0);
    }

    void ApplyRecoil()
    {
        if (weaponTransform != null)
        {
            weaponTransform.localPosition = Vector3.Lerp(
                weaponTransform.localPosition,
                normalPosition,
                Time.deltaTime * recoilSmoothness
            );
        }
    }
}
