using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public bool destroyedOnPickup = true;
    public int healthGained;

    public float duration; // there needs to be a better way to do this. If someone has the effects of a pickup when another spawns, they keep the effects forever.

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
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
                
                StartCoroutine(WaitToDestroy(this.gameObject, duration));
                this.GetComponent<Collider>().enabled = false;
                this.GetComponent<MeshRenderer>().enabled = false;
                //this.gameObject.SetActive(false); // hide the object and destroy later commenting this out makes it actually get to the end, but it doesn't set everything back to normal
            }
        }
    }

    protected virtual void ExtraEffects(BodyPart recipient)
    {
        // Implement for extra effects on pickup
    }

    protected virtual IEnumerator WaitToDestroy(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time + 1);
        Destroy(obj);
    }
}
