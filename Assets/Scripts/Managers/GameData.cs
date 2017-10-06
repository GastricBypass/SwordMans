using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

    public List<string> hats;
    public List<string> misc;
    public List<string> versusStages;
    public List<string> coopStages;
	
	public void Save()
    {
        SaveManager.Save(GameConstants.Files.dataFileName, this);
	}

    public void UnlockHat(string hatName)
    {
        if (!hats.Contains(hatName))
        {
            Insert(hats, hatName);
            Save();
        }
    }

    public void UnlockMisc(string miscName)
    {
        if (!misc.Contains(miscName))
        {
            Insert(misc, miscName);
            Save();
        }
    }

    public void UnlockVersusStage(string stageName)
    {
        if (!versusStages.Contains(stageName) && stageName != "Main Menu")
        {
            Insert(versusStages, stageName);
            Save();
        }
    }

    public void UnlockCoopStage(string stageName)
    {
        if (!coopStages.Contains(stageName) && stageName != "Main Menu")
        {
            Insert(coopStages, stageName);
            Save();
        }
    }
    
    public void Insert(List<string> list, string item)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (item.CompareTo(list[i]) <= 0)
            {
                list.Insert(i, item);
                break;
            }

            if (i == list.Count - 1)
            {
                list.Add(item);
                break;
            }
        }
    }
}
