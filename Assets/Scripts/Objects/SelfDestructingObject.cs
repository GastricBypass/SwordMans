using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructingObject : MonoBehaviour
{
    public float lifetime = 5; // seconds

    public bool destroyOnCollision = true;

	// Use this for initialization
	void Start()
    {
        if (lifetime > 0)
        {
            StartCoroutine(DestroyAfterTime(lifetime));
        }
    }
	
    public IEnumerator DestroyAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Destroy(this.gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (destroyOnCollision)
        {
            Destroy(this.gameObject);
        }
    }
}
