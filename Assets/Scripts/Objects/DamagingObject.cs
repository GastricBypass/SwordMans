using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingObject : IEntity {

    public float damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (active)
        {
            BodyPart recipient = collision.collider.GetComponent<BodyPart>();
            if (recipient != null)
            {
                recipient.owner.TakeDamage(damage);
            }
        }
    }
}
