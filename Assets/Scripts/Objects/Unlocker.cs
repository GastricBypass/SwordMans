using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlocker : IEntity {

    public List<string> hatsToUnlock;
    public List<string> miscToUnlock;
    public List<string> skinsToUnlock;
    public List<string> versusStagesToUnlock;
    public List<string> coopStagesToUnlock;

    public bool destroyOnUnlock = true;

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
            if (UnlockAllItems() && destroyOnUnlock) // Anything was unlocked by calling this
            {
                Destroy(this.gameObject);
            }
        }
    }

    private bool UnlockAllItems()
    {
        bool unlockedAnything = false;

        for (int i = 0; i < hatsToUnlock.Count; i++)
        {
            if (!gsm.data.HasItem(hatsToUnlock[i])) // The unlocker shouldn't unlock things that have already been unlocked
            {
                gsm.data.UnlockHat(hatsToUnlock[i]);
                unlockedAnything = true;
            }
        }

        for (int i = 0; i < miscToUnlock.Count; i++)
        {
            if (!gsm.data.HasItem(miscToUnlock[i]))
            {
                gsm.data.UnlockMisc(miscToUnlock[i]);
                unlockedAnything = true;
            }
        }

        for (int i = 0; i < skinsToUnlock.Count; i++)
        {
            if (!gsm.data.HasItem(skinsToUnlock[i]))
            {
                gsm.data.UnlockSkin(skinsToUnlock[i]);
                unlockedAnything = true;
            }
        }

        for (int i = 0; i < versusStagesToUnlock.Count; i++)
        {
            gsm.data.UnlockVersusStage(versusStagesToUnlock[i]);
            unlockedAnything = true;
        }

        for (int i = 0; i < coopStagesToUnlock.Count; i++)
        {
            gsm.data.UnlockCoopStage(coopStagesToUnlock[i]);
            unlockedAnything = true;
        }

        return unlockedAnything;
    }
}
