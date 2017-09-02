using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomForceAdder : MonoBehaviour {

    public Vector3 minForce;
    public Vector3 maxForce;
    public Vector2 moveTimeRange;
    public Vector2 downTimeRange;

    private Rigidbody rigbod;

    private bool shouldMove = true;
    private bool moving = false;
    Vector3 forceToAdd;

    // Use this for initialization
    void Start ()
    {
        rigbod = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void FixedUpdate()
    {
        if (shouldMove)
        {
            forceToAdd = new Vector3(
                Random.Range(minForce.x, maxForce.x),
                Random.Range(minForce.y, maxForce.y),
                Random.Range(minForce.z, maxForce.z)
            );
            StartCoroutine(SetMovingForTime(Random.Range(moveTimeRange.x, moveTimeRange.y)));
        }

        if (moving)
        {
            rigbod.AddForce(forceToAdd);
        }
    }

    private IEnumerator SetMovingForTime(float time)
    {
        shouldMove = false;
        moving = true;
        yield return new WaitForSeconds(time);
        moving = false;
        StartCoroutine(WaitForTime(Random.Range(downTimeRange.x, downTimeRange.y)));
    }

    private IEnumerator WaitForTime(float time)
    {
        yield return new WaitForSeconds(time);
        shouldMove = true;
    }
}
