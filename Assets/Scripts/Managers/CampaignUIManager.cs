using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CampaignUIManager : UIManager
{
    public string nextLevelName;
    public NextLevelArea nextLevelProgressionArea;
    public bool bossLevel;
    public Slider bossHealthBar;
    public Text bossHealthValue;
    public float bossMaxHealth;

    public bool introLevel;

    private bool playersLost;

	// Use this for initialization
	public override void Start ()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        gsm = FindObjectOfType<GameSettingsManager>();

        for (int i = 0; i < gsm.playerSpawns.Count; i++)
        {
            if (playerSpawns[i] == null)
            {
                GameObject.Find("PlayerSpawn" + i);
            }
            gsm.playerSpawns[i] = playerSpawns[i];
        }

        pauseMenu.SetActive(false);
        endGameCountdownTimer.gameObject.SetActive(false);

        hurtDisplays[0].gameObject.SetActive(false);
        hurtDisplays[1].gameObject.SetActive(false);
        hurtDisplays[2].gameObject.SetActive(false);
        hurtDisplays[3].gameObject.SetActive(false);
        hurtDisplays[4].gameObject.SetActive(false);

        damageTexts[0].gameObject.SetActive(false);
        damageTexts[1].gameObject.SetActive(false);
        damageTexts[2].gameObject.SetActive(false);
        damageTexts[3].gameObject.SetActive(false);
        damageTexts[4].gameObject.SetActive(false);

        unlockedItem.SetActive(false);

        //gsm.player1Spawn = player1Spawn;
        //gsm.player2Spawn = player2Spawn;
        //gsm.player3Spawn = player3Spawn;
        //gsm.player4Spawn = player4Spawn;

        gsm.numberOfAIPlayers = 0;

        deadPlayers = new bool[gsm.numberOfPlayers];
        gsm.SpawnPlayers();

        if (!introLevel)
        {
            SetHealthBars();
        }

        bossHealthBar.gameObject.SetActive(bossLevel);
    }

    public override void ChangeHealth(float percent, int playerNumber)
    {
        base.ChangeHealth(percent, playerNumber);
        if (bossLevel && playerNumber == 0)
        {
            bossHealthBar.value = percent * bossMaxHealth;
            bossHealthValue.text = ((int)(bossHealthBar.value + 0.9f) + " / " + bossMaxHealth);
        }
    }

    public override void ShowHurtImage(int playerNumber, float damage)
    {
        base.ShowHurtImage(playerNumber, damage);
        if (bossLevel && playerNumber == 0)
        {
            StartCoroutine(ShowHurtImageForSeconds(hurtDisplays[4], damage, 0.2f));
            StartCoroutine(ShowHurtTextForSeconds(damageTexts[4], damage, 0.5f));
        }
    }

    public override void StartPressed()
    {
        if (introLevel)
        {
            StartCoroutine(StartEndGameCountdown());
        }
        else
        {
            base.StartPressed();
        }
    }

    public override bool CheckWinStatus()
    {
        int numDead = 0;

        for (int i = 0; i < deadPlayers.Length; i++)
        {
            if (deadPlayers[i])
            {
                numDead++;
            }
        }

        if (numDead >= gsm.numberOfPlayers)
        {
            playersLost = true;
            return true;
        }

        if (WinConditions())
        {
            if (nextLevelProgressionArea.playersInArea.Count >= gsm.numberOfPlayers - numDead)
            {
                return true;
            }
        }

        return false;
    }

    public bool WinConditions()
    {
        return true;
    }

    public override IEnumerator StartEndGameCountdown()
    {
        gameOver = true;

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
        if (playersLost)
        {
            gsm.LoadStage(SceneManager.GetActiveScene().name);
        }
        else
        {
            StartNextLevel();
        }
    }

    public void StartNextLevel()
    {
        gsm.data.UnlockCoopStage(nextLevelName);
        gsm.LoadStage(nextLevelName);
    }
}
