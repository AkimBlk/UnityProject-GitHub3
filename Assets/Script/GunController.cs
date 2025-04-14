using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    public float fireRate = 0.1f;
    public int clipSize = 30;
    public int reserveAmmoCapacity = 999;

    [Header("Gun State")]
    public bool canShoot;
    public int currentAmmoInClip;
    public int ammoInReserve;

    [Header("FX")]
    public Image muzzleFlashImage;
    public AudioSource gunSound;

    [Header("Recoil")]
    public Transform weaponTransform;
    public Vector3 recoilOffset = new Vector3(0, 0, -0.05f);
    public float recoilSmoothness = 10f;

    [Header("UI")]
    public TextMeshProUGUI ammoText;

    private Vector3 normalPosition;

    void Start()
    {
        currentAmmoInClip = clipSize;
        ammoInReserve = reserveAmmoCapacity;
        canShoot = true;

        if (muzzleFlashImage != null)
            muzzleFlashImage.color = new Color(1, 1, 1, 0);

        if (weaponTransform != null)
            normalPosition = weaponTransform.localPosition;

        UpdateAmmoUI();
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
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeHit();
            }
        }

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

        Debug.Log("Reloaded. Clip: " + currentAmmoInClip + " | Reserve: " + ammoInReserve);
        UpdateAmmoUI();
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

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = "" + currentAmmoInClip + " / " + ammoInReserve;
        }
    }
}
