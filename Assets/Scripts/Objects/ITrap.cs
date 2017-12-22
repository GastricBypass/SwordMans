using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITrap : MonoBehaviour
{
    public bool isReady = true;
    public float readyTime = 0;

    public virtual void OnTriggerEnter(Collider other)
    {
        if (isReady && other.GetComponent<Rigidbody>() != null)
        {
            Activate();
        }
    }

    public virtual void Activate()
    {
        isReady = false;
        if (readyTime != 0)
        {
            StartCoroutine(WaitForReadyTime());
        }
    }

    public virtual void Ready()
    {
        isReady = true;
    }

    public virtual IEnumerator WaitForReadyTime()
    {
        yield return new WaitForSeconds(readyTime);
        Ready();
    }
}
