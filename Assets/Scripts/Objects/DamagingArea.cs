using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingArea : IEntity
{
    public float damagePerTick; // damage done at the center of the area
    public bool useDamageFalloff = false;
    public float falloffAmount;
    public float minDamagePerTick; // damage done at the very edge of the area
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
                float damage = GetDamage(other);

                recipient.owner.TakeDamage(damage);
                recipient.owner.isInDamagingArea = true; // AI: to determine if they are in a damage dealing area.

                StartCoroutine(WaitToDealDamageAgain(tickLengthMS, recipient.owner));
            }
        }
    }

    private float GetDamage(Collider other)
    {
        if (!useDamageFalloff)
        {
            return damagePerTick;
        }

        float distance = Vector3.Distance(other.transform.position, this.transform.position);
        float damageReduction = distance * falloffAmount;
        float newDamage = damagePerTick - damageReduction;
        if (newDamage < minDamagePerTick)
        {
            newDamage = minDamagePerTick;
        }

        return newDamage;
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
