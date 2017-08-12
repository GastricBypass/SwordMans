using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public Man enemyPrefab;

    public int maxEnemies;
    public int numEnemies;
    public float spawnIntervalMS;

    private System.DateTime lastSpawn;

	// Use this for initialization
	void Start ()
    {
        lastSpawn = System.DateTime.Now;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (numEnemies < maxEnemies && (System.DateTime.Now - lastSpawn).TotalMilliseconds > spawnIntervalMS)
        {
            SpawnEnemy();
        }
	}

    void SpawnEnemy()
    {
        //Man newEnemy = Instantiate(enemyPrefab, this.transform) as Man; // throwing warning. Needs to be uncommented for functionality
        lastSpawn = System.DateTime.Now;
        numEnemies++;
    }
}
