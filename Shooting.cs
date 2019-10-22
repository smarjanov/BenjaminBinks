using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public static Shooting instance;

    [Header ("Audio for guns")]
    [SerializeField] private AudioClip gunShot;
    [SerializeField] private AudioClip gunReload;
    [SerializeField] private AudioClip gunEmpty;
    [SerializeField] private AudioSource gunSound;
    [SerializeField] private Animator reloadAnim;

    [Header("Gun settings")]
    public float gunDamage = 100f;
    [SerializeField] private float range;
    [SerializeField] private float thrust;
    [SerializeField] private int ammoInGun;
    [SerializeField] private int clipSize;
    [SerializeField] private float rateOfFire;
    [SerializeField] private float timeToReload;
    [SerializeField] private float recoilAmountMax = 3;
    public int totalAmmo;
    public bool isReloading = false;

    [Header("Other settings")]
    public Camera playerCamera;
    public GameObject bullet;
    public Transform muzzleFlashTransform;
    public Transform bulletPosition;
    public GameObject muzzleFlash;
    public GameObject cameraRecoil;
    public Text ammoInGunT;
    public Text pressR;

    private bool gameIsPaused;
    private float nextTimeToFire = 0f;
    int toTakeAway;



    private void Awake()
    {
        instance = this;
        gunSound = GetComponent<AudioSource>();
        ammoInGun = clipSize;
    }

    private void Update()
    {
        if (ammoInGun == 0)
        {
            pressR.text = "PRESS R TO RELOAD";
        }
        else if (ammoInGun > 0)
        {
            pressR.text = "";
        }
        gameIsPaused = FindObjectOfType<GameManager>().gameIsPaused;
        if (!gameIsPaused) {

            if (isReloading)

                return;
             SetCorrectAmmo();
             Shoot();
             Reload();
        }
    }

    void Shoot()
    {
    if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
    {
        if (ammoInGun > 0)
        {
                ApplyRecoil();
                nextTimeToFire = Time.time + 1f / rateOfFire;
                ShootABullet();
                GameObject muzzle = Instantiate(muzzleFlash, muzzleFlashTransform.position, muzzleFlashTransform.rotation);
                Destroy(muzzle, 0.5f);
            }

        else if(ammoInGun == 0 && Input.GetButtonDown("Fire1"))
        {
            gunSound.PlayOneShot(gunEmpty);
        }
    }
}
    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            toTakeAway = clipSize - ammoInGun;
            if (totalAmmo >= clipSize && ammoInGun < clipSize && ammoInGun != clipSize)
            {
                StartCoroutine(WaitReload());
            }

            else if (toTakeAway > totalAmmo && totalAmmo != 0)
            {
                StartCoroutine(WaitReload());
            }

        }
    }

    IEnumerator WaitReload()
    {
        isReloading = true;
        reloadAnim.SetBool("isReloading", true);
        yield return new WaitForSeconds(timeToReload);
        isReloading = false;
        reloadAnim.SetBool("isReloading", false);

        toTakeAway = clipSize - ammoInGun;

        if (totalAmmo >= clipSize && ammoInGun != clipSize)
        {

            totalAmmo = totalAmmo - toTakeAway;
            ammoInGun = ammoInGun + toTakeAway;
            gunSound.PlayOneShot(gunReload);
            KeepTrackOfAmmo();
        }

        else if (toTakeAway > totalAmmo)
        {
            ammoInGun = ammoInGun + totalAmmo;
            totalAmmo = 0;
            gunSound.PlayOneShot(gunReload);
            KeepTrackOfAmmo();
        }

    }

    void KeepTrackOfAmmo()
    {

        if (gameObject.name == "Sniper")
        {
            InventoryScript.instance.heavyAmmo = totalAmmo;
        }
             else if (gameObject.name == "AssaultRifle")
             {
                  InventoryScript.instance.mediumAmmo = totalAmmo;
        }
                else if (gameObject.name == "Pistol")
                 {
                  InventoryScript.instance.smallAmmo = totalAmmo;
        }

    }

    void SetCorrectAmmo()
    {
        ammoInGunT.text = "" + ammoInGun;

        if (gameObject.name == "Sniper")
        {
            totalAmmo = InventoryScript.instance.heavyAmmo;
        }
        else if (gameObject.name == "AssaultRifle")
        {
            totalAmmo = InventoryScript.instance.mediumAmmo;
        }
        else if (gameObject.name == "Pistol")
        {
            totalAmmo = InventoryScript.instance.smallAmmo;
        }

    }


    void ShootABullet()
    {

        RaycastHit hit;
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.Log(hit.collider.name);
        }
        gunSound.PlayOneShot(gunShot);

    }

    void ApplyRecoil()
    {
         float recoilAmount = Random.Range(1, recoilAmountMax);
         cameraRecoil.transform.Rotate(-(recoilAmount), 0, 0);
    }

}
