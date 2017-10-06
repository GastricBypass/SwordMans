using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAreaPowerup : Pickup
{
    public DamagingArea damagingArea;

    protected override void ExtraEffects(BodyPart recipient)
    {
        base.ExtraEffects(recipient);

        Transform weapons = recipient.owner.transform.Find("Weapon");

        Sword sword = null;

        for (int i = 0; i < (weapons.childCount); i++)
        {
            if (weapons.GetChild(i).gameObject.activeSelf)
            {
                sword = weapons.GetChild(i).GetComponent<Sword>();
                break;
            }
        }

        StartCoroutine(noBoostLimitForDuration(sword, duration));
    }

    private IEnumerator noBoostLimitForDuration(Sword sword, float time)
    {
        DamagingArea fire = Instantiate(damagingArea);
        fire.transform.parent = sword.transform;
        fire.immuneToDamage.Add(sword.owner);
        fire.transform.position = sword.transform.position;
        fire.transform.rotation = sword.transform.rotation;

        yield return new WaitForSeconds(time);

        Destroy(fire.gameObject);
    }
}
