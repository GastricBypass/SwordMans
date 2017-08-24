using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material : MonoBehaviour {

    public bool metal;
    public bool wood;
    public bool soft;
    public bool glass;

    public float volumeMultiplier = 1;
    public float soundBreakMS = 200;

    private AudioSource audioSource;
    private bool canSound = true;

	// Use this for initialization
	void Start ()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
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
        int suffix = (int)Random.Range(1, 4);
        AudioClip clip = (AudioClip)Resources.Load("Sounds/" + sound + suffix, typeof(AudioClip));

        audioSource.volume = volume * volumeMultiplier;
        audioSource.clip = clip;
        audioSource.Play();
        StartCoroutine(ToggleCanSound());
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
