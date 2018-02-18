using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material : MonoBehaviour {

    public bool metal;
    public bool wood;
    public bool soft;
    public bool glass;

    public ParticleSystem bloodEffectPrefab; // only for things that can bleed
    public bool canBleed;
    private ParticleSystem bloodEffect;

    public float volumeMultiplier = 1;
    public float soundBreakMS = 200;

    private AudioSource audioSource;
    private AudioSource bloodAudio;
    private bool canSound = true;

    private GameSettingsManager gsm;

	// Use this for initialization
	void Start ()
    {
        gsm = FindObjectOfType<GameSettingsManager>();

        if (bloodEffectPrefab != null && gsm.settings.showBlood)
        {
            bloodEffect = Instantiate<ParticleSystem>(bloodEffectPrefab, this.transform);
            bloodAudio = gameObject.AddComponent<AudioSource>();
        }

        volumeMultiplier = volumeMultiplier * gsm.settings.effectsVolume;

        audioSource = gameObject.AddComponent<AudioSource>();
        //Resources.LoadAll("Sounds"); // causes load time on levels to increase dramatically. Also there is still a delay on the first time a sound is loaded.
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        Material colliderMaterial = collision.collider.GetComponent<Material>();
        
        if(ShouldNotMakeSound(collision))
        {
            return;
        }

        float volume = collision.relativeVelocity.magnitude / 100f;

        if (this.metal)
        {
            if (colliderMaterial.metal)
            {
                PlaySound(GameConstants.Sounds.metalOnMetal, volume);
            }

            if (colliderMaterial.wood)
            {
                PlaySound(GameConstants.Sounds.metalOnWood, volume);
            }

            if (colliderMaterial.soft)
            {
                PlaySound(GameConstants.Sounds.metalOnSoft, volume);
            }

            if (colliderMaterial.glass)
            {
                PlaySound(GameConstants.Sounds.metalOnGlass, volume);
            }
        }

        if (this.wood)
        {
            if (colliderMaterial.metal)
            {
                PlaySound(GameConstants.Sounds.metalOnWood, volume);
            }
            
            if (colliderMaterial.wood)
            {
                PlaySound(GameConstants.Sounds.woodOnWood, volume);
            }

            if (colliderMaterial.soft)         
            {                                  
                PlaySound(GameConstants.Sounds.woodOnSoft, volume);
            }

            if (colliderMaterial.glass)
            {
                PlaySound(GameConstants.Sounds.woodOnGlass, volume);
            }
        }

        if (this.soft)
        {
            if (colliderMaterial.metal)
            {
                PlaySound(GameConstants.Sounds.metalOnSoft, volume);
            }

            if (colliderMaterial.wood)
            {
                PlaySound(GameConstants.Sounds.woodOnSoft, volume);
            }

            if (colliderMaterial.soft)
            {
                PlaySound(GameConstants.Sounds.softOnSoft, volume);
            }

            if (colliderMaterial.glass)
            {
                PlaySound(GameConstants.Sounds.softOnGlass, volume);
            }
        }

        if (this.glass)
        {
            if (colliderMaterial.metal)
            {
                PlaySound(GameConstants.Sounds.metalOnGlass, volume);
            }

            if (colliderMaterial.wood)
            {
                PlaySound(GameConstants.Sounds.woodOnGlass, volume);
            }

            if (colliderMaterial.soft)
            {
                PlaySound(GameConstants.Sounds.softOnGlass, volume);
            }

            if (colliderMaterial.glass)
            {
                PlaySound(GameConstants.Sounds.glassOnGlass, volume);
            }
        }
    }

    public void PlaySound(string sound, float volume)
    {
        int suffix = Random.Range(1, 4);
        AudioClip clip = (AudioClip)Resources.Load("Sounds/" + sound + suffix, typeof(AudioClip));

        audioSource.volume = volume * volumeMultiplier;
        audioSource.clip = clip;
        audioSource.Play();
        StartCoroutine(ToggleCanSound());

        if (canBleed && gsm.settings.showBlood && volume > 0.1)
        {
            if (bloodEffect.isPlaying)
            {
                return;
            }

            ParticleSystem.MainModule main = bloodEffect.main;
            main.duration = volume;

            AudioClip bloodSound = (AudioClip)Resources.Load("Sounds/bloodSplat" + suffix, typeof(AudioClip));
            bloodAudio.volume = volume * volumeMultiplier;
            bloodAudio.clip = bloodSound;

            bloodEffect.Play();
            bloodAudio.Play();
        }
    }

    public bool ShouldNotMakeSound(Collision collision)
    {
        Material colliderMaterial = collision.collider.GetComponent<Material>();

        if (colliderMaterial == null || !canSound || audioSource.isPlaying)
        {
            return true;
        }

        BodyPart colliderPart = collision.collider.GetComponent<BodyPart>();
        BodyPart thisPart = this.GetComponent<BodyPart>();

        if (thisPart != null && colliderPart != null)
        {
            if (thisPart.owner == colliderPart.owner)
            {
                return true;
            }
        }

        Sword colliderSword = collision.collider.GetComponent<Sword>();
        Sword thisSword = this.GetComponent<Sword>();
        if (thisSword != null && colliderPart != null)
        {
            if (thisSword.owner == colliderPart.owner)
            {
                return true;
            }
        }

        if (colliderSword != null && thisPart != null)
        {
            if (colliderSword.owner == thisPart.owner)
            {
                return true;
            }
        }

        return false;
    }

    public IEnumerator ToggleCanSound()
    {
        canSound = false;
        yield return new WaitForSeconds(soundBreakMS / 1000f);
        canSound = true;
    }
}
