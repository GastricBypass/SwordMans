using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {

    public GameSettingsManager gsm;
    public bool useInBounds = true;
    public Transform player1Spawn;
    public Transform player2Spawn;
    public Transform player3Spawn;
    public Transform player4Spawn;

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

    protected bool[] deadPlayers;
    private int winningPlayerNumber = 0;

    // Use this for initialization
    public virtual void Start()
    {
        pauseMenu.SetActive(false);
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

        gsm = FindObjectOfType<GameSettingsManager>();
        gsm.player1Spawn = player1Spawn;
        gsm.player2Spawn = player2Spawn;
        gsm.player3Spawn = player3Spawn;
        gsm.player4Spawn = player4Spawn;

        deadPlayers = new bool[4];

        DisplayRoundBubbles();

        gsm.SpawnPlayers();

        SetHealthBars();

        StartCoroutine(DisplayRoundNumber());
    }
	
	// Update is called once per frame
	public void Update ()
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

    public virtual void ChangeHealth(float percent, int playerNumber)
    {
        if (playerNumber > 0)
        {
            healthMeters[playerNumber - 1].value = percent * GameConstants.Players.playerMaxHealth;
            healthValues[playerNumber - 1].text = ((int)(healthMeters[playerNumber - 1].value + 0.9f) + " / " + GameConstants.Players.playerMaxHealth);
        }
    }

    public virtual void ShowHurtImage(int playerNumber, float damage)
    {
        if (playerNumber > 0)
        {
            StartCoroutine(ShowImageForSeconds(hurtDisplays[playerNumber - 1], damage, 0.2f));
            StartCoroutine(ShowTextForSeconds(damageTexts[playerNumber - 1], damage, 0.5f));
        }
    }

    public IEnumerator ShowImageForSeconds(Image image, float damage, float time)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, damage / 200);
        image.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        image.gameObject.SetActive(false);
    }

    public IEnumerator ShowTextForSeconds(Text text, float damage, float time)
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
        if (playerNumber > 0)
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
            if (gsm.wins[i] == 1)
                roundBubbles[i].color = DetermineColor(gsm.player1Color); 
            else if (gsm.wins[i] == 2)
                roundBubbles[i].color = DetermineColor(gsm.player2Color);
            else if (gsm.wins[i] == 3)
                roundBubbles[i].color = DetermineColor(gsm.player3Color);
            else if (gsm.wins[i] == 4)
                roundBubbles[i].color = DetermineColor(gsm.player4Color);

            i++;
        }

        while (i < 5) // max number of rounds
        {
            roundBubbles[i].gameObject.SetActive(false);
            i++;
        }
    }

    public Color DetermineColor(string c)
    {
        if (c == "Red") 
            return GameConstants.PlayerColors.red;
        if (c == "Blue")
            return GameConstants.PlayerColors.blue;
        if (c == "Green")
            return GameConstants.PlayerColors.green;
        if (c == "Yellow")
            return GameConstants.PlayerColors.yellow;
        if (c == "Purple")
            return GameConstants.PlayerColors.purple;
        if (c == "Orange")
            return GameConstants.PlayerColors.orange;
        else return Color.gray;
    }

    public void StartPressed()
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
        while (i < gsm.numberOfPlayers)
        {
            healthMeters[i].gameObject.SetActive(true);
            boostMeters[i].gameObject.SetActive(true);

            if (i == 0)
                SetHealthBarColor(healthMeters[i], gsm.player1Color);
            if (i == 1)
                SetHealthBarColor(healthMeters[i], gsm.player2Color);
            if (i == 2)
                SetHealthBarColor(healthMeters[i], gsm.player3Color);
            if (i == 3)
                SetHealthBarColor(healthMeters[i], gsm.player4Color);

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

        //if (gsm.numberOfPlayers >= 1)
        //{
        //    healthMeter1.gameObject.SetActive(true);
        //    boostMeter1.gameObject.SetActive(true);

        //    SetHealthBarColor(healthMeter1, gsm.player1Color);
        //    healthValues1.gameObject.SetActive(gsm.settings.showHealthValues);
        //}
        //else
        //{
        //    healthMeter1.gameObject.SetActive(false);
        //    boostMeter1.gameObject.SetActive(false);
        //}

        //if (gsm.numberOfPlayers >= 2)
        //{
        //    healthMeter2.gameObject.SetActive(true);
        //    boostMeter2.gameObject.SetActive(true);

        //    SetHealthBarColor(healthMeter2, gsm.player2Color);
        //    healthValues2.gameObject.SetActive(gsm.settings.showHealthValues);
        //}
        //else
        //{
        //    healthMeter2.gameObject.SetActive(false);
        //    boostMeter2.gameObject.SetActive(false);
        //}

        //if (gsm.numberOfPlayers >= 3)
        //{
        //    healthMeter3.gameObject.SetActive(true);
        //    boostMeter3.gameObject.SetActive(true);

        //    SetHealthBarColor(healthMeter3, gsm.player3Color);
        //    healthValues3.gameObject.SetActive(gsm.settings.showHealthValues);
        //}
        //else
        //{
        //    healthMeter3.gameObject.SetActive(false);
        //    boostMeter3.gameObject.SetActive(false);
        //}

        //if (gsm.numberOfPlayers >= 4)
        //{
        //    healthMeter4.gameObject.SetActive(true);
        //    boostMeter4.gameObject.SetActive(true);

        //    SetHealthBarColor(healthMeter4, gsm.player4Color);
        //    healthValues4.gameObject.SetActive(gsm.settings.showHealthValues);
        //}
        //else
        //{
        //    healthMeter4.gameObject.SetActive(false);
        //    boostMeter4.gameObject.SetActive(false);
        //}
    }

    public void SetHealthBarColor(Slider healthBar, string color)
    {
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

        //for (int i = 0; i < gsm.numberOfPlayers; i++)
        //{
        //    if (healthMeters[i].value <= 0)
        //    {
        //        numDead++;
        //    }
        //    else
        //    {
        //        alivePlayer = i + 1;
        //    }
        //}

        if (numDead >= gsm.numberOfPlayers - 1)
        {
            winningPlayerNumber = alivePlayer;
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void PlayerDead(int playerNum)
    {
        if (playerNum > 0)
        {
            deadPlayers[playerNum - 1] = true;
        }
    }

    public virtual IEnumerator StartEndGameCountdown()
    {
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
}
