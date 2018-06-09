using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyDestroyer : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rigbod = other.GetComponent<Rigidbody>();

        if (rigbod != null)
        {
            Destroy(rigbod.gameObject);
        }
    }
}
