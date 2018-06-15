using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

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

    public int numberOfPlayers = 1;
    public int numberOfAIPlayers = 0;

    public List<string> playerColors = new List<string>(new string[4]);
    public List<string> playerWeapons = new List<string>(new string[4]);
    public List<string> playerHats = new List<string>(new string[4]);
    public List<string> playerMisc = new List<string>(new string[4]);
    public List<string> playerSkins = new List<string>(new string[4]);

    public Settings settings = new Settings();

    public int roundNumber = 1;
    public int[] wins;

    public MusicManager music;

    public GameData data;

    public Steam steam;

    public bool inDemo = false;

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

        //settings.graphicsQuality = QualitySettings.names.Length - 1; // Can't set this as a default value in settings...

        if (music == null)
        {
            music = this.GetComponent<MusicManager>();
        }

        if (data == null)
        {
            data = this.GetComponent<GameData>();
        }

        if (steam == null)
        {
            steam = this.GetComponent<Steam>();
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

            data.arenaStages = GameConstants.Unlocks.startingArenaStages;
            data.arenaStages.Sort();

            data.Save();
        }

        settings.SetupData(this);
    }

    // Update is called once per frame
    void Update ()
    {
        
	}

    public void SwitchToOnlinePlay()
    {
        // other stuff
        
        SceneManager.LoadScene("Online Lobby");
    }

    public void SwitchToLocalPlay()
    {
        NetworkManager networkManager = FindObjectOfType<NetworkManager>();
        if (networkManager != null)
        {
            Destroy(networkManager);
        }

        SceneManager.LoadScene("Main Menu");
    }

    public void SetCursor(bool active)
    {
        Cursor.visible = active;

        if (active)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
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
        Man[] men = FindObjectsOfType<Man>();
        foreach(Man man in men)
        {
            Destroy(man.gameObject);
        }

        int temp = numberOfPlayers;
        int tempAI = numberOfAIPlayers;
        numberOfPlayers = 4;
        numberOfAIPlayers = 0;
        SpawnPlayers();
        numberOfPlayers = temp;
        numberOfAIPlayers = tempAI;
    }

    public void SpawnSingleMenuPlayer(int i)
    {
        bool playerExists = false;

        Man[] men = FindObjectsOfType<Man>();
        foreach (Man man in men)
        {
            if (man.playerNumber == i + 1)
            {
                man.ResetAllCosmetics();

                man.SetSkin(playerSkins[i]);
                man.SetColor(playerColors[i]);
                man.SetWeapon(playerWeapons[i]);
                man.SetMisc(playerMisc[i]);
                man.SetHat(playerHats[i]);

                playerExists = true;
            }
        }

        if (!playerExists)
        {
            SpawnSinglePlayer(i);
        }
    }

    public void SpawnPlayers()
    {
        for (int i = 0; i < numberOfPlayers + numberOfAIPlayers; i++)
        {
            SpawnSinglePlayer(i);
        }
    }

    public void SpawnSinglePlayer(int i)
    {
        Man player;
        if (numberOfPlayers > i)
        {
            player = Instantiate(playerPrefab) as Man;
            if (i == Input.GetJoystickNames().Length) // player with number = number of controllers + 1 will use keyboard controls
            {
                player.usesKeyboardControls = true;
            }
        }
        else
        {
            player = Instantiate(AIPlayerPrefab) as Man;
        }

        player.transform.position = playerSpawns[i].position;
        player.transform.rotation = playerSpawns[i].rotation;

        player.playerNumber = i + 1;
        player.color = playerColors[i];
        player.weapon = playerWeapons[i];
        player.hat = playerHats[i];
        player.misc = playerMisc[i];
        player.skin = playerSkins[i];

        if (NetworkServer.active)
        {
            NetworkServer.Spawn(player.gameObject);
        }
    }

    public void RespawnPlayers(int[] playerNumbers, float healthOverride = 1000)
    {
        CameraFollow camera = FindObjectOfType<CameraFollow>();
        for (int i = 0; i < playerNumbers.Length; i++)
        {
            int playerNumberIndex = playerNumbers[i] - 1; // for index uses

            Man player;
            if (numberOfPlayers > playerNumberIndex)
            {
                player = Instantiate(playerPrefab) as Man;
                if (i == Input.GetJoystickNames().Length) // player with number = number of controllers + 1 will use keyboard controls
                {
                    player.usesKeyboardControls = true;
                }
            }
            else
            {
                player = Instantiate(AIPlayerPrefab) as Man;
            }

            player.transform.position = playerSpawns[playerNumberIndex].position;
            player.transform.rotation = playerSpawns[playerNumberIndex].rotation;

            player.playerNumber = playerNumbers[i];
            player.color = playerColors[playerNumberIndex];
            player.weapon = playerWeapons[playerNumberIndex];
            player.hat = playerHats[playerNumberIndex];
            player.misc = playerMisc[playerNumberIndex];
            player.skin = playerSkins[playerNumberIndex];

            if (healthOverride != 1000)
            {
                player.health = healthOverride;
            }

            if (NetworkServer.active)
            {
                NetworkServer.Spawn(player.gameObject);
            }

            if (camera != null)
            {
                camera.Add(player);
            }
        }
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

        if (numberOfPlayers == 4)
        {
            // Achievement: Friends Make Everything Better
            steam.UnlockAchievement(GameConstants.AchievementId.FRIENDS_MAKE_EVERYTHING_BETTER);
        }

        for (int i = 0; i < playerWeapons.Count && i < numberOfPlayers; i++) // To check to make sure it was players and not AIs using them
        {
            // Achievement: Well Rounded
            data.AddWeaponUsed(playerWeapons[i]);
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

    public void LoadCurrentStage()
    {
        LoadStage(currentStage);
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
            if (name.Contains("Level"))
            {
                name = name.Substring(0, name.Length - 6);
            }
            activeStageIndex = stages.IndexOf(name);

            currentStage = stages[activeStageIndex];
            SetCursor(false);
        }
        else
        {
            if (inDemo)
            {
                currentStage = "Main Menu Demo";
            }
            else
            {
                currentStage = "Main Menu";
            }

            SetCursor(true);
        }
        
        Debug.Log("Load stage: " + currentStage);
        SceneManager.LoadScene(currentStage, LoadSceneMode.Single);

        if (songChange)
        {
            music.SetSong(currentStage);
        }
    }

    public void LoadTutorial()
    {
        currentStage = "Tutorial";
        SetCursor(false);
        SceneManager.LoadScene(currentStage, LoadSceneMode.Single);
        music.SetSong(currentStage);
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
        public bool showBlood = true;

        public int roundsPerStage = 3;
        public bool randomStageSelect = false;

        public bool windowed = false;
        public int resolution;
        public int graphicsQuality;

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

            gsm.SetMusicOn(musicOn);
            gsm.SetMusicVolume(musicVolume);
        }

        public void MapFromData()
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
        }

        public void UpdateData()
        {
            Debug.Log("Updating data to default settings");
            data.colorizeHealthBars = colorizeHealthBars;
            data.showHealthValues = showHealthValues;
            data.showBlood = showBlood;

            data.roundsPerStage = roundsPerStage;
            data.randomStageSelect = randomStageSelect;

            data.windowed = windowed;
            data.resolution = new List<Resolution>(Screen.resolutions).IndexOf(Screen.currentResolution);
            data.graphicsQuality = QualitySettings.names.Length - 1;
            resolution = data.resolution;
            graphicsQuality = data.graphicsQuality;

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

        public void SetShowBlood(bool value)
        {
            showBlood = value;
            data.showBlood = value;

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

        public void SetWindowed(bool value)
        {
            windowed = value;
            data.windowed = value;

            data.Save();
        }

        public void SetResolution(int value)
        {
            resolution = value;
            data.resolution = value;

            data.Save();
        }

        public void SetGraphicsQuality(int value)
        {
            graphicsQuality = value;
            data.graphicsQuality = value;

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
