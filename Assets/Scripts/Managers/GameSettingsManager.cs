using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettingsManager : MonoBehaviour {

    public bool active = false;

    // If players are within the play zone, they will not be killed.
    public Vector3 inBoundsMin = new Vector3(-100, -100, -100);
    public Vector3 inBoundsMax = new Vector3(100, 100, 100);

    public Man playerPrefab;

    public string currentStage = "Main Menu";
    public List<string> stages;
    public int activeStageIndex;

    public Transform player1Spawn;
    public Transform player2Spawn;
    public Transform player3Spawn;
    public Transform player4Spawn;

    public int numberOfPlayers = 1;

    public string player1Color;
    public string player1Weapon;
    public string player1Hat;
    public string player1Misc;
    public string player1Skin;

    public string player2Color;
    public string player2Weapon;
    public string player2Hat;
    public string player2Misc;
    public string player2Skin;

    public string player3Color;
    public string player3Weapon;
    public string player3Hat;
    public string player3Misc;
    public string player3Skin;

    public string player4Color;
    public string player4Weapon;
    public string player4Hat;
    public string player4Misc;
    public string player4Skin;

    public Settings settings = new Settings();

    public int roundNumber = 1;

    public int[] wins;

    public MusicManager music;

    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(this);
        if (music == null)
        {
            music = this.GetComponent<MusicManager>();
        }
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
        if (numberOfPlayers >= 1)
        {
            // spawn player 1
            Man player1 = Instantiate(playerPrefab) as Man;
            player1.transform.position = player1Spawn.position;
            player1.transform.rotation = player1Spawn.rotation;

            player1.playerNumber = 1;
            player1.color = player1Color;
            player1.weapon = player1Weapon;
            player1.hat = player1Hat;
            player1.misc = player1Misc;
            player1.skin = player1Skin;
}
        if (numberOfPlayers >= 2)
        {
            // spawn player 2
            Man player2 = Instantiate(playerPrefab) as Man;
            player2.transform.position = player2Spawn.position;
            player2.transform.rotation = player2Spawn.rotation;

            player2.playerNumber = 2;
            player2.color = player2Color;
            player2.weapon = player2Weapon;
            player2.hat = player2Hat;
            player2.misc = player2Misc;
            player2.skin = player2Skin;
        }
        if (numberOfPlayers >= 3)
        {
            // spawn player 3
            Man player3 = Instantiate(playerPrefab) as Man;
            player3.transform.position = player3Spawn.position;
            player3.transform.rotation = player3Spawn.rotation;

            player3.playerNumber = 3;
            player3.color = player3Color;
            player3.weapon = player3Weapon;
            player3.hat = player3Hat;
            player3.misc = player3Misc;
            player3.skin = player3Skin;
        }
        if (numberOfPlayers >= 4)
        {
            // spawn player 4
            Man player4 = Instantiate(playerPrefab) as Man;
            player4.transform.position = player4Spawn.position;
            player4.transform.rotation = player4Spawn.rotation;

            player4.playerNumber = 4;
            player4.color = player4Color;
            player4.weapon = player4Weapon;
            player4.hat = player4Hat;
            player4.misc = player4Misc;
            player4.skin = player4Skin;
        }
    }

    public void LoadNextStage()
    {
        int index = 0;
        if (settings.randomStateSelect)
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
        if (settings.randomStateSelect)
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

    public void ToggleMusicOn()
    {
        settings.musicOn = !settings.musicOn;
        if (settings.musicOn)
        {
            music.Mute(false);
        }
        else
        {
            music.Mute(true);
        }
    }

    public class Settings
    {
        public bool colorizeHealthBars = true;
        public bool showHealthValues = true;

        public int roundsPerStage = 3;
        public bool randomStateSelect = false;

        public bool musicOn = true;
        public float musicVolume = 0.6f;
    }
}
