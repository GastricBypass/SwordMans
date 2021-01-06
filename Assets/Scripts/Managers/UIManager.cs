using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public GameSettingsManager gsm;
    public bool useInBounds = true;

    public List<Transform> playerSpawns;

    public StandaloneInputModule control;

    public Slider[] healthMeters;
    public Text[] healthValues;
    public Slider[] boostMeters;
    public Image[] hurtDisplays;
    public Text[] damageTexts;

    public Image[] roundBubbles;

    public GameObject pauseMenu;

    public bool paused = false;

    public Text roundNumber;
    public Text endGamePlayerWinText;
    public Text endGameCountdownTimer;

    public Text timeRemaining;

    public GameObject[] lifeCounters;
    public Text[] livesRemaining;

    public bool gameOver;

    public GameObject unlockedItem;
    public Text unlockedItemOrStage;
    public Image unlockedItemImage;
    public Text unlockedItemText;

    protected bool[] deadPlayers; // deadPlayers[0] will be true if player1 is dead.
    protected AudioSource audioSource;
    private int winningPlayerNumber = 0;

    // Use this for initialization
    public virtual void Start()
    {
        CommonStart();

        gsm.SpawnPlayers();

        endGamePlayerWinText.gameObject.SetActive(false);

        DisplayRoundBubbles();

        SetHealthBars();

        SetTimeRemaining();

        SetLives();

        StartCoroutine(DisplayRoundNumber());
    }

    public void CommonStart()
    {
        gsm = FindObjectOfType<GameSettingsManager>();
        deadPlayers = new bool[gsm.numberOfPlayers + gsm.numberOfAIPlayers];

        audioSource = gameObject.AddComponent<AudioSource>();

        RectTransform backdrop = pauseMenu.transform.Find("Backdrop").GetComponent<RectTransform>();
        backdrop.sizeDelta = new Vector2(Screen.width * 2, Screen.height * 2);

        pauseMenu.SetActive(false);
        endGameCountdownTimer.gameObject.SetActive(false);

        hurtDisplays[0].gameObject.SetActive(false);
        hurtDisplays[1].gameObject.SetActive(false);
        hurtDisplays[2].gameObject.SetActive(false);
        hurtDisplays[3].gameObject.SetActive(false);

        damageTexts[0].gameObject.SetActive(false);
        damageTexts[1].gameObject.SetActive(false);
        damageTexts[2].gameObject.SetActive(false);
        damageTexts[3].gameObject.SetActive(false);

        unlockedItem.SetActive(false);

        for (int i = 0; i < gsm.playerSpawns.Count; i++)
        {
            if (playerSpawns[i] == null)
            {
                GameObject spawn = GameObject.Find("Player Spawn (" + (i + 1) + ")");
                playerSpawns[i] = spawn.transform;
            }
            gsm.playerSpawns[i] = playerSpawns[i];
        }
    }

    // Update is called once per frame
    public void Update ()
    {
        if (gsm.currentStage != "Main Menu")
        {
            if (Input.GetButtonDown("Start"))
            {
                StartPressed();
            }

            if (!gameOver && CheckWinStatus())
            {
                StartCoroutine(StartEndGameCountdown());
            }
        }
	}

    public virtual void ChangeHealth(float percent, int playerNumber)
    {
        if (playerNumber > 0 && playerNumber <= 4)
        {
            healthMeters[playerNumber - 1].value = percent * GameConstants.Players.playerMaxHealth;
            healthValues[playerNumber - 1].text = (Mathf.Ceil(healthMeters[playerNumber - 1].value) + " / " + GameConstants.Players.playerMaxHealth);
        }
    }

    public virtual void ShowHurtImage(int playerNumber, float damage)
    {
        if (playerNumber > 0 && playerNumber <= 4)
        {
            StartCoroutine(ShowHurtImageForSeconds(hurtDisplays[playerNumber - 1], damage, 0.2f));
            StartCoroutine(ShowHurtTextForSeconds(damageTexts[playerNumber - 1], damage, 0.5f));
        }
    }

    public IEnumerator ShowHurtImageForSeconds(Image image, float damage, float time)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, damage / 200);
        image.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        image.gameObject.SetActive(false);
    }

    public IEnumerator ShowHurtTextForSeconds(Text text, float damage, float time)
    {
        text.text = ((int)damage).ToString();
        if (text.text != "0")
        {
            text.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(time);
        text.gameObject.SetActive(false);
    }

    public void ChangeBoost(float percent, int playerNumber)
    {
        if (playerNumber > 0 && playerNumber <= 4)
        {
            boostMeters[playerNumber - 1].value = percent * 100;
        }
    }

    public void DisplayRoundBubbles()
    {
        int i = 0;
        while (i < gsm.settings.roundsPerStage && i < gsm.wins.Length)
        {
            roundBubbles[i].gameObject.SetActive(true);

            if (gsm.wins[i] != 0) 
            {
                roundBubbles[i].color = GameConstants.PlayerColors.ParseFromName(gsm.playerColors[gsm.wins[i] - 1]);
            }

            i++;
        }

        while (i < 5) // max number of rounds
        {
            roundBubbles[i].gameObject.SetActive(false);
            i++;
        }
    }

    public virtual void StartPressed()
    {
        if (paused)
        {
            Time.timeScale = 1;
            gsm.SetCursor(false);
        }
        else
        {
            Time.timeScale = 0;
            gsm.SetCursor(true);
        }

        paused = !paused;
        pauseMenu.SetActive(paused);
        if (paused)
        {
            Button resumeButton = pauseMenu.transform.Find("Resume Button").GetComponent<Button>();
            if (resumeButton != null && resumeButton.gameObject.activeSelf)
            {
                resumeButton.Select();
            }
            else
            {
                Button playButton = pauseMenu.transform.Find("Go Button").GetComponent<Button>();
                if (playButton != null && playButton.gameObject.activeSelf)
                {
                    playButton.Select();
                }
            }
        }
    }

    public void MainMenuPressed()
    {
        gsm.LoadMainMenu();
        Time.timeScale = 1;
    }

    public void NextStagePressed()
    {
        gsm.LoadNextStage();
        Time.timeScale = 1;
    }

    public void PreviousStagePressed()
    {
        gsm.LoadPrevStage();
        Time.timeScale = 1;
    }

    public void RestartLevelPressed()
    {
        gsm.LoadCurrentStage();
        Time.timeScale = 1;
    }

    public void SetHealthBars()
    {
        int i = 0;
        // Set active players' health bars active
        while (i < gsm.numberOfPlayers + gsm.numberOfAIPlayers)
        {
            healthMeters[i].gameObject.SetActive(true);
            boostMeters[i].gameObject.SetActive(true);
            
            SetHealthBarColor(healthMeters[i], gsm.playerColors[i]);

            healthValues[i].gameObject.SetActive(gsm.settings.showHealthValues);

            i++;
        }
        // Set remaining players' health bars inactive
        while (i < 4)
        {
            healthMeters[i].gameObject.SetActive(false);
            boostMeters[i].gameObject.SetActive(false);

            i++;
        }
    }

    public void SetHealthBarColor(Slider healthBar, string color)
    {
        if (!gsm.settings.colorizeHealthBars)
        {
            return;
        }

        Image fill = healthBar.transform.Find("Fill Area/Fill").GetComponent<Image>();
        if (fill == null) return;

        fill.color = GameConstants.UIColors.ParseFromName(color);
    }

    public virtual bool CheckWinStatus()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu" || gsm.numberOfPlayers + gsm.numberOfAIPlayers == 1)
        {
            return false;
        }

        int numDead = 0;
        int alivePlayer = 0;

        for (int i = 0; i < deadPlayers.Length; i++)
        {
            if (deadPlayers[i])
            {
                int lives;
                if (TryParseLivesRemaining(livesRemaining[i].text, out lives)) 
                {
                    if (lives > 1)
                    {
                        livesRemaining[i].text = "x" + (lives - 1);
                        StartCoroutine(RespawnPlayerAfterTime(i + 1, 1));
                        deadPlayers[i] = false;
                    }
                    else
                    {
                        numDead++;
                    }
                }
            }
            else
            {
                alivePlayer = i + 1;
            }
        }

        if (numDead >= gsm.numberOfPlayers + gsm.numberOfAIPlayers - 1)
        {
            //if (numDead == 0)  // Removing the gsm.numberOfPlayers + gsm.numberOfAIPlayers == 1 conditional at the beginning of this method and adding this commented part allows you to 
            //{                  // continue to the next round when you die while playing versus solo. However, it will also allow you to get drops. In the interest of not being exploitable,
            //    return false;  // the game freezes when you die while exploring the versus stages solo.
            //}

            winningPlayerNumber = alivePlayer;
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator RespawnPlayerAfterTime(int playerNumber, float time)
    {
        yield return new WaitForSeconds(time);

        gsm.RespawnPlayers(new int[] { playerNumber });
    }

    public void SetTimeRemaining()
    {
        if (gsm.settings.timePerRound > 0)
        {
            timeRemaining.gameObject.SetActive(true);
            timeRemaining.text = GameConstants.TimeUtilities.ParseTime(gsm.settings.timePerRound);
            StartCoroutine(EndRoundAfterTime(gsm.settings.timePerRound));
        }
        else
        {
            timeRemaining.gameObject.SetActive(false);
        }
    }

    public void SetLives()
    {
        if (gsm.settings.livesPerRound > 1)
        {
            foreach (GameObject lifeCounter in lifeCounters)
            {
                lifeCounter.SetActive(true);
            }

            foreach (var lifeText in livesRemaining)
            {
                lifeText.text = "x" + gsm.settings.livesPerRound;
            }
        }
        else
        {
            foreach (GameObject lifeCounter in lifeCounters)
            {
                lifeCounter.SetActive(false);
            }
        }

        foreach (var lifeText in livesRemaining)
        {
            lifeText.text = "x" + gsm.settings.livesPerRound;
        }
    }

    public virtual void PlayerDead(int playerNumber)
    {
        if (playerNumber > 0 && playerNumber <= 4)
        {
            deadPlayers[playerNumber - 1] = true;
        }
    }

    public IEnumerator EndRoundAfterTime(int seconds)
    {
        for (int i = seconds; i >= 0; i--)
        {
            yield return new WaitForSeconds(1);
            timeRemaining.text = GameConstants.TimeUtilities.ParseTime(i);
        }

        winningPlayerNumber = GetPlayerNumberWithMostHealth();
        if (!gameOver && winningPlayerNumber != 0)
        {
            StartCoroutine(StartEndGameCountdown());
        }
    }

    public int GetPlayerNumberWithMostHealth()
    {
        float maxHealth = 0;
        int maxLives = 0;
        int playerNumber = 0;
        int numInactivePlayers = 0;

        for (int i = 0; i < healthMeters.Length; i++)
        {
            if (!healthMeters[i].IsActive())
            {
                numInactivePlayers++;
                continue;
            }
            
            int lives;
            if (TryParseLivesRemaining(livesRemaining[i].text, out lives))
            {
                if (lives > maxLives)
                {
                    maxLives = lives;
                    maxHealth = healthMeters[i].value;
                    playerNumber = i + 1;
                }
                else if (lives == maxLives)
                {
                    if (healthMeters[i].value > maxHealth)
                    {
                        maxHealth = healthMeters[i].value;
                        playerNumber = i + 1;
                    }
                }
            }
        }

        if (numInactivePlayers >= 3)
        {
            return 0;
        }

        return playerNumber;
    }

    private bool TryParseLivesRemaining(string livesText, out int livesRemaining)
    {
        string number = livesText.Replace("x", string.Empty);

        return int.TryParse(number, out livesRemaining);
    }

    public virtual IEnumerator StartEndGameCountdown()
    {
        UnlockItemByChance();

        gameOver = true;

        endGamePlayerWinText.gameObject.SetActive(true);
        endGamePlayerWinText.text = "Player " + winningPlayerNumber + " Wins!";
        gsm.wins[gsm.roundNumber - 1] = winningPlayerNumber;
        DisplayRoundBubbles();

        if (gsm.roundNumber == gsm.settings.roundsPerStage)
        {
            gsm.music.FadeOut(3);
        }

        endGameCountdownTimer.gameObject.SetActive(true);
        endGameCountdownTimer.text = "3";

        yield return new WaitForSeconds(1);
        endGameCountdownTimer.text = "2";

        yield return new WaitForSeconds(1);
        endGameCountdownTimer.text = "1";

        yield return new WaitForSeconds(1);
        gsm.NextRound();
    }

    public IEnumerator DisplayRoundNumber()
    {
        roundNumber.gameObject.SetActive(true);
        roundNumber.text = gsm.currentStage + "\nRound " + gsm.roundNumber;
        yield return new WaitForSeconds(2);
        roundNumber.gameObject.SetActive(false);
    }

    public void UnlockItemByChance()
    {
        if (Random.Range(0f, 1f) < gsm.chanceToUnlockItemsEachRound)
        {
            int itemIndex = Random.Range(0, GameConstants.Unlocks.purchasableHats.Count + GameConstants.Unlocks.purchasableMisc.Count);

            if (itemIndex < GameConstants.Unlocks.purchasableHats.Count)
            {
                gsm.data.UnlockHat(GameConstants.Unlocks.purchasableHats[itemIndex]);
            }
            else
            {
                gsm.data.UnlockMisc(GameConstants.Unlocks.purchasableMisc[itemIndex - GameConstants.Unlocks.purchasableMisc.Count]);
            }
        }
    }

    public void UnlockItem(string itemName, string goldValue = "")
    {
        Sprite newImage;

        unlockedItemText.text = goldValue + itemName;

        if (GameConstants.Unlocks.allVersusStages.Contains(itemName) || GameConstants.Unlocks.allCoopStages.Contains(itemName))
        {
            unlockedItemOrStage.text = "Stage Unlocked";
            newImage = (Sprite)Resources.Load("StageImages/" + itemName, typeof(Sprite));
        }
        else
        {
            unlockedItemOrStage.text = "Item Unlocked";
            newImage = (Sprite)Resources.Load("ItemImages/" + itemName, typeof(Sprite));
        }

        if (newImage != null)
        {
            unlockedItemImage.sprite = newImage;
        }

        AudioClip clip = (AudioClip)Resources.Load("Sounds/unlockItem", typeof(AudioClip));
        audioSource.clip = clip;
        audioSource.volume = gsm.settings.effectsVolume;
        audioSource.Play();

        StartCoroutine(ShowUnlockForSeconds(3));
    }

    public IEnumerator ShowUnlockForSeconds(float time)
    {
        unlockedItem.SetActive(true);

        yield return new WaitForSeconds(time);

        unlockedItem.SetActive(false);
    }
}
