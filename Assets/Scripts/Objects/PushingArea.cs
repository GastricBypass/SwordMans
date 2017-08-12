using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingArea : IEntity {

    public Vector3 pushForce;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (active)
        {
            Rigidbody colliderBod = other.GetComponent<Rigidbody>();

            if (colliderBod != null)
            {
                colliderBod.AddForce(pushForce);
            }
        }
    }
}
