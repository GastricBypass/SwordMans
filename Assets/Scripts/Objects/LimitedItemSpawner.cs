using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedItemSpawner : IEntity
{
    public GameObject itemPrefab;

    public bool destroyOldSpawnsOnNewSpawn = false;

    public int numItemsToSpawn;
    public float spawnInterval;

    public bool readyToSpawn = true;

    private GameObject previousSpawn;

    // Use this for initialization
    void Start()
    {
        readyToSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (active && numItemsToSpawn > 0 && readyToSpawn)
        {
            SpawnItem();
        }
    }

    public void SpawnItem()
    {
        if (destroyOldSpawnsOnNewSpawn && previousSpawn != null)
        {
            Destroy(previousSpawn.gameObject);
        }

        readyToSpawn = false;

        StartCoroutine(WaitToReadySpawn());

        GameObject newItem = Instantiate(itemPrefab, this.transform) as GameObject;
        newItem.transform.position = this.transform.position;
        //newItem.transform.rotation = this.transform.rotation;

        previousSpawn = newItem;
        numItemsToSpawn--;
    }

    public IEnumerator WaitToReadySpawn()
    {
        yield return new WaitForSeconds(spawnInterval);

        readyToSpawn = true;
    }
}
