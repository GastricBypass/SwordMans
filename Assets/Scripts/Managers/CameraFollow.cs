using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public List<GameObject> targets;
    public Vector3 offset = new Vector3(0, 6, -10);
    public float moveAmount = 1;

    private string targetBodyPart = "Body/Body Spine";
    // Use this for initialization
    void Start () {
        Man[] men = FindObjectsOfType<Man>();

        for (int i = 0; i < men.Length; i++)
        {
            if (men[i].isCameraTarget)
            {
                targets.Add(men[i].transform.Find(targetBodyPart).gameObject);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {

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

    public void Delete(Man man)
    {
        Transform toRemove = man.transform.Find(targetBodyPart);
        if (toRemove != null)
        {
            targets.Remove(toRemove.gameObject);
        }
    }
}
