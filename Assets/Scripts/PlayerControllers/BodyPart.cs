using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour {

    public Man owner;
    public float damageMultiplier = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (owner.ui != null && owner.ui.useInBounds && OutOfBounds())
        {
            owner.TakeDamage(1000f);
            //Destroy(owner.gameObject);
            StartCoroutine(DestroyAfterTime(owner.gameObject, 0.5f));
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody colliderBod = collision.collider.GetComponent<Rigidbody>();
        Rigidbody thisBod = this.GetComponent<Rigidbody>();

        if (colliderBod != null)
        {
            float damage = 0;
            if (thisBod != null)
            {
                //damage = colliderBod.mass * Mathf.Pow((((thisBod.velocity * 0.2f) - colliderBod.velocity).magnitude), 2); // scaled individual velocity and collider's velocity squared times collider's mass
                //damage = Mathf.Pow(collision.relativeVelocity.magnitude, 2) * colliderBod.mass; // Kinetic Energy
                //damage = collision.relativeVelocity.magnitude * colliderBod.mass; // Momentum
                //damage = Vector3.Dot(collision.contacts[0].normal, collision.relativeVelocity) * colliderBod.mass; // something else
                //damage = colliderBod.mass * colliderBod.velocity.magnitude * (thisBod.velocity - colliderBod.velocity).magnitude; // Momentum of collider times velocity difference
                //damage = colliderBod.mass * colliderBod.velocity.magnitude * collision.relativeVelocity.magnitude; // Momentum of collider times velocity difference
                damage = collision.impulse.magnitude; // It can't be that simple, can it?
                if (colliderBod.velocity.magnitude < 0.05)
                {
                    damage = 0;
                }
            }
            else
            {
                damage = collision.impulse.magnitude;
            }

            // no damage if it is a body part with the same owner
            BodyPart bodyPart = collision.collider.GetComponent<BodyPart>();
            if (bodyPart != null)
            {
                if (bodyPart.owner == this.owner)
                {
                    damage = 0;
                }
            }

            // no damage if it is hit with its own weapon
            Sword sword = collision.collider.GetComponent<Sword>();
            if (sword != null)
            {
                if (sword.owner == this.owner)
                {
                    damage = 0;
                }
            }

            // extra damage from weapons or other damaging objects
            DamageMultiplyingObject extraDamageObject = collision.collider.GetComponent<DamageMultiplyingObject>();
            if (extraDamageObject != null)
            {
                damage = damage * extraDamageObject.damageMultiplier; 
            
            }

            owner.TakeDamage(damage * damageMultiplier);
        }
    }

    public bool OutOfBounds()
    {
        if (
            transform.position.x < owner.ui.gsm.inBoundsMin.x ||
            transform.position.x > owner.ui.gsm.inBoundsMax.x ||
            transform.position.y < owner.ui.gsm.inBoundsMin.y ||
            transform.position.y > owner.ui.gsm.inBoundsMax.y ||
            transform.position.z < owner.ui.gsm.inBoundsMin.z ||
            transform.position.z > owner.ui.gsm.inBoundsMax.z )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator DestroyAfterTime(GameObject toDestroy, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(toDestroy);
    }
}
