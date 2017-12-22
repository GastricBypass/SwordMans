using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyObject : IEntity {

    public float breakForce;
    public List<Rigidbody> immuneToStickObjects;
    
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
        if (!active)
        {
            return;
        }

        Rigidbody colliderBody = collision.collider.GetComponent<Rigidbody>();

        if (this.GetComponent<BodyPart>() != null && collision.collider.GetComponent<BodyPart>() != null &&
            this.GetComponent<BodyPart>().owner == collision.collider.GetComponent<BodyPart>().owner)
        {
            return;
        }

        if (colliderBody != null)
        {
            if (immuneToStickObjects.Contains(colliderBody))
            {
                return;
            }

            FixedJoint joint = this.gameObject.AddComponent<FixedJoint>();

            joint.connectedBody = colliderBody;
            joint.breakForce = breakForce;
        }
    }
}
