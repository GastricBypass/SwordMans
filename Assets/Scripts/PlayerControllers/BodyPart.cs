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
            if (!owner.hasExploded)
            {
                owner.hasExploded = true;

                float playerHealth = owner.health;
                owner.ChangeHealth(playerHealth - 1000f);

                if (playerHealth > 0) // Their original health before taking the out of bounds damage, so if it was 0, they were already dead and don't need to be respawned.
                {
                    owner.ui.gsm.RespawnPlayers(new int[] { owner.playerNumber }, playerHealth);
                }
                
                //Destroy(owner.gameObject);
                StartCoroutine(DestroyAfterTime(owner.gameObject, 0.5f));
            }
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        //float damage = colliderBod.mass * Mathf.Pow((((thisBod.velocity * 0.2f) - colliderBod.velocity).magnitude), 2); // scaled individual velocity and collider's velocity squared times collider's mass
        //float damage = Mathf.Pow(collision.relativeVelocity.magnitude, 2) * colliderBod.mass; // Kinetic Energy
        //float damage = collision.relativeVelocity.magnitude * colliderBod.mass; // Momentum
        //float damage = Vector3.Dot(collision.contacts[0].normal, collision.relativeVelocity) * colliderBod.mass; // something else
        //float damage = colliderBod.mass * colliderBod.velocity.magnitude * (thisBod.velocity - colliderBod.velocity).magnitude; // Momentum of collider times velocity difference
        //float damage = colliderBod.mass * colliderBod.velocity.magnitude * collision.relativeVelocity.magnitude; // Momentum of collider times velocity difference
        float damage = collision.impulse.magnitude; // It can't be that simple, can it?

        damage = CalculateDamage(damage, collision.collider.transform);

        owner.TakeDamage(damage * damageMultiplier);
    }

    public float CalculateDamage(float damage, Transform colliderTransform)
    {
        Rigidbody colliderBod = colliderTransform.GetComponent<Rigidbody>();

        if (colliderBod == null)
        {
            Transform parent = colliderTransform.parent;
            if (parent == null)
            {
                // Achievement: Air Time
                owner.boostUsedSinceLanding = 0; // Because at this point we know we have collided with a non-rigidbody.

                return 0;
            }

            return CalculateDamage(damage, parent); // This will go up the hierarchy until it finds a parent with a rigidbody
                                                    // This will allow you to make more complex rigidbodies with colliders as children that will still deal damage
        }
        
        // no damage if it hits a very slow moving object (such as landing on a crate)
        if (colliderBod.velocity.magnitude < 0.05)
        {
            // Achievement: Air Time
            //owner.boostUsedSinceLanding = 0; // Also reset if the thing isn't moving, so you get reset from landing on a box or whatever.
                                               // Got rid of this because it could potentially reset on your body if it's moving slowly.

            damage = 0;
        }

        // no damage if it is a body part with the same owner
        BodyPart bodyPart = colliderTransform.GetComponent<BodyPart>();
        if (bodyPart != null)
        {
            if (bodyPart.owner == this.owner)
            {
                damage = 0;
            }
        }

        bool fromSword = false;
        // no damage if it is hit with its own weapon
        Sword sword = colliderTransform.GetComponent<Sword>();
        if (sword != null)
        {
            if (sword.owner == this.owner)
            {
                damage = 0;
            }

            fromSword = true;
        }

        // extra damage from weapons or other damaging objects
        DamageMultiplyingObject extraDamageObject = colliderTransform.GetComponent<DamageMultiplyingObject>();
        if (extraDamageObject != null)
        {
            damage = damage * extraDamageObject.damageMultiplier;
            if (extraDamageObject.immuneToDamage.Contains(this.owner))
            {
                damage = 0;
            }
        }

        if (fromSword && damage * damageMultiplier >= 500 && owner.CanTakeDamage())
        {
            // Achievement: Heavy Hitter
            owner.ui.gsm.steam.UnlockAchievement(GameConstants.AchievementId.HEAVY_HITTER);
        }

        return damage;
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
