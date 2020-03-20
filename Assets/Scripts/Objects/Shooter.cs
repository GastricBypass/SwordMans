using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : IEntity
{
    public Rigidbody bulletPrefab;
    public Vector3 bulletSpawnOffset;
    public float secondsBetweenShots = 0.5f;

    public AudioClip shootSound;
    public float volumeMultiplier = 1;
    public ParticleSystem muzzleFlashPrefab;
    
    public Vector3 spread = new Vector3(0, 0, 0); // degrees
    public float bulletSpeed = 50;
    public float bulletDamageMultiplier = 1;
    
    private GameSettingsManager gsm;
    private AudioSource audioSource;
    private ParticleSystem muzzleFlash;

    private bool canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        gsm = FindObjectOfType<GameSettingsManager>();
        volumeMultiplier = volumeMultiplier * gsm.settings.effectsVolume;
        audioSource = gameObject.AddComponent<AudioSource>();

        if (muzzleFlashPrefab != null)
        {
            muzzleFlash = Instantiate<ParticleSystem>(muzzleFlashPrefab, this.transform);
            muzzleFlash.gameObject.transform.position += bulletSpawnOffset + new Vector3(0, 0, 0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (canShoot && other.GetComponent<Rigidbody>() != null)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Vector3 worldOffset = this.transform.rotation * bulletSpawnOffset; // convert the local offset to a world coordinate system
        Quaternion spread = GetSpread();

        Rigidbody bullet = Instantiate(bulletPrefab, this.transform.position + worldOffset, spread);

        DamageMultiplyingObject damageMult = bullet.GetComponent<DamageMultiplyingObject>();
        if (damageMult != null)
        {
            damageMult.damageMultiplier *= bulletDamageMultiplier;
        }

        bullet.velocity = spread * Vector3.back * bulletSpeed;
        
        PlayShotSound();
        PlayMuzzleFlash();

        StartCoroutine(WaitToAllowShoot(secondsBetweenShots));
    }

    private Quaternion GetSpread()
    {
        Vector3 angles = this.transform.rotation.eulerAngles;
        float randX = Random.Range(-spread.x, spread.x);
        float randY = Random.Range(-spread.y, spread.y);
        float randZ = Random.Range(-spread.z, spread.z);

        Vector3 randomAngles = angles + new Vector3(randX, randY, randZ);

        return Quaternion.Euler(randomAngles.x, randomAngles.y, randomAngles.z);
    }

    public void PlayShotSound()
    {
        audioSource.volume = volumeMultiplier;
        audioSource.clip = shootSound;

        audioSource.Play();
    }

    public void PlayMuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
    }

    private IEnumerator WaitToAllowShoot(float seconds)
    {
        canShoot = false;
        yield return new WaitForSeconds(seconds);
        canShoot = true;
    }
}
