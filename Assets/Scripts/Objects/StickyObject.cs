using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyObject : MonoBehaviour {

    public float breakForce;
    
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody colliderBody = collision.collider.GetComponent<Rigidbody>();

        if (this.GetComponent<BodyPart>() != null && collision.collider.GetComponent<BodyPart>() != null &&
            this.GetComponent<BodyPart>().owner == collision.collider.GetComponent<BodyPart>().owner)
        {
            return;
        }

        if (colliderBody != null)
        {
            FixedJoint joint = this.gameObject.AddComponent<FixedJoint>();

            joint.connectedBody = colliderBody;
            joint.breakForce = breakForce;
        }
    }
}
