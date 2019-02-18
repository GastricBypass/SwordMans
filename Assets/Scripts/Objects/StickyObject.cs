using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyObject : IEntity
{
    public bool stickToNonRigidBodies = false;
    public float minNonRigbodStickAngle = 115;
    public float breakForce;
    public List<Rigidbody> immuneToStickObjects;

    private Rigidbody rigbod;
    
	// Use this for initialization
	void Start ()
    {
        rigbod = this.GetComponent<Rigidbody>();
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

        else if (stickToNonRigidBodies)
        {
            float angle = Vector3.Angle(rigbod.velocity, -collision.contacts[0].normal);

            if (angle < minNonRigbodStickAngle || angle > 180 - minNonRigbodStickAngle) //TODO: Works okay, but inconsistent. Ideally, any narrow angle of impact shouldn't stick ever.
            {
                rigbod.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }
}
