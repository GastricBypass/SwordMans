using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : IEntity
{
    //public bool moveX = true;
    public Vector2 xRange;
    //public bool moveY = true;
    public Vector2 yRange;
    //public bool moveZ = true;
    public Vector2 zRange;

    public float waveTimeMS;

    public bool positiveFirst = true;

    private float difX;
    private float difY;
    private float difZ;

    private float avgX;
    private float avgY;
    private float avgZ;

    private System.DateTime levelStartTime;

    // Use this for initialization
    void Start ()
    {
        avgX = (xRange.x + xRange.y) / 2;
        avgY = (yRange.x + yRange.y) / 2;
        avgZ = (zRange.x + zRange.y) / 2;

        transform.position = new Vector3(avgX, avgY, avgZ);

        /*if (moveX)*/ difX = Mathf.Abs(xRange.x - xRange.y);
        /*if (moveY)*/ difY = Mathf.Abs(yRange.x - yRange.y);
        /*if (moveZ)*/ difZ = Mathf.Abs(zRange.x - zRange.y);

        if (!positiveFirst)
        {
            difX = -difX;
            difY = -difY;
            difZ = -difZ;
        }

        levelStartTime = System.DateTime.Now;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (active)
        {
            float x = transform.position.x;
            float y = transform.position.y;
            float z = transform.position.z;

            float timeDifSeconds = Time.timeSinceLevelLoad; // (float)(System.DateTime.Now - levelStartTime).TotalSeconds;

            // if you set moveX to false, it will accelerate in the x direction very quickly.
            /*if (moveX)*/ x = difX / 2 * Mathf.Sin(timeDifSeconds / (waveTimeMS / 1000)) + avgX;
            /*if (moveY)*/ y = difY / 2 * Mathf.Sin(timeDifSeconds / (waveTimeMS / 1000)) + avgY;
            /*if (moveZ)*/ z = difZ / 2 * Mathf.Sin(timeDifSeconds / (waveTimeMS / 1000)) + avgZ;

            transform.position = new Vector3(x, y, z);
        }
    }
}
