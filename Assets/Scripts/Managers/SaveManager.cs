using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void Save(string filename, GameData data)
    {
        Debug.Log("SaveManager saving all of the game data to the file: " + Application.persistentDataPath + "/" + filename);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + filename);
        Debug.Log("File created");
        SwordMansData serializableData = new SwordMansData(data);
        Debug.Log("Serializable data created");
        formatter.Serialize(file, serializableData);
        Debug.Log("Data serialized and saved in a file");
        file.Close();
    }

    public static void Load(string filename, GameData data)
    {
        string filePath = Application.persistentDataPath + "/" + filename;

        Debug.Log("File: " + filePath + " Exists = " + File.Exists(filePath));

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);

            SwordMansData serializableData = (SwordMansData)formatter.Deserialize(file); // load from file
            serializableData.UpdateGameData(data);

            file.Close();
        }
    }

    [Serializable]
    private class SwordMansData
    {
        private List<string> unlockedHats;
        private List<string> unlockedMisc;
        private List<string> unlockedVersusStages;
        private List<string> unlockedCoopStages;

        public SwordMansData(GameData data)
        {
            Map(data);
        }

        public void Map(GameData data)
        {
            unlockedHats = data.hats;
            unlockedMisc = data.misc;
            unlockedVersusStages = data.versusStages;
            unlockedCoopStages = data.coopStages;
        }

        public void UpdateGameData(GameData data)
        {
            data.hats = unlockedHats;
            data.misc = unlockedMisc;
            data.versusStages = unlockedVersusStages;
            data.coopStages = unlockedCoopStages;
         }
    }
}


