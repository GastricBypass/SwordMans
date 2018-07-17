using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayMenuManager : MonoBehaviour {

    public MainMenuManager manager;

    public bool versus = true;

    public GameObject playOptionsMenu;

    public GameObject howToPlayMenu;
    public Button howToPlayMenuStartOption;

    public Button versusButton;
    public GameObject versusMenu;
    public Button versusMenuStartOption;
    public Image versusStageImage;

    // versus options
    public GameObject versusOptionsMenu;
    public Button versusOptionsMenuStartOption;
    // random stage
    public Button toggleRandomStageSelect;
    private GameObject randomStageSelectSelected;
    // num rounds
    public int maxNumberOfRounds = 5;
    public Text numberOfRounds;
    // num rounds
    public int maxNumberOfLives = 5;
    public Text numberOfLives;
    // num rounds
    public int maxTimePerRound = 600; // seconds
    public Text timePerRound;

    public Button coopButton;
    public GameObject coopMenu;
    public Button coopMenuStartOption;
    public Image coopStageImage;

    public Sprite defaultStageImage;

    public GameObject tutorialMenu;
    public Text tutorialNumberOfPlayers;
    public Button tutorialPlay;

    public List<string> stages;
    public Text selectedStage;
    public Text numberOfPlayers;
    public Text numberOfAIPlayers;

    public List<string> coopStages;
    public Text coopCurrentChapter;
    public Text coopSelectedStage;
    public Text coopNumberOfPlayers;

    private int activeCampaignChapterNumber = 1;
    private int numberOfAvailableChapters;

    public List<string> arenaStages;
    public Text arenaSelectedStage;
    
    public Text selectedCoopGameMode;
    private int activeCoopGameModeIndex = 0;

    private int activeStageIndex;
    private int numPlayers = 1;
    private int numAIPlayers = 0;

    private int coopActiveStageIndex;
    private int arenaActiveStageIndex;
    
    private int currentChapterStartingIndex;
    private int currentChapterEndingIndex;

    // Use this for initialization
    void Start ()
    {
        // versus options
        randomStageSelectSelected = toggleRandomStageSelect.transform.Find("Selected").gameObject;

        StartCoroutine(RepeatedlyTryToSetStages(0.1f));

        versusMenu.SetActive(false);
        versusOptionsMenu.SetActive(false);
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
            SetStages();
            SetSettings();
        }
    }

    public void SetStages()
    {
        numPlayers = manager.gsm.numberOfPlayers;
        numAIPlayers = manager.gsm.numberOfAIPlayers;
        numberOfPlayers.text = manager.gsm.numberOfPlayers.ToString();
        numberOfAIPlayers.text = manager.gsm.numberOfAIPlayers.ToString();
        coopNumberOfPlayers.text = manager.gsm.numberOfPlayers.ToString();

        stages = manager.gsm.data.versusStages;
        coopStages = manager.gsm.data.coopStages;
        arenaStages = manager.gsm.data.arenaStages;

        SetNumberOfAvailableChapters();

        SetStagePresets();
    }

    public void SetSettings()
    {
        randomStageSelectSelected.SetActive(manager.gsm.settings.randomStageSelect);
        numberOfRounds.text = manager.gsm.settings.roundsPerStage.ToString();
        numberOfLives.text = manager.gsm.settings.livesPerRound.ToString();
        timePerRound.text = ParseTime(manager.gsm.settings.timePerRound);
    }
	
    public void SetStagePresets()
    {
        if (GsmStagesEqual(stages)) // versus
        {
            activeStageIndex = manager.gsm.activeStageIndex;
            selectedStage.text = stages[activeStageIndex];
        }
        else if (manager.gsm.stages.Count > 0 && manager.gsm.stages[0].Contains("Ch")) // campaign
        {
            coopActiveStageIndex = manager.gsm.activeStageIndex;
            coopSelectedStage.text = coopStages[coopActiveStageIndex];

            string chapterNumberString = manager.gsm.stages[0].Replace("Ch", "");
            chapterNumberString = chapterNumberString.Replace("Resolution", "");
            chapterNumberString = chapterNumberString.Trim();

            int chapterNumber = int.Parse(chapterNumberString);
            activeCampaignChapterNumber = chapterNumber;
        }
        else if (GsmStagesEqual(arenaStages)) // arena
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

        SetStageImage();
    }

    // Update is called once per frame
    void Update ()
    {
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
        versusOptionsMenu.SetActive(false);
        versusMenu.SetActive(true);
        versusMenuStartOption.Select();
        versus = true;

        if (!GsmStagesEqual(stages))
        {
            manager.gsm.activeStageIndex = 0;
            manager.gsm.SetStages(stages);
        }

        SetStageImage();
    }

    public void VersusOptionsButtonPressed()
    {
        versusMenu.SetActive(false);
        versusOptionsMenu.SetActive(true);

        SetSettings();
        versusOptionsMenuStartOption.Select();
    }

    public void CoopButtonPressed()
    {
        playOptionsMenu.SetActive(false);
        coopMenu.SetActive(true);
        coopMenuStartOption.Select();
        versus = false;

        if (GameConstants.Unlocks.allCoopGameModes[activeCoopGameModeIndex] == "Arena")
        {
            if (!GsmStagesEqual(arenaStages))
            {
                manager.gsm.activeStageIndex = 0;
                manager.gsm.SetStages(arenaStages);
            }
        }
        else if (GameConstants.Unlocks.allCoopGameModes[activeCoopGameModeIndex] == "Campaign")
        {
            if (!GsmStagesEqual(coopStages))
            {
                manager.gsm.activeStageIndex = 0;
                manager.gsm.SetStages(coopStages);
            }
        }

        SetStageImage();
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

            SetChapterIndeces(activeCampaignChapterNumber);

            arenaSelectedStage.gameObject.SetActive(false);
            coopSelectedStage.gameObject.SetActive(true);
        }

        SetStageImage();
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

            SetChapterIndeces(activeCampaignChapterNumber);

            arenaSelectedStage.gameObject.SetActive(false);
            coopSelectedStage.gameObject.SetActive(true);
        }

        SetStageImage();
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
                if (coopActiveStageIndex < currentChapterStartingIndex)
                {
                    coopActiveStageIndex = currentChapterEndingIndex;
                }

                coopSelectedStage.text = coopStages[coopActiveStageIndex];
                manager.gsm.activeStageIndex = coopActiveStageIndex;
            }
        }

        SetStageImage();
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
                if (coopActiveStageIndex > currentChapterEndingIndex)
                {
                    coopActiveStageIndex = currentChapterStartingIndex;
                }

                coopSelectedStage.text = coopStages[coopActiveStageIndex];
                manager.gsm.activeStageIndex = coopActiveStageIndex;
            }
        }
        
        SetStageImage();
    }

    public void BackChapterPressed()
    {
        activeCampaignChapterNumber--;

        if (activeCampaignChapterNumber < 1)
        {
            activeCampaignChapterNumber = numberOfAvailableChapters;
        }

        SetChapterIndeces(activeCampaignChapterNumber);
    }

    public void ForwardChapterPressed()
    {
        activeCampaignChapterNumber++;

        if (activeCampaignChapterNumber > numberOfAvailableChapters)
        {
            activeCampaignChapterNumber = 1;
        }

        SetChapterIndeces(activeCampaignChapterNumber);
    }

    public void SetChapterIndeces(int chapter)
    {
        currentChapterStartingIndex = coopStages.IndexOf("Ch " + chapter);
        if (currentChapterStartingIndex < 0)
        {
            currentChapterStartingIndex = 0;
        }

        for (int i = 0; i < coopStages.Count; i++)
        {
            if (coopStages[i].Contains("Ch " + chapter))
            {
                currentChapterEndingIndex = i;
            }
        }

        coopActiveStageIndex = currentChapterStartingIndex;
        coopCurrentChapter.text = "Chapter " + activeCampaignChapterNumber;

        coopSelectedStage.text = coopStages[coopActiveStageIndex];
        SetStageImage(coopStages[coopActiveStageIndex]);
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

    public void SetStageImage(string stageName = "")
    {
        if (stageName == "")
        {
            if (manager.gsm.stages.Count > 0)
            {
                stageName = manager.gsm.stages[manager.gsm.activeStageIndex];
            }
        }

        Sprite newImage = (Sprite)Resources.Load("StageImages/" + stageName, typeof(Sprite));

        if (newImage != null)
        {
            versusStageImage.sprite = newImage;
            coopStageImage.sprite = newImage; // maybe these two images could be combined?
        }
        else
        {
            versusStageImage.sprite = defaultStageImage;
            coopStageImage.sprite = defaultStageImage;
        }
    }

    private void SetNumberOfAvailableChapters()
    {
        for (int i = 1; i < 100 /*Just need a number I guess*/; i++)
        {
            if (!coopStages.Contains("Ch " + i))
            {
                numberOfAvailableChapters = i - 1;
                return;
            }
        }
    }

    private bool GsmStagesEqual(List<string> compare)
    {
        if (manager.gsm.stages.Count != compare.Count)
        {
            return false;
        }

        for (int i = 0; i < manager.gsm.stages.Count; i++)
        {
            if (manager.gsm.stages[i] != compare[i])
            {
                return false;
            }
        }

        return true;
    }

    //////////     VERSUS OPTIONS SETTINGS     //////////
    
    public void ToggleRandomStageSelect()
    {
        manager.gsm.settings.SetRandomStageSelect(!manager.gsm.settings.randomStageSelect);
        randomStageSelectSelected.SetActive(!randomStageSelectSelected.activeSelf);
    }

    public void ForwardNumRounds()
    {
        manager.gsm.settings.SetRoundsPerStage(manager.gsm.settings.roundsPerStage + 1);

        if (manager.gsm.settings.roundsPerStage > maxNumberOfRounds)
        {
            manager.gsm.settings.SetRoundsPerStage(1);
        }

        numberOfRounds.text = manager.gsm.settings.roundsPerStage.ToString();
    }

    public void BackNumRounds()
    {
        manager.gsm.settings.SetRoundsPerStage(manager.gsm.settings.roundsPerStage - 1);

        if (manager.gsm.settings.roundsPerStage < 1)
        {
            manager.gsm.settings.SetRoundsPerStage(maxNumberOfRounds);
        }

        numberOfRounds.text = manager.gsm.settings.roundsPerStage.ToString();
    }

    public void ForwardNumLives()
    {
        manager.gsm.settings.SetLivesPerRound(manager.gsm.settings.livesPerRound + 1);
        
        if (manager.gsm.settings.livesPerRound > maxNumberOfLives)
        {
            manager.gsm.settings.SetLivesPerRound(1);
        }
        
        numberOfLives.text = manager.gsm.settings.livesPerRound.ToString();
    }

    public void BackNumLives()
    {
        manager.gsm.settings.SetLivesPerRound(manager.gsm.settings.livesPerRound - 1);
        
        if (manager.gsm.settings.livesPerRound < 1)
        {
            manager.gsm.settings.SetLivesPerRound(maxNumberOfLives);
        }
        
        numberOfLives.text = manager.gsm.settings.livesPerRound.ToString();
    }

    public void ForwardTime()
    {
        manager.gsm.settings.SetTimePerRound(manager.gsm.settings.timePerRound + 30);

        if (manager.gsm.settings.timePerRound > maxTimePerRound)
        {
            manager.gsm.settings.SetTimePerRound(0);
        }

        timePerRound.text = ParseTime(manager.gsm.settings.timePerRound);
    }

    public void BackTime()
    {
        manager.gsm.settings.SetTimePerRound(manager.gsm.settings.timePerRound - 30);

        if (manager.gsm.settings.timePerRound < 0)
        {
            manager.gsm.settings.SetTimePerRound(maxTimePerRound);
        }

        timePerRound.text = ParseTime(manager.gsm.settings.timePerRound);
    }

    private string ParseTime(int seconds)
    {
        if (seconds == 0)
        {
            return "∞";
        }

        return GameConstants.TimeUtilities.ParseTime(seconds);
    }
}
