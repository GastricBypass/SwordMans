using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyDestroyer : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rigbod = other.GetComponent<Rigidbody>();
        Sword sword = other.GetComponent<Sword>();
        BodyPart bodyPart = other.GetComponent<BodyPart>();

        if (sword == null && bodyPart == null && rigbod != null) // Don't destroy sword or mans. Just items.
        {
            Destroy(rigbod.gameObject);
        }
    }
}
