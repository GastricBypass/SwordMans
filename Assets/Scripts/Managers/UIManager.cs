using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {

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
    public bool gameOver;

    public GameObject unlockedItem;
    public Image unlockedItemImage;
    public Text unlockedItemText;

    protected bool[] deadPlayers;
    protected AudioSource audioSource;
    private int winningPlayerNumber = 0;

    // Use this for initialization
    public virtual void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        pauseMenu.SetActive(false);
        RectTransform backdrop = pauseMenu.transform.Find("Backdrop").GetComponent<RectTransform>();
        backdrop.sizeDelta = new Vector2(Screen.width * 2, Screen.height * 2);

        endGamePlayerWinText.gameObject.SetActive(false);
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

        gsm = FindObjectOfType<GameSettingsManager>();

        for (int i = 0; i < gsm.playerSpawns.Count; i++)
        {
            if (playerSpawns[i] == null)
            {
                Debug.Log("player " + i + " spawn found");
                GameObject spawn = GameObject.Find("Player Spawn (" + (i + 1) + ")");
                playerSpawns[i] = spawn.transform;
            }
            gsm.playerSpawns[i] = playerSpawns[i];
        }

        deadPlayers = new bool[gsm.numberOfPlayers + gsm.numberOfAIPlayers];

        DisplayRoundBubbles();

        gsm.SpawnPlayers();

        SetHealthBars();

        StartCoroutine(DisplayRoundNumber());
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
            healthValues[playerNumber - 1].text = ((int)(healthMeters[playerNumber - 1].value + 0.9f) + " / " + GameConstants.Players.playerMaxHealth);
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

            roundBubbles[i].color = GameConstants.PlayerColors.ParseFromName(gsm.playerColors[i]);

            i++;
        }

        while (i < 5) // max number of rounds
        {
            roundBubbles[i].gameObject.SetActive(false);
            i++;
        }
    }

    //public Color DetermineColor(string c)
    //{
    //    if (c == "Red") 
    //        return GameConstants.PlayerColors.red;
    //    if (c == "Blue")
    //        return GameConstants.PlayerColors.blue;
    //    if (c == "Green")
    //        return GameConstants.PlayerColors.green;
    //    if (c == "Yellow")
    //        return GameConstants.PlayerColors.yellow;
    //    if (c == "Purple")
    //        return GameConstants.PlayerColors.purple;
    //    if (c == "Orange")
    //        return GameConstants.PlayerColors.orange;
    //    else return Color.gray;
    //}

    public virtual void StartPressed()
    {
        if (paused)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }

        paused = !paused;
        pauseMenu.SetActive(paused);
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
        // Add faded colors to the game constants
        if (!gsm.settings.colorizeHealthBars)
        {
            return;
        }

        Image fill = healthBar.transform.Find("Fill Area/Fill").GetComponent<Image>();
        if (fill == null) return;
        if (color == "Red")
        {
            fill.color = GameConstants.PlayerColors.red;
            fill.color = new Color(fill.color.r, fill.color.g + 0.2f, fill.color.b + 0.2f);
        }
        else if (color == "Blue")
        {
            fill.color = GameConstants.PlayerColors.blue;
            fill.color = new Color(fill.color.r + 0.25f, fill.color.g + 0.25f, fill.color.b);

        }
        else if (color == "Green")
        {
            fill.color = GameConstants.PlayerColors.green;
        }
        else if (color == "Yellow")
        {
            fill.color = GameConstants.PlayerColors.yellow;
            fill.color = new Color(fill.color.r / 1.25f, fill.color.g / 1.25f, fill.color.b);
        }
        else if (color == "Orange")
        {
            fill.color = GameConstants.PlayerColors.orange;
        }
        else if (color == "Purple")
        {
            fill.color = GameConstants.PlayerColors.purple;
        }
        else
        {
            fill.color = Color.gray;
        }
    }

    public virtual bool CheckWinStatus()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            return false;
        }

        int numDead = 0;
        int alivePlayer = 0;

        for (int i = 0; i < deadPlayers.Length; i++)
        {
            if (deadPlayers[i])
            {
                numDead++;
            }
            else
            {
                alivePlayer = i + 1;
            }
        }

        if (numDead >= gsm.numberOfPlayers + gsm.numberOfAIPlayers - 1)
        {
            winningPlayerNumber = alivePlayer;
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void PlayerDead(int playerNumber)
    {
        if (playerNumber > 0 && playerNumber <= 4)
        {
            deadPlayers[playerNumber - 1] = true;
        }
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
            int itemIndex = Random.Range(0, GameConstants.Unlocks.allHats.Count + GameConstants.Unlocks.allMisc.Count);

            if (itemIndex < GameConstants.Unlocks.allHats.Count)
            {
                gsm.data.UnlockHat(GameConstants.Unlocks.allHats[itemIndex]);
            }
            else
            {
                gsm.data.UnlockMisc(GameConstants.Unlocks.allMisc[itemIndex - GameConstants.Unlocks.allHats.Count]);
            }
        }
    }

    public void UnlockItem(string itemName)
    {
        Sprite newImage = (Sprite)Resources.Load("ItemImages/" + itemName, typeof(Sprite));

        if (newImage != null)
        {
            unlockedItemImage.sprite = newImage;
        }
        unlockedItemText.text = itemName;

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
