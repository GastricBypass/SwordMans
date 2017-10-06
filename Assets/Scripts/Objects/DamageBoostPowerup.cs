using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBoostPowerup : Pickup
{
    public GameObject bubblePrefab;
    private float oldDamageMultiplier;

    protected override void ExtraEffects(BodyPart recipient)
    {
        base.ExtraEffects(recipient);

        Transform weapons = recipient.owner.transform.Find("Weapon");

        DamageMultiplyingObject damagingObject = null;

        for (int i = 0; i < (weapons.childCount); i++)
        {
            if (weapons.GetChild(i).gameObject.activeSelf)
            {
                damagingObject = weapons.GetChild(i).GetComponent<DamageMultiplyingObject>();
                break;
            }
        }

        StartCoroutine(noBoostLimitForDuration(damagingObject, duration));
    }

    private IEnumerator noBoostLimitForDuration(DamageMultiplyingObject damagingObject, float time)
    {
        GameObject bubble = Instantiate(bubblePrefab);
        bubble.transform.parent = damagingObject.transform;
        bubble.transform.position = damagingObject.transform.position;
        bubble.transform.rotation = damagingObject.transform.rotation;

        oldDamageMultiplier = damagingObject.damageMultiplier;
        damagingObject.damageMultiplier = damagingObject.damageMultiplier * 2;

        yield return new WaitForSeconds(time);

        damagingObject.damageMultiplier = oldDamageMultiplier;
        Destroy(bubble.gameObject);
    }
}
