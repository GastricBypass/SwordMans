using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelArea : MonoBehaviour {

    public List<int> playersInArea;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Sword player = other.GetComponent<Sword>();

        if (player != null && player.playerNumber != 0 && !playersInArea.Contains(player.playerNumber))
        {
            playersInArea.Add(player.playerNumber);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Sword player = other.GetComponent<Sword>();

        if (player != null && player.playerNumber != 0 && !playersInArea.Contains(player.playerNumber))
        {
            playersInArea.Remove(player.playerNumber);
        }
    }
}
