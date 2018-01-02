using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SkirmishUIManager : UIManager
{
    public List<EnemyMan> possibleEnemyPrefabs;
    public List<EnemySpawner> enemySpawners;
    public Text enemiesRemainingText;
    public Text waveNumberText;

    public GameObject gameOverScreen;
    public Text gameOverReport;

    public Button retryButton;

    private int waveNumber = 0;
    private int enemiesRemaining = 0;
    private int totalEnemiesKilled = 0;
    private int totalGoldEarned = 0;

    public override void Start()
    {
        CommonStart();

        gameOverScreen.SetActive(false);

        RectTransform gameOverBackdrop = gameOverScreen.transform.Find("Backdrop").GetComponent<RectTransform>();
        gameOverBackdrop.sizeDelta = new Vector2(Screen.width * 2, Screen.height * 2);

        gsm.SpawnPlayers();
        SetHealthBars();

        StartNextWave();
    }

    public void StartNextWave()
    {
        gsm.data.AddGold(waveNumber);
        totalGoldEarned += waveNumber;

        waveNumber++;
        waveNumberText.text = "Wave " + waveNumber + ":";

        foreach (var spawner in enemySpawners)
        {
            spawner.active = false;
        }

        int numSpawners = enemySpawners.Count;
        int numPlayers = gsm.numberOfPlayers;

        int numEnemies = (int)Mathf.Ceil((waveNumber * numPlayers) / 2f);

        enemiesRemaining = numEnemies;
        enemiesRemainingText.text = "Enemies Remaining: " + enemiesRemaining;

        int prefabIndex = Random.Range(0, possibleEnemyPrefabs.Count);
        foreach (var spawner in enemySpawners)
        {
            spawner.numEnemiesToSpawn = numEnemies / numSpawners;
            spawner.enemyPrefab = possibleEnemyPrefabs[prefabIndex];
        }

        enemySpawners[0].numEnemiesToSpawn += numEnemies % numSpawners; // Give the remaining enemies to the first spawner

        StartCoroutine(WaitToStartNextRound());
    }

    public void EnemyDied()
    {
        enemiesRemaining--;
        totalEnemiesKilled++;
        enemiesRemainingText.text = "Enemies Remaining: " + enemiesRemaining;

        if (enemiesRemaining == 0)
        {
            StartNextWave();
        }
    }

    public IEnumerator WaitToStartNextRound()
    {
        endGameCountdownTimer.gameObject.SetActive(true);
        endGameCountdownTimer.text = "3";

        yield return new WaitForSeconds(1);
        endGameCountdownTimer.text = "2";

        yield return new WaitForSeconds(1);
        endGameCountdownTimer.text = "1";

        yield return new WaitForSeconds(1);
        endGameCountdownTimer.gameObject.SetActive(false);

        foreach (var spawner in enemySpawners)
        {
            spawner.active = true;
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

        gameOverReport.text = "Survived until wave " + waveNumber + "\n\nEnemies defeated: " + totalEnemiesKilled + "\nGold earned: " + totalGoldEarned;
        gameOverScreen.SetActive(true);
        retryButton.Select();
    }
}
