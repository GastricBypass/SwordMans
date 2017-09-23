using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingArea : IEntity
{
    public float damagePerTick;
    //public float tickLengthMS; not working properly. When active, not all people 

    //private System.DateTime lastTick;

    private void OnTriggerStay(Collider other)
    {
        if (active)
        {
            //if ((System.DateTime.Now - lastTick).TotalMilliseconds > tickLengthMS)
            //{
            //lastTick = System.DateTime.Now;
            BodyPart[] recipients = other.GetComponents<BodyPart>();
            for (int i = 0; i < recipients.Length; i++)
            {
                recipients[i].owner.TakeDamage(damagePerTick);
            }
            //}
        }
    }
}
