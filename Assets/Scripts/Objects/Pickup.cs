using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public bool destroyedOnPickup = true;
    public int healthGained;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collider)
    {
        BodyPart recipient = collider.GetComponent<BodyPart>();

        if (recipient != null)
        {
            recipient.owner.ChangeHealth(recipient.owner.health + healthGained);
            ExtraEffects(recipient);
            if (destroyedOnPickup)
            {
                Destroy(this.gameObject);
            }
        }
    }

    protected virtual void ExtraEffects(BodyPart recipient)
    {
        // Implement for extra effects on pickup
    }
}
