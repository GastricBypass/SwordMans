using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialPushingArea : IEntity
{
    public float pushMagnitude;

    public bool pullInsteadOfPush = false;

    private void OnTriggerStay(Collider other)
    {
        if (active)
        {
            Rigidbody colliderBod = other.GetComponent<Rigidbody>();

            if (colliderBod != null)
            {
                // This will push things away from the center of the trigger area, unless the trigger isn't centered on the object.
                Vector3 pushForce = (colliderBod.transform.position - this.transform.position).normalized * pushMagnitude;

                if (pullInsteadOfPush)
                {
                    pushForce = pushForce * -1;
                }

                colliderBod.AddForce(pushForce);
            }
        }
    }
}
