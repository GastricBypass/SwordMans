using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

    // Cosmetics
    public bool hasSavedCosmetics = true;

    public List<string> hats;
    public List<string> misc;
    public List<string> versusStages;
    public List<string> coopStages;

    // Settings
    public bool hasSavedSettings = true;

    public bool colorizeHealthBars;
    public bool showHealthValues;

    public int roundsPerStage;
    public bool randomStageSelect;

    public bool musicOn;
    public float musicVolume;
    public float effectsVolume;

    public void Save()
    {
        SaveManager.SaveCosmetics(GameConstants.Files.cosmeticsFileName, this);
        SaveManager.SaveSettings(GameConstants.Files.settingsFileName, this);
    }

    public void Load()
    {
        SaveManager.LoadCosmetics(GameConstants.Files.cosmeticsFileName, this);
        SaveManager.LoadSettings(GameConstants.Files.settingsFileName, this);
    }

    public void UnlockHat(string hatName)
    {
        if (!hats.Contains(hatName) && GameConstants.Unlocks.allHats.Contains(hatName))
        {
            Insert(hats, hatName);
            Save();
        }
    }

    public void UnlockMisc(string miscName)
    {
        if (!misc.Contains(miscName) && GameConstants.Unlocks.allMisc.Contains(miscName))
        {
            Insert(misc, miscName);
            Save();
        }
    }

    public void UnlockVersusStage(string stageName)
    {
        if (!versusStages.Contains(stageName) && GameConstants.Unlocks.allVersusStages.Contains(stageName))
        {
            Insert(versusStages, stageName);
            Save();
        }
    }

    public void UnlockCoopStage(string stageName)
    {
        if (!coopStages.Contains(stageName) && GameConstants.Unlocks.allCoopStages.Contains(stageName))
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

        if (list == hats || list == misc)
        {
            FindObjectOfType<UIManager>().UnlockItem(item);
        }
    }
}
