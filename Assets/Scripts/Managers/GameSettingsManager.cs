using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettingsManager : MonoBehaviour {

    public bool active = false;

    public float chanceToUnlockItemsEachRound = 0.1f;

    // If players are within the play zone, they will not be killed.
    public Vector3 inBoundsMin = new Vector3(-100, -100, -100);
    public Vector3 inBoundsMax = new Vector3(100, 100, 100);

    public Man playerPrefab;
    public Man AIPlayerPrefab;

    public string currentStage = "Main Menu";
    public List<string> stages;
    public int activeStageIndex;

    public List<Transform> playerSpawns = new List<Transform>(new Transform[4]);

    //public Transform player1Spawn;
    //public Transform player2Spawn;
    //public Transform player3Spawn;
    //public Transform player4Spawn;

    public int numberOfPlayers = 1;
    public int numberOfAIPlayers = 0;

    public List<string> playerColors = new List<string>(new string[4]);
    public List<string> playerWeapons = new List<string>(new string[4]);
    public List<string> playerHats = new List<string>(new string[4]);
    public List<string> playerMisc = new List<string>(new string[4]);
    public List<string> playerSkins = new List<string>(new string[4]);

    //public string player1Color;
    //public string player1Weapon;
    //public string player1Hat;
    //public string player1Misc;
    //public string player1Skin;

    //public string player2Color;
    //public string player2Weapon;
    //public string player2Hat;
    //public string player2Misc;
    //public string player2Skin;

    //public string player3Color;
    //public string player3Weapon;
    //public string player3Hat;
    //public string player3Misc;
    //public string player3Skin;

    //public string player4Color;
    //public string player4Weapon;
    //public string player4Hat;
    //public string player4Misc;
    //public string player4Skin;

    public Settings settings = new Settings();

    public int roundNumber = 1;
    public int[] wins;

    public MusicManager music;

    public GameData data;

    private GameSettingsManager singletonGsm;

    // Use this for initialization
    void Start ()
    {
        if (singletonGsm == null)
        {
            singletonGsm = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (music == null)
        {
            music = this.GetComponent<MusicManager>();
        }

        if (data == null)
        {
            data = this.GetComponent<GameData>();
        }
        SetupGameData();

        music.Play();

        //Resources.LoadAll("Sounds"); // this causes loading the main menu to take longer. Also the first sound loaded takes just as long

        // This might help a little, but there's still a delay when the first sound loads.
        //AudioClip clip = (AudioClip)Resources.Load("Sounds/metalOnMetal1", typeof(AudioClip));
        //AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        //audioSource.volume = 0;
        //audioSource.clip = clip;
        //audioSource.Play();
	}

    private void SetupGameData()
    {
        data.Load();

        if (!data.hasSavedCosmetics)
        {
            data.hats = GameConstants.Unlocks.startingHats;
            data.hats.Sort();

            data.misc = GameConstants.Unlocks.startingMisc;
            data.misc.Sort();

            data.versusStages = GameConstants.Unlocks.startingVersusStages;
            data.versusStages.Sort();

            data.coopStages = GameConstants.Unlocks.startingCoopStages;
            data.coopStages.Sort();

            data.Save();
        }

        settings.SetupData(this);
    }

    // Update is called once per frame
    void Update ()
    {
        
	}

    public void SetStages(List<string> newStages)
    {
        stages = newStages;
    }

    public void SetCurrentStage(int index)
    {
        //activeStageIndex = index;
        //currentStage = stages[activeStageIndex];
    }

    public void SpawnMenuPlayers()
    {
        Debug.Log("Spawn Menu Players");
        Man[] men = FindObjectsOfType<Man>();
        foreach(Man man in men)
        {
            Destroy(man.gameObject);
        }

        int temp = numberOfPlayers;
        numberOfPlayers = 4;
        SpawnPlayers();
        numberOfPlayers = temp;

        men = FindObjectsOfType<Man>();
        foreach (Man man in men)
        {
            man.playerNumber = 0;
        }
    }

    public void SpawnPlayers()
    {
        //if (playerSpawns.Count == 0)
        //{
        //    // sucks...
        //    return;
        //}

        Debug.Log("Spawn Players");
        for (int i = 0; i < numberOfPlayers + numberOfAIPlayers; i++)
        {
            //if (playerSpawns[i] == null)
            //{
            //    //sucks
            //    continue;
            //}

            Man player;
            if (numberOfPlayers >= i)
            {
                player = Instantiate(playerPrefab) as Man;
            }
            else
            {
                player = Instantiate(AIPlayerPrefab) as Man;
            }

            Debug.Log(playerSpawns.Count + " player spawns. At " + i + ". This one: " + playerSpawns[i]);
            player.transform.position = playerSpawns[i].position;
            player.transform.rotation = playerSpawns[i].rotation;

            player.playerNumber = i + 1;
            player.color = playerColors[i];
            player.weapon = playerWeapons[i];
            player.hat = playerHats[i];
            player.misc = playerMisc[i];
            player.skin = playerSkins[i];
        }

//        if (numberOfPlayers >= 1)
//        {
//            // spawn player 1
//            Man player1 = Instantiate(playerPrefab) as Man;
//            player1.transform.position = player1Spawn.position;
//            player1.transform.rotation = player1Spawn.rotation;

//            player1.playerNumber = 1;
//            player1.color = player1Color;
//            player1.weapon = player1Weapon;
//            player1.hat = player1Hat;
//            player1.misc = player1Misc;
//            player1.skin = player1Skin;
//}
//        if (numberOfPlayers + numberOfAIPlayers >= 2)
//        {
//            // spawn player 2
//            Man player2;
//            if (numberOfPlayers >= 2)
//            {
//                player2 = Instantiate(playerPrefab) as Man;
//            }
//            else
//            {
//                player2 = Instantiate(AIPlayerPrefab) as Man;
//            }

//            player2.transform.position = player2Spawn.position;
//            player2.transform.rotation = player2Spawn.rotation;

//            player2.playerNumber = 2;
//            player2.color = player2Color;
//            player2.weapon = player2Weapon;
//            player2.hat = player2Hat;
//            player2.misc = player2Misc;
//            player2.skin = player2Skin;
//        }
//        if (numberOfPlayers + numberOfAIPlayers >= 3)
//        {
//            // spawn player 3
//            Man player3;
//            if (numberOfPlayers >= 3)
//            {
//                player3 = Instantiate(playerPrefab) as Man;
//            }
//            else
//            {
//                player3 = Instantiate(AIPlayerPrefab) as Man;
//            }

//            player3.transform.position = player3Spawn.position;
//            player3.transform.rotation = player3Spawn.rotation;

//            player3.playerNumber = 3;
//            player3.color = player3Color;
//            player3.weapon = player3Weapon;
//            player3.hat = player3Hat;
//            player3.misc = player3Misc;
//            player3.skin = player3Skin;
//        }
//        if (numberOfPlayers + numberOfAIPlayers >= 4)
//        {
//            // spawn player 4
//            Man player4;
//            if (numberOfPlayers >= 4)
//            {
//                player4 = Instantiate(playerPrefab) as Man;
//            }
//            else
//            {
//                player4 = Instantiate(AIPlayerPrefab) as Man;
//            }

//            player4.transform.position = player4Spawn.position;
//            player4.transform.rotation = player4Spawn.rotation;

//            player4.playerNumber = 4;
//            player4.color = player4Color;
//            player4.weapon = player4Weapon;
//            player4.hat = player4Hat;
//            player4.misc = player4Misc;
//            player4.skin = player4Skin;
//        }
    }

    public void LoadNextStage()
    {
        int index = 0;
        if (settings.randomStageSelect)
        {
            index = Random.Range(0, stages.Count);
            roundNumber = 1;
            wins = new int[settings.roundsPerStage];
        }
        else
        {
            index = activeStageIndex + 1;
            if (index > stages.Count - 1)
            {
                index = 0;
            }
        }
        activeStageIndex = index;
        LoadStage(stages[index]);
    }

    public void LoadPrevStage()
    {
        int index = 0;
        if (settings.randomStageSelect)
        {
            index = Random.Range(0, stages.Count);
            roundNumber = 1;
            wins = new int[settings.roundsPerStage];
        }
        else
        {
            index = activeStageIndex - 1;
            if (index < 0)
            {
                index = stages.Count - 1;
            }
        }
        activeStageIndex = index;
        LoadStage(stages[index]);
    }

    public void NextRound()
    {
        roundNumber++;

        if (roundNumber > settings.roundsPerStage)
        {
            roundNumber = 1;
            LoadNextStage();
        }
        else
        {
            LoadStage(currentStage);
        }
    }

    public void LoadStage(string name)
    {
        bool songChange = false;
        if (name != currentStage)
        {
            roundNumber = 1;
            wins = new int[settings.roundsPerStage];
            songChange = true;
        }
        if (name != "Main Menu")
        {
            activeStageIndex = stages.IndexOf(name);
            currentStage = stages[activeStageIndex];
        }
        else
        {
            currentStage = "Main Menu";
        }
        
        Debug.Log("Load stage: " + currentStage);
        SceneManager.LoadScene(currentStage, LoadSceneMode.Single);

        if (songChange)
        {
            music.SetSong(currentStage);
        }
    }

    public void LoadMainMenu()
    {
        LoadStage("Main Menu");
    }

    public void SetMusicOn(bool musicOn)
    {
        settings.SetMusicOn(musicOn);

        if (settings.musicOn)
        {
            music.Mute(false);
        }
        else
        {
            music.Mute(true);
        }
    }

    public void SetMusicVolume(float volume)
    {
        settings.SetMusicVolume(volume);

        if (music != null)
        {
            music.SetVolume(volume);
        }
    }

    public class Settings
    {
        public bool colorizeHealthBars = true;
        public bool showHealthValues = true;

        public int roundsPerStage = 3;
        public bool randomStageSelect = false;

        public bool musicOn = true;
        public float musicVolume = 0.6f;
        public float effectsVolume = 1f;
        
        private GameData data;

        public void SetupData(GameSettingsManager gsm)
        {
            this.data = gsm.data;

            if (data.hasSavedSettings)
            {
                MapFromData();
            }
            else
            {
                UpdateData();
            }

            Debug.Log("Creating game settings data successful\nThe data is as follows:" +
            "\ncolorizeHealthBars: " + data.colorizeHealthBars +
            "\nshowHealthValues: " + data.showHealthValues +
            "\nroundsPerStage: " + data.roundsPerStage +
            "\nrandomStageSelect: " + data.randomStageSelect +
            "\nmusicOn: " + data.musicOn +
            "\nmusicVolume: " + data.musicVolume +
            "\neffectsVolume: " + data.effectsVolume);

            gsm.SetMusicOn(musicOn);
            gsm.SetMusicVolume(musicVolume);
        }

        public void MapFromData()
        {
            Debug.Log("Mapping settings from data");
            colorizeHealthBars = data.colorizeHealthBars;
            showHealthValues = data.showHealthValues;

            roundsPerStage = data.roundsPerStage;
            randomStageSelect = data.randomStageSelect;

            musicOn = data.musicOn;
            musicVolume = data.musicVolume;
            effectsVolume = data.effectsVolume;
        }

        public void UpdateData()
        {
            Debug.Log("Updating data to default settings");
            data.colorizeHealthBars = colorizeHealthBars;
            data.showHealthValues = showHealthValues;

            data.roundsPerStage = roundsPerStage;
            data.randomStageSelect = randomStageSelect;

            data.musicOn = musicOn;
            data.musicVolume = musicVolume;
            data.effectsVolume = effectsVolume;

            data.Save();
        }

        public void SetColorizeHealthBars(bool value)
        {
            colorizeHealthBars = value;
            data.colorizeHealthBars = value;

            data.Save();
        }

        public void SetShowHealthValues(bool value)
        {
            showHealthValues = value;
            data.showHealthValues = value;

            data.Save();
        }

        public void SetRoundsPerStage(int value)
        {
            roundsPerStage = value;
            data.roundsPerStage = value;

            data.Save();
        }

        public void SetRandomStageSelect(bool value)
        {
            randomStageSelect = value;
            data.randomStageSelect = value;

            data.Save();
        }

        public void SetMusicOn(bool value)
        {
            musicOn = value;
            data.musicOn = value;

            data.Save();
        }

        public void SetMusicVolume(float value)
        {
            musicVolume = value;
            data.musicVolume = value;

            data.Save();
        }

        public void SetEffectsVolume(float value)
        {
            effectsVolume = value;
            data.effectsVolume = value;

            data.Save();
        }
    }
}
