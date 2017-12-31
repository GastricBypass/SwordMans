using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : IEntity {

    public EnemyMan enemyPrefab;
    
    public int numEnemiesToSpawn;
    public float spawnInterval;

    public bool readyToSpawn = true;

	// Use this for initialization
	void Start ()
    {
        readyToSpawn = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (active && numEnemiesToSpawn > 0 && readyToSpawn)
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        readyToSpawn = false;

        StartCoroutine(WaitToReadySpawn());

        EnemyMan newEnemy = Instantiate(enemyPrefab, this.transform) as EnemyMan;
        numEnemiesToSpawn--;
    }

    public IEnumerator WaitToReadySpawn()
    {
        yield return new WaitForSeconds(spawnInterval);

        readyToSpawn = true;
    }
}
