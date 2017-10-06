using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBoostPowerup : Pickup
{
    public GameObject bubblePrefab;
    protected override void ExtraEffects(BodyPart recipient)
    {
        base.ExtraEffects(recipient);
        
        StartCoroutine(noBoostLimitForDuration(recipient.owner, duration));
    }

    private IEnumerator noBoostLimitForDuration(Man man, float time)
    {
        GameObject bubble = Instantiate(bubblePrefab);
        Transform spine = man.transform.Find("Body/Body Spine");
        bubble.transform.parent = spine;
        bubble.transform.position = spine.position;
        bubble.transform.rotation = spine.rotation;

        man.invincible = true;

        yield return new WaitForSeconds(time);

        man.invincible = false;
        Destroy(bubble.gameObject);
    }
}
