using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))] // It also must be a trigger
public class Teleporter : MonoBehaviour
{
    public Transform exit;
    public bool freezeOnTeleport = false; // If a man enters, it will only freeze the body part that triggered it
    public float minimumTimeToTeleportTheSameObject = 0.05f;

    public AudioClip teleportSound;
    public bool playSoundOnTeleportMan = true;
    public bool playSoundOnTeleportObject = true;

    private List<GameObject> objectsThatHaveBeenTeleportedRecently = new List<GameObject>();
    private AudioSource audioSource;

	// Use this for initialization
	void Start ()
    {
        audioSource = this.gameObject.AddComponent<AudioSource>();

        audioSource.clip = teleportSound;
        audioSource.volume = FindObjectOfType<GameSettingsManager>().settings.effectsVolume;
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OnTriggerEnter(Collider other)
    {
        Rigidbody colliderBod = FindClosestRigidbody(other.transform);

        if (colliderBod != null)
        {
            Teleport(colliderBod);
        }
    }

    public Rigidbody FindClosestRigidbody(Transform obj)
    {
        Rigidbody body = obj.GetComponent<Rigidbody>();

        if (body == null)
        {
            if (obj.parent != null)
            {
                body = FindClosestRigidbody(obj.parent);
            }
        }

        return body;
    }
    
    public void Teleport(Rigidbody colliderBod)
    {
        if (freezeOnTeleport)
        {
            colliderBod.velocity = new Vector3();
        }
        
        Transform colliderTransform = colliderBod.transform;
        Vector3 positionDifference = this.transform.position - colliderTransform.position;

        BodyPart bodyPart = colliderBod.GetComponent<BodyPart>();
        Sword sword = colliderBod.GetComponent<Sword>();
        if (bodyPart != null)
        {
            if (!objectsThatHaveBeenTeleportedRecently.Contains(bodyPart.owner.gameObject))
            {
                TeleportMan(bodyPart.owner);
                StartCoroutine(AddThenRemoveFromList(bodyPart.owner.gameObject));
            }
        }
        else if (sword != null)
        {
            if (!objectsThatHaveBeenTeleportedRecently.Contains(sword.owner.gameObject))
            {
                TeleportMan(sword.owner);
                StartCoroutine(AddThenRemoveFromList(sword.owner.gameObject));
            }
        }
        else
        {
            if (!objectsThatHaveBeenTeleportedRecently.Contains(colliderBod.gameObject))
            {
                if (playSoundOnTeleportObject)
                {
                    audioSource.Play();
                }

                colliderTransform.position = exit.transform.position - positionDifference;
                StartCoroutine(AddThenRemoveFromList(colliderBod.gameObject));
            }
        }
    }

    public void TeleportMan(Man man)
    {
        if (playSoundOnTeleportMan)
        {
            audioSource.Play();
        }

        Vector3 positionAdditive = exit.transform.position - this.transform.position;
        man.transform.position += positionAdditive;
    }

    public IEnumerator AddThenRemoveFromList(GameObject obj)
    {
        objectsThatHaveBeenTeleportedRecently.Add(obj);

        yield return new WaitForSeconds(minimumTimeToTeleportTheSameObject);

        objectsThatHaveBeenTeleportedRecently.Remove(obj);
    }
}
