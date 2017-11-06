﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlocker : IEntity {

    public List<string> hatsToUnlock;
    public List<string> miscToUnlock;
    public List<string> versusStagesToUnlock;
    public List<string> coopStagesToUnlock;

    private GameSettingsManager gsm;
    
	void Start ()
    {
        gsm = FindObjectOfType<GameSettingsManager>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!active)
        {
            return;
        }

        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            UnlockAllItems();
            Destroy(this.gameObject);
        }
    }

    private void UnlockAllItems()
    {
        for (int i = 0; i < hatsToUnlock.Count; i++)
        {
            if (!gsm.data.HasItem(hatsToUnlock[i])) // The unlocker shouldn't unlock things that have already been unlocked
            {
                gsm.data.UnlockHat(hatsToUnlock[i]);
            }
        }

        for (int i = 0; i < miscToUnlock.Count; i++)
        {
            if (!gsm.data.HasItem(hatsToUnlock[i]))
            {
                gsm.data.UnlockMisc(miscToUnlock[i]);
            }
        }

        for (int i = 0; i < versusStagesToUnlock.Count; i++)
        {
            gsm.data.UnlockVersusStage(versusStagesToUnlock[i]);
        }

        for (int i = 0; i < coopStagesToUnlock.Count; i++)
        {
            gsm.data.UnlockCoopStage(coopStagesToUnlock[i]);
        }
    }
}
