using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    public float fireRate = 0.1f;                  // Délai entre chaque tir
    public int clipSize = 30;                      // Munitions par chargeur
    public int reserveAmmoCapacity = 999;          // Munitions en réserve

    [Header("Gun State")]
    public bool canShoot;                          // Si l'arme peut tirer
    public int currentAmmoInClip;                  // Munitions restantes dans le chargeur
    public int ammoInReserve;                      // Munitions restantes en réserve
    private bool isReloading = false;              // Si une recharge est en cours

    [Header("FX")]
    public Image muzzleFlashImage;                 // Sprite d’étincelle (flash)
    public AudioSource gunSound;                   // Son du tir
    public AudioSource emptyClickSound;            // Son "click" si plus de balles
    public AudioSource reloadSound;                // Son du rechargement

    [Header("Recoil")]
    public Transform weaponTransform;              // Transform de l’arme
    public Vector3 recoilOffset = new Vector3(0, 0, -0.05f); // Mouvement vers l’arrière au tir
    public float recoilSmoothness = 10f;           // Fluidité du retour à la position normale

    [Header("Reload Animation")]
    public Vector3 reloadDownOffset = new Vector3(0, -0.7f, -0.3f); // Mouvement vers le bas à la recharge
    public float reloadDuration = 0.4f;                             // Durée de l’animation de reload

    [Header("UI")]
    public TextMeshProUGUI ammoText;               // Texte affichant les munitions

    private Vector3 normalPosition;                // Position normale de l’arme

    void Start()
    {
        // Initi
        currentAmmoInClip = clipSize;
        ammoInReserve = reserveAmmoCapacity;
        canShoot = true;

        if (muzzleFlashImage != null)
            muzzleFlashImage.color = new Color(1, 1, 1, 0); // Cacher le flash au départ

        if (weaponTransform != null)
            normalPosition = weaponTransform.localPosition;

        UpdateAmmoUI();
    }

    void Update()
    {
        // Tir
        if (Input.GetMouseButton(0))
        {
            if (canShoot && currentAmmoInClip > 0)
            {
                Shoot();
            }
            else if (currentAmmoInClip <= 0 && canShoot)
            {
                //  son "click" si plus de balles
                if (emptyClickSound != null)
                    emptyClickSound.Play();

                canShoot = false;
                Invoke("ResetShoot", fireRate);
            }
        }

        // Rechargement
        if (Input.GetKeyDown(KeyCode.R) && currentAmmoInClip < clipSize && ammoInReserve > 0)
        {
            Reload();
        }

        ApplyRecoil(); // Appliquer le recul 
    }

    void Shoot()
    {
        canShoot = false;
        currentAmmoInClip--;

        // Raycast pour vérifier si on touche une cible
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeHit(); // Appelle la méthode TakeHit() du script Target
            }
        }

        // Jouer les effets
        if (gunSound != null)
            gunSound.Play();

        if (muzzleFlashImage != null)
            StartCoroutine(ShowMuzzleFlash());

        if (weaponTransform != null)
            weaponTransform.localPosition = normalPosition + recoilOffset;

        UpdateAmmoUI();
        Invoke("ResetShoot", fireRate);
    }

    void ResetShoot()
    {
        canShoot = true;
    }

    void Reload()
    {
        if (isReloading) return;

        // Calcul des munitions à recharger
        int neededAmmo = clipSize - currentAmmoInClip;

        if (ammoInReserve >= neededAmmo)
        {
            currentAmmoInClip += neededAmmo;
            ammoInReserve -= neededAmmo;
        }
        else
        {
            currentAmmoInClip += ammoInReserve;
            ammoInReserve = 0;
        }

        // Jouer le son
        if (reloadSound != null)
            reloadSound.Play();

        // Animation simple par déplacement
        if (weaponTransform != null)
            StartCoroutine(PlayReloadAnimation());

        UpdateAmmoUI();
    }

    // Animation simple : descend puis remonte l’arme
    System.Collections.IEnumerator PlayReloadAnimation()
    {
        isReloading = true;
        float t = 0f;
        Vector3 downPos = normalPosition + reloadDownOffset;

        // Descente
        while (t < reloadDuration)
        {
            weaponTransform.localPosition = Vector3.Lerp(normalPosition, downPos, t / reloadDuration);
            t += Time.deltaTime;
            yield return null;
        }

        // Remontée
        t = 0f;
        while (t < reloadDuration)
        {
            weaponTransform.localPosition = Vector3.Lerp(downPos, normalPosition, t / reloadDuration);
            t += Time.deltaTime;
            yield return null;
        }

        weaponTransform.localPosition = normalPosition;
        isReloading = false;
    }

    // Affiche brièvement l’étincelle du tir
    System.Collections.IEnumerator ShowMuzzleFlash()
    {
        muzzleFlashImage.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.05f);
        muzzleFlashImage.color = new Color(1, 1, 1, 0);
    }

    // Lisse le mouvement de recul pour revenir à la position normale
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

    // Met à jour le texte des munitions dans l'UI
    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmoInClip + " / " + ammoInReserve;
        }
    }
}
