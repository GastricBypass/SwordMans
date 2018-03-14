using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    public DamagingArea explosionPrefab;
    public float explosionDuration;

    public float pushForce;

    public float damagePerTick;
    public float tickLength;

    private bool exploded;

    public void OnCollisionEnter(Collision collision)
    {
        if (!exploded)
        {
            exploded = true;

            DamagingArea explosion = Instantiate(explosionPrefab);
            explosion.transform.position = this.transform.position;

            explosion.damagePerTick = damagePerTick;
            explosion.tickLengthMS = tickLength * 1000f;

            RadialPushingArea pusher = this.gameObject.GetComponent<RadialPushingArea>();
            if (pusher == null)
            { 
                pusher = explosion.gameObject.AddComponent<RadialPushingArea>();
            }
            pusher.pushMagnitude = pushForce;

            StartCoroutine(DestroyAfterTime(explosion.gameObject, explosionDuration));

            Collider collider = this.GetComponent<Collider>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }
            MeshRenderer mesh = this.GetComponent<MeshRenderer>();
            if (mesh != null)
            {
                mesh.enabled = false;
            }
        }
    }

    public IEnumerator DestroyAfterTime(GameObject toDestroy, float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(toDestroy);
        Destroy(this.gameObject);
    }
}
