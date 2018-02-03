using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayMenuManager : MonoBehaviour {

    public MainMenuManager manager;

    public bool versus = true;

    public GameObject playOptionsMenu;
    public Button versusSelect;
    public Button coopSelect;

    public GameObject howToPlayMenu;
    public Button howToPlayMenuStartOption;

    public Button versusButton;
    public GameObject versusMenu;
    public Button versusMenuStartOption;

    public Button coopButton;
    public GameObject coopMenu;
    public Button coopMenuStartOption;

    public GameObject tutorialMenu;
    public Text tutorialNumberOfPlayers;
    public Button tutorialPlay;

    public List<string> stages;
    public Text selectedStage;
    public Text numberOfPlayers;
    public Text numberOfAIPlayers;

    public List<string> coopStages;
    public Text coopSelectedStage;
    public Text coopNumberOfPlayers;

    public List<string> arenaStages;
    public Text arenaSelectedStage;
    
    public Text selectedCoopGameMode;
    private int activeCoopGameModeIndex = 0;

    private int activeStageIndex;
    private int numPlayers = 1;
    private int numAIPlayers = 0;

    private int coopActiveStageIndex;
    private int arenaActiveStageIndex;

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(RepeatedlyTryToSetStages(0.1f));

        versusMenu.SetActive(false);
        coopMenu.SetActive(false);
        howToPlayMenu.SetActive(false);
	}

    public IEnumerator RepeatedlyTryToSetStages(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        if (manager.gsm == null || manager.gsm.data == null)
        {
            StartCoroutine(RepeatedlyTryToSetStages(time));
        }
        else
        {
            numPlayers = manager.gsm.numberOfPlayers;
            numAIPlayers = manager.gsm.numberOfAIPlayers;
            numberOfPlayers.text = manager.gsm.numberOfPlayers.ToString();
            numberOfAIPlayers.text = manager.gsm.numberOfAIPlayers.ToString();
            coopNumberOfPlayers.text = manager.gsm.numberOfPlayers.ToString();

            stages = manager.gsm.data.versusStages;
            coopStages = manager.gsm.data.coopStages;
            arenaStages = manager.gsm.data.arenaStages;
            SetStagePresets();
        }
    }
	
    public void SetStagePresets()
    {
        if (GsmStagesEqual(stages))
        {
            activeStageIndex = manager.gsm.activeStageIndex;
            selectedStage.text = stages[activeStageIndex];
        }
        else if (GsmStagesEqual(coopStages))
        {
            coopActiveStageIndex = manager.gsm.activeStageIndex;
            coopSelectedStage.text = coopStages[coopActiveStageIndex];
        }
        else if (GsmStagesEqual(arenaStages))
        {
            arenaActiveStageIndex = manager.gsm.activeStageIndex;
            arenaSelectedStage.text = arenaStages[arenaActiveStageIndex];
        }
        else
        {
            activeStageIndex = 0;
            coopActiveStageIndex = 0;

            selectedStage.text = stages[activeStageIndex];
            coopSelectedStage.text = coopStages[coopActiveStageIndex];
        }
    }

	// Update is called once per frame
	void Update () {
		if (manager.shouldRestoreDefaults)
        {
            versusMenu.SetActive(false);
            coopMenu.SetActive(false);
            howToPlayMenu.SetActive(false);
            tutorialMenu.SetActive(false);
            playOptionsMenu.SetActive(true);
            manager.shouldRestoreDefaults = false;
        }
	}

    public void PlayOnline()
    {
        manager.gsm.SwitchToOnlinePlay();
    }

    public void PlayOptionsPressed()
    {
        versusMenu.SetActive(false);
        coopMenu.SetActive(false);
        howToPlayMenu.SetActive(false);
        playOptionsMenu.SetActive(true);

        if (versus)
        {
            versusButton.Select();
        }
        else
        {
            coopButton.Select();
        }
    }

    public void VersusButtonPressed()
    {
        playOptionsMenu.SetActive(false);
        versusMenu.SetActive(true);
        versusMenuStartOption.Select();
        versus = true;

        if (!GsmStagesEqual(stages))
        {
            manager.gsm.activeStageIndex = 0;
            manager.gsm.SetStages(stages);
        }
    }

    public void CoopButtonPressed()
    {
        playOptionsMenu.SetActive(false);
        coopMenu.SetActive(true);
        coopMenuStartOption.Select();
        versus = false;

        if (!GsmStagesEqual(arenaStages))
        {
            manager.gsm.activeStageIndex = 0;
            manager.gsm.SetStages(arenaStages);
        }
    }

    public void HowToPlayPressed()
    {
        playOptionsMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        howToPlayMenu.SetActive(true);
        howToPlayMenuStartOption.Select();
    }

    public void GotItButtonPressed()
    {
        howToPlayMenu.SetActive(false);
        playOptionsMenu.SetActive(true);
        manager.playMenuStartOption.Select();
    }

    public void DontGotItPressed()
    {
        howToPlayMenu.SetActive(false);
        tutorialMenu.SetActive(true);
        tutorialPlay.Select();
    }

    public void PlayTutorialPressed()
    {
        manager.gsm.LoadTutorial();
        Time.timeScale = 1;
    }
    
    public void PlayButtonPressed()
    {
        string nextStage = "Main Menu";

        if (versus)
        {
            nextStage = selectedStage.text;
        }
        else
        {
            if (GameConstants.Unlocks.allCoopGameModes[activeCoopGameModeIndex] == "Arena")
            {
                nextStage = arenaSelectedStage.text;
            }
            else if (GameConstants.Unlocks.allCoopGameModes[activeCoopGameModeIndex] == "Campaign")
            { 
                nextStage = coopSelectedStage.text;
            }
        }
        manager.gsm.LoadStage(nextStage);
        Time.timeScale = 1;
    }

    public void BackGameModePressed()
    {
        activeCoopGameModeIndex--;

        if (activeCoopGameModeIndex < 0)
        {
            activeCoopGameModeIndex = GameConstants.Unlocks.allCoopGameModes.Count - 1;
        }

        selectedCoopGameMode.text = GameConstants.Unlocks.allCoopGameModes[activeCoopGameModeIndex];

        if (GameConstants.Unlocks.allCoopGameModes[activeCoopGameModeIndex] == "Arena")
        {
            manager.gsm.activeStageIndex = arenaActiveStageIndex;
            manager.gsm.SetStages(arenaStages);

            arenaSelectedStage.gameObject.SetActive(true);
            coopSelectedStage.gameObject.SetActive(false);
        }
        else if (GameConstants.Unlocks.allCoopGameModes[activeCoopGameModeIndex] == "Campaign")
        {
            manager.gsm.activeStageIndex = coopActiveStageIndex;
            manager.gsm.SetStages(coopStages);

            arenaSelectedStage.gameObject.SetActive(false);
            coopSelectedStage.gameObject.SetActive(true);
        }
    }

    public void ForwardGameModePressed()
    {
        activeCoopGameModeIndex++;

        if (activeCoopGameModeIndex > GameConstants.Unlocks.allCoopGameModes.Count - 1)
        {
            activeCoopGameModeIndex = 0;
        }

        selectedCoopGameMode.text = GameConstants.Unlocks.allCoopGameModes[activeCoopGameModeIndex];

        if (GameConstants.Unlocks.allCoopGameModes[activeCoopGameModeIndex] == "Arena")
        {
            manager.gsm.activeStageIndex = arenaActiveStageIndex;
            manager.gsm.SetStages(arenaStages);

            arenaSelectedStage.gameObject.SetActive(true);
            coopSelectedStage.gameObject.SetActive(false);
        }
        else if (GameConstants.Unlocks.allCoopGameModes[activeCoopGameModeIndex] == "Campaign")
        {
            manager.gsm.activeStageIndex = coopActiveStageIndex;
            manager.gsm.SetStages(coopStages);

            arenaSelectedStage.gameObject.SetActive(false);
            coopSelectedStage.gameObject.SetActive(true);
        }
    }

    public void BackStagePressed()
    {
        if (versus)
        {
            activeStageIndex--;
            if (activeStageIndex < 0)
            {
                activeStageIndex = stages.Count - 1;
            }

            selectedStage.text = stages[activeStageIndex];
            manager.gsm.activeStageIndex = activeStageIndex;
        }

        else
        {
            if (GameConstants.Unlocks.allCoopGameModes[activeCoopGameModeIndex] == "Arena")
            {
                arenaActiveStageIndex--;
                if (arenaActiveStageIndex < 0)
                {
                    arenaActiveStageIndex = arenaStages.Count - 1;
                }

                arenaSelectedStage.text = arenaStages[arenaActiveStageIndex];
                manager.gsm.activeStageIndex = arenaActiveStageIndex;
            }
            else if (GameConstants.Unlocks.allCoopGameModes[activeCoopGameModeIndex] == "Campaign")
            {
                coopActiveStageIndex--;
                if (coopActiveStageIndex < 0)
                {
                    coopActiveStageIndex = coopStages.Count - 1;
                }

                coopSelectedStage.text = coopStages[coopActiveStageIndex];
                manager.gsm.activeStageIndex = coopActiveStageIndex;
            }
        }
    }

    public void ForwardStagePressed()
    {
        if (versus) {
            activeStageIndex++;
            if (activeStageIndex > stages.Count - 1)
            {
                activeStageIndex = 0;
            }
            
            selectedStage.text = stages[activeStageIndex];
            manager.gsm.activeStageIndex = activeStageIndex;
        }

        else
        {
            if (GameConstants.Unlocks.allCoopGameModes[activeCoopGameModeIndex] == "Arena")
            {
                arenaActiveStageIndex++;
                if (arenaActiveStageIndex > arenaStages.Count - 1)
                {
                    arenaActiveStageIndex = 0;
                }

                arenaSelectedStage.text = arenaStages[arenaActiveStageIndex];
                manager.gsm.activeStageIndex = arenaActiveStageIndex;
            }
            else if (GameConstants.Unlocks.allCoopGameModes[activeCoopGameModeIndex] == "Campaign")
            {
                coopActiveStageIndex++;
                if (coopActiveStageIndex > coopStages.Count - 1)
                {
                    coopActiveStageIndex = 0;
                }

                coopSelectedStage.text = coopStages[coopActiveStageIndex];
                manager.gsm.activeStageIndex = coopActiveStageIndex;
            }
        }
    }

    public void IncNumPlayers()
    {
        numPlayers++;
        if (numPlayers > 4)
        {
            numPlayers = 1;
        }

        if (numPlayers + numAIPlayers > 4)
        {
            numAIPlayers = 4 - numPlayers;
            numberOfAIPlayers.text = numAIPlayers.ToString();
            manager.gsm.numberOfAIPlayers = numAIPlayers;
        }

        numberOfPlayers.text = numPlayers.ToString();
        coopNumberOfPlayers.text = numPlayers.ToString();
        tutorialNumberOfPlayers.text = numPlayers.ToString();
        manager.gsm.numberOfPlayers = numPlayers;
    }

    public void DecNumPlayers()
    {
        numPlayers--;
        if (numPlayers < 1)
        {
            numPlayers = 4;
        }

        if (numPlayers + numAIPlayers > 4)
        {
            numAIPlayers = 4 - numPlayers;
            numberOfAIPlayers.text = numAIPlayers.ToString();
            manager.gsm.numberOfAIPlayers = numAIPlayers;
        }

        numberOfPlayers.text = numPlayers.ToString();
        coopNumberOfPlayers.text = numPlayers.ToString();
        tutorialNumberOfPlayers.text = numPlayers.ToString();
        manager.gsm.numberOfPlayers = numPlayers;
    }

    public void IncNumAIPlayers()
    {
        numAIPlayers++;
        if (numAIPlayers > 4 - numPlayers)
        {
            numAIPlayers = 0;
        }

        numberOfAIPlayers.text = numAIPlayers.ToString();
        manager.gsm.numberOfAIPlayers = numAIPlayers;
    }

    public void DecNumAIPlayers()
    {
        numAIPlayers--;
        if (numAIPlayers < 0)
        {
            numAIPlayers = 4 - numPlayers;
        }

        numberOfAIPlayers.text = numAIPlayers.ToString();
        manager.gsm.numberOfAIPlayers = numAIPlayers;
    }

    private bool GsmStagesEqual(List<string> compare)
    {
        return manager.gsm.stages.Count == compare.Count; // Lazy, fix later
    }
}
