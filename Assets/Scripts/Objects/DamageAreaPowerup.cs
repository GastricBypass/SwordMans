using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAreaPowerup : Pickup
{
    public DamagingArea damagingArea;

    private DamagingArea fire; // I don't like having these instances stored globally, but I need to be able to access them in OnDestroy().
                               // Otherwise, if they're destroyed before the duration destroys them, the effect will last forever.
                               // It would probably be better to have some kind of powerup mananger that handles them. Then they could just
                               // get destroyed whenever and it wouldn't matter as long as the powerup mananger didn't get destroyed.

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
        fire = Instantiate(damagingArea);
        fire.transform.parent = sword.transform;
        fire.immuneToDamage.Add(sword.owner);
        fire.transform.position = sword.transform.position;
        fire.transform.rotation = sword.transform.rotation;

        yield return new WaitForSeconds(time);

        Destroy(fire.gameObject);
    }

    private void OnDestroy()
    {
        if (fire != null) Destroy(fire.gameObject);
    }
}
