using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SkirmishUIManager : UIManager
{
    public List<EnemySpawner> enemySpawners;
    public Text enemiesRemaining;
    public Text waveCounter;

    public GameObject gameOverScreen;

    private int waveNumber;

    public override void Start()
    {
        CommonStart();

        gsm.SpawnPlayers();
        SetHealthBars();
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
            return true;
        }

        return false;
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
        endGameCountdownTimer.gameObject.SetActive(false);

        DisplayGameOverOptions();
    }

    public void DisplayGameOverOptions()
    {
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
    }
}
