using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

    public float viscosity = 5; // 0 = air
    public float upwardVelocityPushFactor = 0.25f; // how much it pushes while moving up compared to moving down.
    public float normalForceFactor = 1.8f; // How much gravity is multiplied by before being subtracted from the force.

    //public Vector3 verticalDirection = new Vector3(0, 1, 0); // Maybe it'll always be up. Consider allowing this later.

    private void OnTriggerStay(Collider other)
    {
        Rigidbody colliderBod = other.GetComponent<Rigidbody>();

        NonFloatyObject nonFloaty = other.GetComponent<NonFloatyObject>();
        if (nonFloaty != null && nonFloaty.active)
        {
            return;
        }

        if (colliderBod != null)
        {
            float waterHeight = this.GetComponent<Collider>().transform.localScale.y / 2;
            float surfaceHeight = this.transform.position.y + waterHeight;
            float depth = surfaceHeight - colliderBod.transform.position.y;

            Vector3 verticalDirection = new Vector3(0, 1, 0);

            if (colliderBod.velocity.y > 0)
            {
                depth = depth * upwardVelocityPushFactor;
            }

            Vector3 pushForce = verticalDirection * depth * viscosity - (Physics.gravity * normalForceFactor); // This should make it push harder if the object is deeper, but less when it's shallower.
            
            colliderBod.AddForce(pushForce);
        }
    }
}
