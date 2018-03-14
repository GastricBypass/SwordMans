using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPowerup : Pickup
{
    public GameObject bubblePrefab;

    private float oldBoostDrainRate;
    private Sword swordToEnhance;
    private GameObject bubble;

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
                if (sword == null)
                {
                    sword = weapons.GetChild(i).GetComponentInChildren<Sword>();
                }

                break;
            }
        }

        StartCoroutine(noBoostLimitForDuration(sword, duration));
    }

    private IEnumerator noBoostLimitForDuration(Sword sword, float time)
    {
        swordToEnhance = sword;

        bubble = Instantiate(bubblePrefab);
        bubble.transform.parent = sword.transform;
        bubble.transform.position = sword.transform.position;
        bubble.transform.rotation = sword.transform.rotation;

        oldBoostDrainRate = sword.boostDrainSpeed;
        sword.boostDrainSpeed = 0;
        
        yield return new WaitForSeconds(time);

        sword.boostDrainSpeed = oldBoostDrainRate;
        Destroy(bubble.gameObject);
    }

    private void OnDestroy()
    {
        if (swordToEnhance != null) swordToEnhance.boostDrainSpeed = oldBoostDrainRate;
        if (bubble != null) Destroy(bubble.gameObject);
    }
}
