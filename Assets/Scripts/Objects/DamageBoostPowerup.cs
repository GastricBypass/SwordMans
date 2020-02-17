using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBoostPowerup : Pickup
{
    public GameObject bubblePrefab;

    private float oldDamageMultiplier;
    private DamageMultiplyingObject damagingObjectToEnhance;
    private GameObject bubble;

    protected override void ExtraEffects(BodyPart recipient)
    {
        base.ExtraEffects(recipient);

        Transform weapons = recipient.owner.transform.Find("Weapon");

        DamageMultiplyingObject damagingObject = null;

        for (int i = 0; i < weapons.childCount; i++)
        {
            if (weapons.GetChild(i).gameObject.activeSelf)
            {
                damagingObject = weapons.GetChild(i).GetComponent<DamageMultiplyingObject>();
                if (damagingObject == null)
                {
                    damagingObject = weapons.GetChild(i).Find("FlailHead").GetComponent<DamageMultiplyingObject>(); // TODO: Might be a gun, maybe make a new extension of DamageMultiplying object that applies the multiplier to bullets
                }

                break;
            }
        }

        StartCoroutine(noBoostLimitForDuration(damagingObject, duration));
    }

    private IEnumerator noBoostLimitForDuration(DamageMultiplyingObject damagingObject, float time)
    {
        damagingObjectToEnhance = damagingObject;

        bubble = Instantiate(bubblePrefab);
        bubble.transform.parent = damagingObject.transform;
        bubble.transform.position = damagingObject.transform.position;
        bubble.transform.rotation = damagingObject.transform.rotation;

        oldDamageMultiplier = damagingObject.damageMultiplier;
        damagingObject.damageMultiplier = damagingObject.damageMultiplier * 2;

        yield return new WaitForSeconds(time);

        damagingObject.damageMultiplier = oldDamageMultiplier;
        Destroy(bubble.gameObject);
    }

    private void OnDestroy()
    {
        if (damagingObjectToEnhance != null) damagingObjectToEnhance.damageMultiplier = oldDamageMultiplier;
        if (bubble != null) Destroy(bubble.gameObject);
    }
}
