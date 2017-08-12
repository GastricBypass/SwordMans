using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggler : MonoBehaviour {

    public float activeTimeMS;
    public float inactiveTimeMS;

    public IEntity target;

    private bool shouldSetActive = false;
    private bool shouldSetInactive = true;

	// Use this for initialization
	void Start () {
        if (target == null)
        {
            target = this.GetComponent<IEntity>();
        }
        if (target == null)
        {
            Destroy(this.gameObject);
        }

        shouldSetActive = target.active;
        shouldSetInactive = !target.active;
        target.active = !target.active;
	}
	
	// Update is called once per frame
	void Update () {
		if (target.active && shouldSetInactive)
        {
            StartCoroutine(SetInactive());
        }
        else if (!target.active && shouldSetActive)
        {
            StartCoroutine(SetActive());
        }
	}

    private IEnumerator SetActive()
    {
        target.active = true;
        shouldSetActive = false;
        yield return new WaitForSeconds(activeTimeMS / 1000f);
        shouldSetInactive = true;
    }

    private IEnumerator SetInactive()
    {
        target.active = false;
        shouldSetInactive = false;
        yield return new WaitForSeconds(inactiveTimeMS / 1000f);
        shouldSetActive = true;
    }
}
