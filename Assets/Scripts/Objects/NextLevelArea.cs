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

        if (player != null && player.playerNumber != 0 && playersInArea.Contains(player.playerNumber))
        {
            playersInArea.Remove(player.playerNumber);
        }
    }

    // This actually checks that all of the player numbers in the area are as expected.
    public bool AllPlayersInArea(int numPlayers, bool[] deadPlayers)
    {
        int numOfPlayersInArea = 0;

        for (int i = 0; i < numPlayers; i++)
        {
            if (playersInArea.Contains(i + 1))
            {
                numOfPlayersInArea++;
            }
            else if (deadPlayers[i])
            {
                numOfPlayersInArea++;
            }
        }

        return numPlayers <= numOfPlayersInArea;
    }
}
