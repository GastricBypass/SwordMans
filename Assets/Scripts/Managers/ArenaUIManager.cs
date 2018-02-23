using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ArenaUIManager : UIManager
{
    public GameObject gameOverScreen;
    public Slider bossHealthBar;
    public Text bossHealthValue;

    public Text gameOverReport;

    public Button retryButton;

    public int roundsPerBossWave = 5;

    public List<EnemyMan> possibleEasyEnemyPrefabs;
    public List<EnemyMan> possibleHardEnemyPrefabs;
    public List<EnemyMan> possibleBossPrefabs;
    public List<EnemySpawner> enemySpawners;
    public List<EnemySpawner> bossSpawners;
    public Text enemiesRemainingText;
    public Text waveNumberText;

    public List<LimitedItemSpawner> heartSpawners;

    private int waveNumber = 0;
    private int enemiesRemaining = 0;
    private int totalEnemiesKilled = 0;
    private int totalGoldEarned = 0;

    private float bossMaxHealth;

    public override void Start()
    {
        CommonStart();

        gsm.numberOfAIPlayers = 0;

        gameOverScreen.SetActive(false);

        RectTransform gameOverBackdrop = gameOverScreen.transform.Find("Backdrop").GetComponent<RectTransform>();
        gameOverBackdrop.sizeDelta = new Vector2(Screen.width * 2, Screen.height * 2);

        gsm.SpawnPlayers();
        SetHealthBars();

        StartNextWave();
    }

    public void StartNextWave()
    {
        if (waveNumber > 0)
        { 
            gsm.data.AddGold(waveNumber + gsm.numberOfPlayers - 1);
            totalGoldEarned += waveNumber + gsm.numberOfPlayers - 1;
        }

        waveNumber++;
        waveNumberText.text = "Wave " + waveNumber + ":";

        if (waveNumber % roundsPerBossWave == 0)
        {
            StartBossWave();
        }
        else
        {
            StartNormalWave();
        }

        StartCoroutine(WaitToStartNextRound());
    }

    public void StartBossWave()
    {
        foreach (var spawner in bossSpawners)
        {
            spawner.active = false;
        }

        int numSpawners = bossSpawners.Count;
        int numPlayers = gsm.numberOfPlayers;

        int numBosses = (int)Mathf.Ceil((waveNumber * numPlayers) / 20f);

        enemiesRemaining = numBosses;
        enemiesRemainingText.text = "Enemies Remaining: " + enemiesRemaining;

        int prefabIndex = Random.Range(0, possibleBossPrefabs.Count);
        foreach (var spawner in bossSpawners)
        {
            spawner.numEnemiesToSpawn = numBosses / numSpawners;
            spawner.enemyPrefab = possibleBossPrefabs[prefabIndex];
            spawner.healthOverride = possibleBossPrefabs[prefabIndex].health + (possibleBossPrefabs[prefabIndex].health * numPlayers / 2f); // Should be unnecessary with boss EnemyMan changes
            
            bossMaxHealth = spawner.healthOverride;

            bossHealthBar.gameObject.SetActive(true);
            bossHealthBar.maxValue = spawner.healthOverride;
            bossHealthBar.value = spawner.healthOverride;
            ChangeHealth(1, 0);
        }

        for (int i = 0; i < numBosses % numSpawners; i++)
        {
            bossSpawners[i].numEnemiesToSpawn += 1;
        }
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

        if (waveNumber % roundsPerBossWave == 1)
        {
            for (int i = 0; i < numPlayers; i++)
            {
                heartSpawners[i].numItemsToSpawn += 1;
                
            }
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

    public override void ChangeHealth(float percent, int playerNumber)
    {
        base.ChangeHealth(percent, playerNumber);
        if (waveNumber % roundsPerBossWave == 0 && playerNumber == 0)
        {
            bossHealthBar.value = percent * bossMaxHealth;
            bossHealthValue.text = (Mathf.Ceil(bossHealthBar.value) + " / " + bossMaxHealth);
        }
    }

    public override void ShowHurtImage(int playerNumber, float damage)
    {
        base.ShowHurtImage(playerNumber, damage);
        if (waveNumber % roundsPerBossWave == 0 && (playerNumber == 0 || playerNumber > 4))
        {
            StartCoroutine(ShowHurtImageForSeconds(hurtDisplays[4], damage, 0.2f));
            StartCoroutine(ShowHurtTextForSeconds(damageTexts[4], damage, 0.5f));
        }
    }

    public void DisplayGameOverOptions()
    {
        Time.timeScale = 0;
        gsm.SetCursor(true);

        gameOverReport.text = "Survived until wave " + waveNumber + "\n\nEnemies defeated: " + totalEnemiesKilled + "\nGold earned: " + totalGoldEarned;
        gameOverScreen.SetActive(true);
        retryButton.Select();
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
