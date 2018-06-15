using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageHazard : MonoBehaviour
{
    public List<IEntity> entities;

    public bool ableToActivate = true;
    public bool ableToDeactivate = true;
    public float duration; // seconds

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void TriggerHazard()
    {
        StartCoroutine(SetOffHazard());
    }

    public IEnumerator SetOffHazard()
    {
        if (ableToActivate)
        {
            Activate();
        }

        yield return new WaitForSeconds(duration);

        if (ableToDeactivate)
        {
            Deactivate();
        }
    }
    
    public void Activate()
    {
        foreach (IEntity entity in entities)
        {
            entity.Activate();
        }
    }

    public void Deactivate()
    {
        foreach (IEntity entity in entities)
        {
            entity.Deactivate();
        }
    }
}
