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
            //FixedJoint joint = collision.collider.gameObject.AddComponent<FixedJoint>();
            //
            //joint.connectedBody = this.GetComponent<Rigidbody>();
            //joint.breakForce = breakForce;
            // ^ this literally destroys the world
            
            float angle = Vector3.Angle(rigbod.velocity, -collision.contacts[0].normal);

            Debug.Log(//"Velocity: " + rigbod.velocity + "\n" + 
                //"Normal: " + -collision.contacts[0].normal + "\n" + 
                "Angle: " + angle);

            if (angle < minNonRigbodStickAngle || angle > 180 - minNonRigbodStickAngle) // TODO: Work on this. Ideally, any narrow angle of impact shouldn't stick
            {
                rigbod.constraints = RigidbodyConstraints.FreezeAll;
            }
            //Destroy(rigBod);
        }
    }
}
