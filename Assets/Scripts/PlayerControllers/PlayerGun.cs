using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Player
{
    public Rigidbody bulletPrefab;
    public AudioClip shootSound;
    public float volumeMultiplier = 1;
    public ParticleSystem muzzleFlashPrefab;

    public float bulletSpeed = 50;
    public float bulletDamageMultiplier = 1;
    public bool automatic = false;
    public bool hasLaserSights = false;
    public GameObject laserSights;

    public float recoilRecoverySpeed = 0.1f;

    public int bulletsPerShot = 1;
    public Vector3 spread = new Vector3(0, 0, 0); // degrees
    public Vector3 bulletSpawnOffset = new Vector3(0, 0, -1);

    private List<Rigidbody> bullets = new List<Rigidbody>();
    private GameSettingsManager gsm;
    private AudioSource audioSource;
    private ParticleSystem muzzleFlash;

    private Quaternion desiredAimingRotation = Quaternion.Euler(0, 0, 0);

    protected override void Start()
    {
        base.Start();

        gsm = FindObjectOfType<GameSettingsManager>();
        volumeMultiplier = volumeMultiplier * gsm.settings.effectsVolume;
        audioSource = gameObject.AddComponent<AudioSource>();

        if (muzzleFlashPrefab != null)
        {
            muzzleFlash = Instantiate<ParticleSystem>(muzzleFlashPrefab, this.transform);
            muzzleFlash.gameObject.transform.position += bulletSpawnOffset + new Vector3(0, 0, 0.5f);
        }
    }

    protected override IEnumerator WaitAttackMS(float ms)
    {
        if (!attacking)
        {
            for (int i = 0; i < bulletsPerShot; i++)
            {
                ShootGun();
            }

            foreach (var bullet in bullets)
            {
                SelfDestructingObject selfDestruct = bullet.GetComponent<SelfDestructingObject>();
                if (selfDestruct != null)
                {
                    selfDestruct.exemptObjects.Add(this.rigbod.gameObject);
                    Physics.IgnoreCollision(bullet.GetComponent<Collider>(), this.GetComponent<Collider>());

                    foreach (var otherBullet in bullets) // TODO: Look here if there are efficiency issues with firing guns
                    {
                        Physics.IgnoreCollision(otherBullet.GetComponent<Collider>(), bullet.GetComponent<Collider>());
                        selfDestruct.exemptObjects.Add(otherBullet.gameObject);
                    }
                }
            }

            bullets.Clear();
        }
        return base.WaitAttackMS(ms);
    }

    protected virtual void ShootGun()
    {
        Vector3 worldOffset = this.transform.rotation * bulletSpawnOffset; // convert the local offset to a world coordinate system
        Quaternion spread = GetSpread();

        Rigidbody bullet = Instantiate(bulletPrefab, this.transform.position + worldOffset, spread);

        DamageMultiplyingObject damageMult = bullet.GetComponent<DamageMultiplyingObject>();
        if (damageMult != null)
        {
            damageMult.immuneToDamage.Add(this.owner);
            damageMult.damageMultiplier *= bulletDamageMultiplier;
        }

        bullet.velocity = spread * Vector3.back * bulletSpeed;

        bullets.Add(bullet);

        PlayShotSound();
        PlayMuzzleFlash();
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

    private Quaternion GetSpread()
    {
        Vector3 angles = this.transform.rotation.eulerAngles;
        float randX = Random.Range(-spread.x, spread.x);
        float randY = Random.Range(-spread.y, spread.y);
        float randZ = Random.Range(-spread.z, spread.z);

        Vector3 randomAngles = angles + new Vector3(randX, randY, randZ);

        return Quaternion.Euler(randomAngles.x, randomAngles.y, randomAngles.z);
    }

    protected override void AttackInput()
    {
        if (automatic)
        {
            if (!attacking && Input.GetButton(playerNumber + "Swing") || (owner.usesKeyboardControls && Input.GetMouseButton(0)))
            {
                shouldAttack = true;
                StartCoroutine(WaitAttackMS(attackSwingTimeMS));

                if (attackCost > 0)
                {
                    ChangeBoost(boost - attackCost);
                    StartCoroutine(WaitPostBoostMS(boostRegenDelayMS));
                }
            }
        }
        else
        {
            base.AttackInput();
        }
    }

    protected override void BlockInput()
    {
        if (Input.GetButtonDown(playerNumber + "Block") || (owner.usesKeyboardControls && Input.GetMouseButtonDown(1)))
        {
            desiredAimingRotation = rigbod.rotation;
        }

        base.BlockInput();
    }

    protected override void Block()
    {
        if (blocking)
        {
            if (hasLaserSights)
            {
                laserSights.SetActive(true);
            }

            Vector3 desired = desiredAimingRotation.eulerAngles;
            Vector3 current = rigbod.rotation.eulerAngles;
            if (desiredAimingRotation != rigbod.rotation)
            {
                rigbod.freezeRotation = true;

                //Vector3 difference = desired - current;
                //Vector3 normal = difference.normalized;
                //
                //rigbod.rotation = Quaternion.Euler(rigbod.rotation.eulerAngles + (normal * recoilRecoverySpeed));

                rigbod.rotation = Quaternion.Lerp(rigbod.rotation, desiredAimingRotation, recoilRecoverySpeed);
            }
            else
            {
                rigbod.freezeRotation = true;
            }
        }

        else
        {
            if (hasLaserSights)
            {
                laserSights.SetActive(false);
            }

            rigbod.freezeRotation = false;
        }
    }
}
