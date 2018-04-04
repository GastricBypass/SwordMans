using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Steamworks;

public class SaveManager : MonoBehaviour
{
    public static void SaveCosmetics(string filename, GameData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + filename);

        SwordMansCosmeticData serializableData = new SwordMansCosmeticData(data);
        formatter.Serialize(file, serializableData);

        file.Close();
    }

    public static void LoadCosmetics(string filename, GameData data)
    {
        string filePath = Application.persistentDataPath + "/" + filename;

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);

            SwordMansCosmeticData serializableData = (SwordMansCosmeticData)formatter.Deserialize(file);
            serializableData.UpdateGameData(data);

            file.Close();

            data.hasSavedCosmetics = true;

            // If the player is missing something that should be unlocked by default, unlock it.
            foreach (string hat in GameConstants.Unlocks.startingHats)
            {
                if (!data.hats.Contains(hat))
                {
                    data.UnlockHat(hat);
                }
            }
            foreach (string misc in GameConstants.Unlocks.startingMisc)
            {
                if (!data.misc.Contains(misc))
                {
                    data.UnlockMisc(misc);
                }
            }
        }

        else
        {
            data.hasSavedCosmetics = false;
        }
    }

    public static void SaveSettings(string filename, GameData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + filename);

        SwordMansSettingsData serializableData = new SwordMansSettingsData(data);
        formatter.Serialize(file, serializableData);

        file.Close();
    }

    public static void LoadSettings(string filename, GameData data)
    {
        string filePath = Application.persistentDataPath + "/" + filename;

        Debug.Log("File: " + filePath + " Exists = " + File.Exists(filePath));

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);

            SwordMansSettingsData serializableData = (SwordMansSettingsData)formatter.Deserialize(file);
            serializableData.UpdateGameData(data);

            file.Close();

            data.hasSavedSettings = true;
        }

        else
        {
            data.hasSavedSettings = false;
        }
    }

    [Serializable]
    private class SwordMansCosmeticData
    {
        private float gold;
        private List<string> unlockedHats;
        private List<string> unlockedMisc;
        private List<string> unlockedVersusStages;
        private List<string> unlockedCoopStages;
        private List<string> unlockedArenaStages;
        private List<string> currentShopItems;

        private DateTime lastPlayDate;

        public SwordMansCosmeticData(GameData data)
        {
            Map(data);
        }

        public void Map(GameData data)
        {
            gold = data.gold;
            unlockedHats = data.hats;
            unlockedMisc = data.misc;
            unlockedVersusStages = data.versusStages;
            unlockedCoopStages = data.coopStages;
            unlockedArenaStages = data.arenaStages;
            
            currentShopItems = data.shopItems;

            // This whole mess was to reset the shop at midnight, but I think just storing the lastPlayDate on the data should do it.
            if (data.lastPlayDate.Date != DateTime.Now.Date) // reset shop items at midnight local time. This should only take effect after reloading the game
            {
                //data.shopItems = new List<string>(); // To reset shop immediately at midnight local time, otherwise it resets afterwards
                data.lastPlayDate = DateTime.Now;
            }

            lastPlayDate = data.lastPlayDate; // Will be the time you started playing, not the time you stopped.
        }

        public void UpdateGameData(GameData data)
        {
            data.gold = gold;
            data.hats = unlockedHats;
            data.misc = unlockedMisc;
            data.versusStages = unlockedVersusStages;
            data.coopStages = unlockedCoopStages;
            data.arenaStages = unlockedArenaStages;

            if (lastPlayDate.Date == DateTime.Now.Date) // Resets shop if you stop playing and then come back on a different day
            {
                data.shopItems = currentShopItems;
            }
            else
            {
                data.shopItems = new List<string>();
            }
        }
    }

    [Serializable]
    private class SwordMansSettingsData
    {
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

        public SwordMansSettingsData(GameData data)
        {
            Map(data);
        }

        public void Map(GameData data)
        {
            colorizeHealthBars = data.colorizeHealthBars;
            showHealthValues = data.showHealthValues;
            showBlood = data.showBlood;

            roundsPerStage = data.roundsPerStage;
            randomStageSelect = data.randomStageSelect;

            windowed = data.windowed;
            resolution = data.resolution;
            graphicsQuality = data.graphicsQuality;

            musicOn = data.musicOn;
            musicVolume = data.musicVolume;
            effectsVolume = data.effectsVolume;

            weaponsUsed = data.weaponsUsed;
        }

        public void UpdateGameData(GameData data)
        {
            data.colorizeHealthBars = colorizeHealthBars;
            data.showHealthValues = showHealthValues;
            data.showBlood = showBlood;

            data.roundsPerStage = roundsPerStage;
            data.randomStageSelect = randomStageSelect;

            data.windowed = windowed;
            data.resolution = resolution;
            data.graphicsQuality = graphicsQuality;

            data.musicOn = musicOn;
            data.musicVolume = musicVolume;
            data.effectsVolume = effectsVolume;

            data.weaponsUsed = weaponsUsed;
        }
    }
}


