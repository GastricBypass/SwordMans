using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public List<GameObject> targets;
    public Vector3 offset = new Vector3(0, 6, -10);
    public float moveAmount = 1;
    public float preMoveDelay = 0.01f; // must be > 0 or else it causes a race condition with players being spawned and may not find players

    private bool shouldFollow = false;
    private bool targetsSet = false;

    private string targetBodyPart = "Body/Body Spine";
    // Use this for initialization
    void Start()
    {
        StartCoroutine(WaitToFollow(preMoveDelay));
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFollow && !targetsSet)
        {
            Man[] men = FindObjectsOfType<Man>();

            if (men.Length == 0)
            {
                StartCoroutine(WaitToFollow(preMoveDelay)); // if we did not find any players, wait again and try after waiting.
            }

            else
            {
                for (int i = 0; i < men.Length; i++)
                {
                    if (men[i].isCameraTarget)
                    {
                        targets.Add(men[i].transform.Find(targetBodyPart).gameObject);
                    }
                }
                targetsSet = true;
            }
        }

        if (targets.Count > 0)
        {
            SetCameraPosition();
        }
    }

    void SetCameraPosition()
    {
        Vector3 avgPos = new Vector3(0, 0, 0);

        float maxDist = 0;

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null)
            {
                continue; // should fix the null references when a man is destroyed
            }

            avgPos += targets[i].transform.position;

            for (int j = 0; j < targets.Count; j++)
            {
                float newDist = Mathf.Abs((targets[i].transform.position - targets[j].transform.position).magnitude);
                if (newDist > maxDist)
                {
                    maxDist = newDist;
                }
            }
        }

        avgPos = avgPos / targets.Count;

        Vector3 goalPosition = avgPos + offset + new Vector3(0, 0, -(maxDist / 2));
        Vector3 positionDif = goalPosition - transform.position;

        if (positionDif.magnitude < moveAmount)
        {
            transform.position = goalPosition;
        }
        else
        {
            transform.position = transform.position + positionDif.normalized * moveAmount;
        }
    }

    private IEnumerator WaitToFollow(float time)
    {
        shouldFollow = false;
        yield return new WaitForSeconds(time);
        shouldFollow = true;
    }

    public void Delete(Man man)
    {
        Transform toRemove = man.transform.Find(targetBodyPart);
        if (toRemove != null)
        {
            targets.Remove(toRemove.gameObject);
        }
    }
}
