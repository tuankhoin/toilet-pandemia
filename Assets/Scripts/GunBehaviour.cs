using TMPro;
using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    public float damage = 15f;
    public float range = 100f;
    public float impactForce = 60f;
    public float fireRate = 3f;
    public int maxMag = 100;
    public int maxClip = 20;

    public Camera fpsCam;

    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    AudioSource sfx;
    AudioSource r_sfx;

    private float nextTimeToFire = 0f;
    private float nextTimeToReload = 0f;
    private float reloadTime = 0f;
    private int bulletsRemaining;
    private int mag;
    private bool isReloading = false;

    public TextMeshProUGUI ammoText;

    void Start () {
        sfx = GameObject.FindGameObjectWithTag("ShootingSFX").GetComponent<AudioSource>();
        r_sfx = GameObject.FindGameObjectWithTag("ReloadSFX").GetComponent<AudioSource>();
        mag = maxMag;
        bulletsRemaining = maxClip;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && bulletsRemaining > 0 && !isReloading)
        {
            if (!sfx.isPlaying) sfx.Play();
            nextTimeToFire = Time.time + 1f/fireRate;
            Shoot();
            bulletsRemaining--;
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            nextTimeToReload = Time.time + 2f;
            isReloading = true;
            if (!r_sfx.isPlaying) r_sfx.Play();
        }

        if (bulletsRemaining == 0 && !isReloading) {
            nextTimeToReload = Time.time + 2f;
            isReloading = true;
            if (!r_sfx.isPlaying) r_sfx.Play();
        }

        if (Time.time >= nextTimeToReload && isReloading) {
            if (mag > (maxClip-bulletsRemaining)) {
                mag -= (maxClip-bulletsRemaining);
                bulletsRemaining = maxClip;
            }
            else {
                bulletsRemaining += mag;
                mag = 0;
            }
            
            isReloading = false;
        }

        ammoText.text = bulletsRemaining + "/" + mag;
    }

    void Shoot()
    {
        muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.takeDamage(damage);
            }

            if (hit.rigidbody != null) {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactObject = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactObject, 2f);
        }
    }
}
