using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingArea : IEntity
{
    public float damagePerTick;
    public float tickLengthMS;
    public List<Man> immuneToDamage;
    //public bool alwaysDealsDamage = true;

    private void OnTriggerStay(Collider other)
    {
        if (active)
        {
            BodyPart recipient = other.GetComponent<BodyPart>();
            if (recipient != null && !immuneToDamage.Contains(recipient.owner))
            {
                recipient.owner.TakeDamage(damagePerTick);
                recipient.owner.isInDamagingArea = true; // AI: to determine if they are in a damage dealing area.

                StartCoroutine(WaitToDealDamageAgain(tickLengthMS, recipient.owner));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BodyPart recipient = other.GetComponent<BodyPart>();
        if (recipient != null)
        {
            StartCoroutine(StopAIBoostingAfterTime(0.5f, recipient)); // AI: to determine if they are in a damage dealing area.
        }
    }

    private IEnumerator WaitToDealDamageAgain(float time, Man man)
    {
        immuneToDamage.Add(man);

        yield return new WaitForSeconds(time / 1000f);

        if (man != null)
        {
            immuneToDamage.Remove(man);
        }
    }

    // AI: to determine if they are in a damage dealing area.
    public IEnumerator StopAIBoostingAfterTime(float time, BodyPart recipient)
    {
        yield return new WaitForSeconds(time);

        recipient.owner.isInDamagingArea = false;
    }
}
