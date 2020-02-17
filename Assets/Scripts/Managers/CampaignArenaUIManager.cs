using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// It was at this point, he realized the arena stuff should have been a seperate thing.
// Almost everything is copied from ArenaUIManager because I'm awful.
public class CampaignArenaUIManager : CampaignUIManager
{
    public int roundsUntilWin = 5;

    public List<EnemyMan> possibleEasyEnemyPrefabs;
    public List<EnemyMan> possibleHardEnemyPrefabs;
    public List<EnemyMan> possibleBossPrefabs;
    public List<EnemySpawner> enemySpawners;
    public List<EnemySpawner> bossSpawners;
    public Text enemiesRemainingText;
    public Text waveNumberText;

    private int waveNumber = 0;
    private int enemiesRemaining = 0;
    private int totalEnemiesKilled = 0;

    public override void Start()
    {
        base.Start();

        StartNextWave();
    }

    public void StartNextWave()
    {
        if (gameOver)
        {
            return;
        }

        waveNumber++;
        waveNumberText.text = "Wave " + waveNumber + ":";

        if (waveNumber != roundsUntilWin)
        {
            StartNormalWave();
        }

        StartCoroutine(WaitToStartNextRound());
    }

    public void StartNormalWave()
    {
        bossHealthBar.gameObject.SetActive(false);

        foreach (var spawner in enemySpawners)
        {
            spawner.active = false;
        }

        int numSpawners = enemySpawners.Count;
        int numPlayers = gsm.numberOfPlayers;

        int numEnemies = (int)Mathf.Ceil((waveNumber * numPlayers) / 2f);

        enemiesRemaining = numEnemies;
        enemiesRemainingText.text = "Enemies Remaining: " + enemiesRemaining;

        List<EnemyMan> possibleEnemyPrefabs = ChooseEnemyDifficulty();

        int prefabIndex = Random.Range(0, possibleEnemyPrefabs.Count);
        foreach (var spawner in enemySpawners)
        {
            spawner.numEnemiesToSpawn = numEnemies / numSpawners;
            spawner.enemyPrefab = possibleEnemyPrefabs[prefabIndex];
        }

        for (int i = 0; i < numEnemies % numSpawners; i++)
        {
            enemySpawners[i].numEnemiesToSpawn += 1;
        }
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
        foreach (var spawner in bossSpawners)
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
            playersLost = true;
            return true;
        }

        if (waveNumber == roundsUntilWin)
        {
            return true;
        }

        return false;
    }

    private List<EnemyMan> ChooseEnemyDifficulty()
    {
        List<EnemyMan> enemies = possibleEasyEnemyPrefabs;

        float waveFactor = waveNumber / 5f;
        float chanceOfHardEnemies = Random.Range(0f, 1f) * waveFactor;

        if (chanceOfHardEnemies > 0.5)
        {
            enemies = possibleHardEnemyPrefabs;
        }

        return enemies;
    }
}
