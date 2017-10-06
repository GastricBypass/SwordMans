using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingArea : IEntity
{
    public float damagePerTick;
    public float tickLengthMS;
    public List<Man> immuneToDamage;
    public bool alwaysDealsDamage = true;

    private void OnTriggerStay(Collider other)
    {
        if (active)
        {
            BodyPart recipient = other.GetComponent<BodyPart>();
            if (recipient != null && !immuneToDamage.Contains(recipient.owner))
            {
                recipient.owner.TakeDamage(damagePerTick, alwaysDealsDamage);
                StartCoroutine(WaitToDealDamageAgain(tickLengthMS, recipient.owner));
            }
            
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
}
