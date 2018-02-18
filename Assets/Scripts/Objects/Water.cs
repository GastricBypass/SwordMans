using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

    public float viscosity = 5; // 0 = air
    public float upwardVelocityPushFactor = 0.25f; // how much it pushes while moving up compared to moving down.
    public float normalForceFactor = 1.8f; // How much gravity is multiplied by before being subtracted from the force.

    public float speedMultiplier = 0.01f;

    public ParticleSystem waterEffectPrefab;
    private AudioSource waterAudio;
    private ParticleSystem waterEffect;
    private float volumeMultiplier;

    private GameSettingsManager gsm;

    //public Vector3 verticalDirection = new Vector3(0, 1, 0); // Maybe it'll always be up. Consider allowing this later.

    void Start()
    {
        gsm = FindObjectOfType<GameSettingsManager>();

        waterAudio = new AudioSource();
        volumeMultiplier = gsm.settings.effectsVolume;
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody colliderBod = other.GetComponent<Rigidbody>();

        NonFloatyObject nonFloaty = other.GetComponent<NonFloatyObject>();
        if (nonFloaty != null && nonFloaty.active)
        {
            return;
        }

        if (colliderBod != null)
        {
            float waterHeight = this.GetComponent<Collider>().transform.localScale.y / 2;
            float surfaceHeight = this.transform.position.y + waterHeight;
            float depth = surfaceHeight - colliderBod.transform.position.y;

            Vector3 verticalDirection = new Vector3(0, 1, 0);

            if (colliderBod.velocity.y > 0)
            {
                depth = depth * upwardVelocityPushFactor;
            }

            Vector3 pushForce = verticalDirection * depth * viscosity - (Physics.gravity * normalForceFactor); // This should make it push harder if the object is deeper, but less when it's shallower.
            
            colliderBod.AddForce(pushForce);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (waterEffect.isPlaying)
        //{
        //    return;
        //}

        Rigidbody colliderBody = other.GetComponent<Rigidbody>();
        if (colliderBody == null)
        {
            return;
        }

        int suffix = Random.Range(1, 4);

        float speed = colliderBody.velocity.magnitude * speedMultiplier;

        waterEffect = Instantiate(waterEffectPrefab);
        ParticleSystem.MainModule main = waterEffect.main;
        main.startSpeedMultiplier = speed * 10;
        waterEffect.transform.position = other.transform.position;

        if (waterAudio == null || !waterAudio.isPlaying)
        {
            AudioClip waterSound = (AudioClip)Resources.Load("Sounds/waterSplash" + suffix, typeof(AudioClip));
            waterAudio = waterEffect.gameObject.AddComponent<AudioSource>();
            waterAudio.volume = speed * volumeMultiplier;
            waterAudio.clip = waterSound;

            waterAudio.Play();
        }

        if (speed > 0.03)
        {
            waterEffect.Play();
        }

        StartCoroutine(DestroyWaterEffectWhenComplete(waterEffect));
    }

    public void OnTriggerExit(Collider other)
    {
        OnTriggerEnter(other);
    }

    private IEnumerator DestroyWaterEffectWhenComplete(ParticleSystem waterEffect)
    {
        yield return new WaitForSeconds(waterEffect.main.duration + 1); // wait a little extra to be safe.

        Destroy(waterEffect.gameObject); // this will destroy the newly create splash effect and the audio source;
    }
}
