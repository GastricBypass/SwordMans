using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    public GameObject item;

    public Vector3 minRange;
    public Vector3 maxRange;

    public Vector3 initialVelocityMin;
    public Vector3 initialVelocityMax;

    public float timeBetweenSpawnsMinMS;
    public float timeBetweenSpawnsMaxMS;
    private bool canSpawn = true;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (canSpawn)
        {
            SpawnItem();
        }
	}

    public void SpawnItem()
    {
        float x = Random.Range(minRange.x, maxRange.x);
        float y = Random.Range(minRange.y, maxRange.y);
        float z = Random.Range(minRange.z, maxRange.z);

        float xMove = Random.Range(initialVelocityMin.x, initialVelocityMax.x);
        float yMove = Random.Range(initialVelocityMin.y, initialVelocityMax.y);
        float zMove = Random.Range(initialVelocityMin.z, initialVelocityMax.z);

        GameObject newlySpawned = Instantiate(item, new Vector3(x, y, z), Quaternion.identity);

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
}
