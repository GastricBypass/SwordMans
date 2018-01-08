using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : IEntity {

    public EnemyMan enemyPrefab;

    public float aggroDistanceOverride = 0;
    public float healthOverride = 0;
    
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
        newEnemy.transform.position = this.transform.position;
        newEnemy.transform.rotation = this.transform.rotation;

        if (aggroDistanceOverride != 0)
        {
            AIEnemy[] weapons = newEnemy.gameObject.GetComponentsInChildren<AIEnemy>(true);
            foreach (var weapon in weapons)
            {
                weapon.aggroDistance = aggroDistanceOverride;
            }
        }

        if (healthOverride != 0)
        {
            newEnemy.health = healthOverride;
            newEnemy.maxHealth = healthOverride;
        }
        numEnemiesToSpawn--;
    }

    public IEnumerator WaitToReadySpawn()
    {
        yield return new WaitForSeconds(spawnInterval);

        readyToSpawn = true;
    }
}
