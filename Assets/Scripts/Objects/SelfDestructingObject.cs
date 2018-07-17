using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructingObject : MonoBehaviour
{
    public float lifetime = 5; // seconds

    public bool destroyOnCollision = true;
    public bool destroyOnTriggerEnter = true;
    public bool onlyDestroyWhenHittingNonRigidbodies = true;

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
            if (onlyDestroyWhenHittingNonRigidbodies && FindParentRigidbody(collision.collider.transform) != null)
            {
                return;
            }

            Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (destroyOnTriggerEnter)
        {
            if (onlyDestroyWhenHittingNonRigidbodies && FindParentRigidbody(other.transform) != null)
            {
                return;
            }

            Destroy(this.gameObject);
        }
    }

    public Rigidbody FindParentRigidbody(Transform t)
    {
        Rigidbody rigbod = t.GetComponent<Rigidbody>();

        if (rigbod == null)
        {
            if (t.parent == null)
            {
                return null;
            }
            else
            {
                return FindParentRigidbody(t.parent); // recurse up the tree to find a rigidbody
            }
        }
        else
        {
            return rigbod;
        }
    }
}
