using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageHazard : MonoBehaviour
{
    public List<IEntity> entities;

    public bool ableToActivate = true;
    public bool ableToDeactivate = true;
    public float duration; // seconds

    public bool limitEntitiesToNumberOfPlayers = false;

    private GameSettingsManager gsm;

	// Use this for initialization
	void Start ()
    {
        gsm = FindObjectOfType<GameSettingsManager>();
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
        for (int i = 0; i < entities.Count; i++)
        {
            if (!limitEntitiesToNumberOfPlayers || i < gsm.numberOfPlayers)
            {
                entities[0].Activate();
            }
        }
    }

    public void Deactivate()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            if (!limitEntitiesToNumberOfPlayers || i < gsm.numberOfPlayers)
            {
                entities[0].Deactivate();
            }
        }
    }
}
