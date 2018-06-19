using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public DateTime lastPlayDate;

    // Cosmetics
    public bool hasSavedCosmetics = true;
    public float gold;

    public List<string> hats;
    public List<string> misc;
    public List<string> skins;
    public List<string> versusStages;
    public List<string> coopStages;
    public List<string> arenaStages;

    public List<string> shopItems;

    // Settings
    public bool hasSavedSettings = true;

    public bool colorizeHealthBars;
    public bool showHealthValues;
    public bool showBlood;

    public int roundsPerStage;
    public bool randomStageSelect;

    public bool windowed;
    public int resolution;
    public int graphicsQuality;

    public bool musicOn;
    public float musicVolume;
    public float effectsVolume;

    public List<string> weaponsUsed;

    public void Save()
    {
        SaveManager.SaveCosmetics(GameConstants.Files.cosmeticsFileName, this);
        SaveManager.SaveSettings(GameConstants.Files.settingsFileName, this);
    }

    public void SaveCosmetics()
    {
        SaveManager.SaveCosmetics(GameConstants.Files.cosmeticsFileName, this);
    }

    public void SaveSettings()
    {
        SaveManager.SaveSettings(GameConstants.Files.settingsFileName, this);
    }

    public void Load()
    {
        SaveManager.LoadCosmetics(GameConstants.Files.cosmeticsFileName, this);
        SaveManager.LoadSettings(GameConstants.Files.settingsFileName, this);
    }

    public void UnlockHat(string hatName)
    {
        if (hats.Contains(hatName))
        {
            float price = 0;
            GameConstants.Unlocks.hatPrices.TryGetValue(hatName, out price);

            AddGold(price / 5);
        }

        else if (GameConstants.Unlocks.allHats.Contains(hatName))
        {
            Insert(hats, hatName);
            SaveCosmetics();
        }
    }

    public void UnlockMisc(string miscName)
    {
        if (misc.Contains(miscName))
        {
            float price = 0;
            GameConstants.Unlocks.miscPrices.TryGetValue(miscName, out price);

            AddGold(price / 5);
        }

        else if (GameConstants.Unlocks.allMisc.Contains(miscName))
        {
            Insert(misc, miscName);
            SaveCosmetics();
        }
    }

    public void UnlockSkin(string skinName)
    {
        if (skins.Contains(skinName))
        {
            return;
        }

        else if (GameConstants.Unlocks.allSkins.Contains(skinName)) 
        {
            Insert(skins, skinName);
            SaveCosmetics();
        }
    }

    public void UnlockVersusStage(string stageName)
    {
        if (!versusStages.Contains(stageName) && GameConstants.Unlocks.allVersusStages.Contains(stageName))
        {
            Insert(versusStages, stageName);
            SaveCosmetics();
        }
    }

    public void UnlockCoopStage(string stageName)
    {
        if (!coopStages.Contains(stageName) && GameConstants.Unlocks.allCoopStages.Contains(stageName))
        {
            Insert(coopStages, stageName);
            SaveCosmetics();
        }
    }

    public void UnlockArenaStage(string stageName)
    {
        if (!arenaStages.Contains(stageName) && GameConstants.Unlocks.allArenaStages.Contains(stageName))
        {
            Insert(arenaStages, stageName);
            SaveCosmetics();
        }
    }

    public void AddGold(float amount)
    {
        if (amount == 0)
        {
            return;
        }

        gold += amount;
        FindObjectOfType<UIManager>().UnlockItem("gold coins", amount.ToString() + " ");

        // Achievement: Hoarder
        if (gold >= 200)
        {
            FindObjectOfType<GameSettingsManager>().steam.UnlockAchievement(GameConstants.AchievementId.HOARDER);
        }

        SaveCosmetics();
    }

    // Achievement: Well Rounded
    public void AddWeaponUsed(string weapon)
    {
        if (!weaponsUsed.Contains(weapon))
        {
            weaponsUsed.Add(weapon);
            Save();

            FindObjectOfType<GameSettingsManager>().steam.AddWeaponsUsed(1);
        }
    }

    public bool HasItem(string itemName)
    {
        return misc.Contains(itemName) || hats.Contains(itemName) || skins.Contains(itemName);
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

        if (list == hats || list == misc || list == skins)
        {
            FindObjectOfType<UIManager>().UnlockItem(item);
        }
        if (list == versusStages)
        {
            FindObjectOfType<UIManager>().UnlockItem(item);
        }
    }
}
