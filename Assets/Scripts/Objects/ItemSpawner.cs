using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : IEntity {

    public List<GameObject> items;

    public Vector3 minRange;
    public Vector3 maxRange;

    public Vector3 initialVelocityMin;
    public Vector3 initialVelocityMax;

    public float timeBetweenSpawnsMinMS;
    public float timeBetweenSpawnsMaxMS;
    public bool destroyOldSpawnsOnNewSpawn = false;
    public float delayOldSpawnDestroy = 0;
    public bool spawnRelativeToSpawner = false;
    public bool spawnOnLoad = false;

    private bool canSpawn = true;

    private GameObject previousSpawn;

    // Use this for initialization
    void Start ()
    {
		if (!spawnOnLoad && active)
        {
            StartCoroutine(ToggleCanSpawn());
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (canSpawn && active)
        {
            SpawnItem();
        }
	}

    public void SpawnItem()
    {
        if (destroyOldSpawnsOnNewSpawn && previousSpawn != null)
        {
            StartCoroutine(WaitToDestroySpawn(previousSpawn.gameObject, delayOldSpawnDestroy));
        }

        float x = Random.Range(minRange.x, maxRange.x);
        float y = Random.Range(minRange.y, maxRange.y);
        float z = Random.Range(minRange.z, maxRange.z);

        Vector3 spawnPosition = new Vector3(x, y, z);

        float xMove = Random.Range(initialVelocityMin.x, initialVelocityMax.x);
        float yMove = Random.Range(initialVelocityMin.y, initialVelocityMax.y);
        float zMove = Random.Range(initialVelocityMin.z, initialVelocityMax.z);

        int itemIndex = Random.Range(0, items.Count);

        if (spawnRelativeToSpawner)
        {
            spawnPosition = spawnPosition + this.transform.position;
        }

        GameObject newlySpawned = Instantiate(items[itemIndex], spawnPosition, items[itemIndex].transform.rotation);

        if (destroyOldSpawnsOnNewSpawn)
        {
            previousSpawn = newlySpawned;
        }

        Rigidbody newRigidbody = newlySpawned.GetComponent<Rigidbody>();
        if (newRigidbody != null)
        {
            newRigidbody.velocity = new Vector3(xMove, yMove, zMove);
        }
        StartCoroutine(ToggleCanSpawn());
    }

    public IEnumerator ToggleCanSpawn()
    {
        canSpawn = false;
        float timeToWaitMS = Random.Range(timeBetweenSpawnsMinMS, timeBetweenSpawnsMaxMS);
        yield return new WaitForSeconds(timeToWaitMS / 1000);
        canSpawn = true;
    }

    public IEnumerator WaitToDestroySpawn(GameObject toDestroy, float time)
    {
        Pickup pickup = toDestroy.GetComponent<Pickup>();
        if (pickup != null)
        {
            pickup.Disable();
        }

        yield return new WaitForSeconds(time);
        Destroy(toDestroy.gameObject);
    }
}
