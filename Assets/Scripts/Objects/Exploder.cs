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

            SetupPusher(explosion);
            SetupSelfDestructer(explosion);

            //StartCoroutine(Utilities.DestroyAfterTime(explosion.gameObject, explosionDuration));

            //Collider collider = this.GetComponent<Collider>();
            //if (collider != null)
            //{
            //    collider.isTrigger = true;
            //}
            //MeshRenderer mesh = this.GetComponent<MeshRenderer>();
            //if (mesh != null)
            //{
            //    mesh.enabled = false;
            //}
            Destroy(this.gameObject);
        }
    }

    public void SetupPusher(DamagingArea explosion)
    {
        RadialPushingArea pusher = explosion.gameObject.GetComponent<RadialPushingArea>();
        if (pusher == null)
        {
            pusher = explosion.gameObject.AddComponent<RadialPushingArea>();
        }
        pusher.pushMagnitude = pushForce;
    }

    public void SetupSelfDestructer(DamagingArea explosion)
    {
        SelfDestructingObject destructer = explosion.gameObject.GetComponent<SelfDestructingObject>();
        if (destructer == null)
        {
            destructer = explosion.gameObject.AddComponent<SelfDestructingObject>();
        }
        destructer.lifetime = explosionDuration;
    }

    //public IEnumerator DestroyAfterTime(GameObject toDestroy, float time)
    //{
    //    Disable();
    //    yield return new WaitForSeconds(time);
    //
    //    Destroy(toDestroy);
    //    Destroy(this.gameObject);
    //}
    //
    //public void Disable()
    //{
    //    
    //}
}
