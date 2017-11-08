using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingArea : IEntity {

    public Vector3 pushForce;

    public bool changePushForcePeriodically = true;
    public List<Vector3> possiblePushForces;
    public float switchTime = 30;
    public bool useTogglerSwitchTime = true; // requires that a toggler is attached to this gameobject

	// Use this for initialization
	void Start ()
    {
        Toggler toggler = this.GetComponent<Toggler>();
        
        if (toggler != null && useTogglerSwitchTime)
        {
            switchTime = (toggler.activeTimeMS + toggler.inactiveTimeMS) / 1000;
        }

        if (changePushForcePeriodically)
        {
            StartCoroutine(ChangeWindForceAfterTime(switchTime));
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public IEnumerator ChangeWindForceAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        ChangeWindDirection();

        StartCoroutine(ChangeWindForceAfterTime(time));
    }

    public void ChangeWindDirection()
    {
        int newPushForceIndex = Random.Range(0, possiblePushForces.Count);
        pushForce = possiblePushForces[newPushForceIndex];
    }

    private void OnTriggerStay(Collider other)
    {
        if (active)
        {
            Rigidbody colliderBod = other.GetComponent<Rigidbody>();

            if (colliderBod != null)
            {
                colliderBod.AddForce(pushForce);
            }
        }
    }
}
