using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructingObject : MonoBehaviour
{
    public float lifetime = 5; // seconds

	// Use this for initialization
	void Start()
    {
        StartCoroutine(DestroyAfterTime(lifetime));
    }
	
    public IEnumerator DestroyAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Destroy(this.gameObject);
    }
}
