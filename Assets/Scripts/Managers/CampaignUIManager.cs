using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CampaignUIManager : UIManager
{
    public string nextLevelName;
    public NextLevelArea nextLevelProgressionArea;
    public bool introLevel;
    public bool cutsceneLevel;
    public bool bossLevel;
    public Slider bossHealthBar;
    public Text bossHealthValue;
    public float bossMaxHealth;
    public List<PathFollower> objectsToMoveOnBossDeath;

    public GameObject resumeButton;
    public GameObject playButton;
    public Canvas extraCanvas;

    protected bool playersLost;

	// Use this for initialization
	public override void Start ()
    {
        CommonStart();

        hurtDisplays[4].gameObject.SetActive(false);
        damageTexts[4].gameObject.SetActive(false);

        gsm.numberOfAIPlayers = 0;

        gsm.SpawnPlayers();

        if (!introLevel && !cutsceneLevel)
        {
            SetHealthBars();
        }
        else
        {
            for (int i = 0; i< healthMeters.Length; i++)
            {
                healthMeters[i].gameObject.SetActive(false);
                boostMeters[i].gameObject.SetActive(false);
            }
        }

        bossHealthBar.gameObject.SetActive(bossLevel);
        bossMaxHealth = bossMaxHealth + (bossMaxHealth * gsm.numberOfPlayers / 2f);
        bossHealthBar.maxValue = bossMaxHealth;
        ChangeHealth(1, 0); // even if the boss's player number isn't 0, this is fine
    }

    public override void ChangeHealth(float percent, int playerNumber)
    {
        base.ChangeHealth(percent, playerNumber);
        if (bossLevel && (playerNumber == 0 || playerNumber > 4))
        {
            bossHealthBar.value = percent * bossMaxHealth;
            bossHealthValue.text = (Mathf.Ceil(bossHealthBar.value) + " / " + bossMaxHealth);
            if (bossHealthBar.value <= 0)
            {
                MoveBossDeathObjects();
            }
        }
    }

    public override void ShowHurtImage(int playerNumber, float damage)
    {
        base.ShowHurtImage(playerNumber, damage);
        if (bossLevel && (playerNumber == 0 || playerNumber > 4))
        {
            StartCoroutine(ShowHurtImageForSeconds(hurtDisplays[4], damage, 0.2f));
            StartCoroutine(ShowHurtTextForSeconds(damageTexts[4], damage, 0.5f));
        }
    }

    public override void StartPressed()
    {
        if (extraCanvas != null)
        {
            extraCanvas.gameObject.SetActive(!extraCanvas.gameObject.activeSelf);
        }
        if (cutsceneLevel)
        {
            LoadNextLevel();
        }
        else
        {
            // For intro levels, you'll need to enable the "play" button and disable the "resume" button.
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

        if (nextLevelProgressionArea.AllPlayersInArea(gsm.numberOfPlayers, deadPlayers))
        {
            return true;
        }

        return false;
    }

    // Start countdown from a button
    public void StartNextLevelCountdown()
    {
        StartCoroutine(StartEndGameCountdown());
    }

    public override IEnumerator StartEndGameCountdown()
    {
        gameOver = true;

        //if (gsm.roundNumber == gsm.settings.roundsPerStage)
        //{
        //    gsm.music.FadeOut(3);
        //}

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

    public void MoveBossDeathObjects()
    {
        foreach (var obj in objectsToMoveOnBossDeath)
        {
            obj.active = true;
        }
    }

    public void LoadNextLevel()
    {
        // bypass the game settings manager and just load the scene. That way it doesn't need to be unlocked
        // This is needed for the intro levels, nothing else (I think)
        SceneManager.LoadScene(nextLevelName); 
    }
}
