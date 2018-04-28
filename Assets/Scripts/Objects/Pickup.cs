using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public bool destroyedOnPickup = true;
    public int healthGained;

    public AudioClip pickupNoise;
    private AudioSource audioSource;

    public float duration; // there needs to be a better way to do this. If someone has the effects of a pickup when another spawns, they keep the effects forever.

    protected GameSettingsManager gsm;

    // Use this for initialization
    void Start ()
    {
        gsm = FindObjectOfType<GameSettingsManager>();

        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = pickupNoise;
        audioSource.volume = gsm.settings.effectsVolume;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider collider)
    {
        BodyPart recipient = collider.GetComponent<BodyPart>();

        if (recipient != null)
        {
            audioSource.Play();

            float newHealth = recipient.owner.health + healthGained;
            recipient.owner.ChangeHealth(newHealth);

            float healthActuallyGained = healthGained;
            if (newHealth > recipient.owner.maxHealth)
            {
                healthActuallyGained = healthGained - (newHealth - recipient.owner.maxHealth);
            }
            // Stat: hp_recovered
            gsm.steam.AddHpRecovered(healthActuallyGained);

            ExtraEffects(recipient);
            if (destroyedOnPickup)
            {
                
                StartCoroutine(WaitToDestroy(this.gameObject, duration));
                this.GetComponent<Collider>().enabled = false;
                this.GetComponent<MeshRenderer>().enabled = false;
                //this.gameObject.SetActive(false); // hide the object and destroy later commenting this out makes it actually get to the end, but it doesn't set everything back to normal
            }
        }
    }

    protected virtual void ExtraEffects(BodyPart recipient)
    {
        // Implement for extra effects on pickup
    }

    protected virtual IEnumerator WaitToDestroy(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time + 1);
        Destroy(obj);
    }
}
