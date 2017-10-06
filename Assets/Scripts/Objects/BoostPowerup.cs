using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPowerup : Pickup
{
    public GameObject bubblePrefab;
    private float oldBoostDrainRate;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

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
        GameObject bubble = Instantiate(bubblePrefab);
        bubble.transform.parent = sword.transform;
        bubble.transform.position = sword.transform.position;
        bubble.transform.rotation = sword.transform.rotation;

        oldBoostDrainRate = sword.boostDrainSpeed;
        sword.boostDrainSpeed = 0;
        
        yield return new WaitForSeconds(time);

        sword.boostDrainSpeed = oldBoostDrainRate;
        Destroy(bubble.gameObject);
    }
}
