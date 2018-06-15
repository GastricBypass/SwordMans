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

    public bool relativePosition;

    protected float difX;
    protected float difY;
    protected float difZ;

    protected float avgX;
    protected float avgY;
    protected float avgZ;

    protected bool waitingToActivate = false;

    //private System.DateTime levelStartTime;

    // Use this for initialization
    void Start ()
    {
        if (relativePosition)
        {
            xRange.x += transform.position.x;
            xRange.y += transform.position.x;
            yRange.x += transform.position.y;
            yRange.y += transform.position.y;
            zRange.x += transform.position.z;
            zRange.y += transform.position.z;
        }

        avgX = (xRange.x + xRange.y) / 2;
        avgY = (yRange.x + yRange.y) / 2;
        avgZ = (zRange.x + zRange.y) / 2;

        if (active)
        {
            SetStartingPosition();
        }

        /*if (moveX)*/ difX = Mathf.Abs(xRange.x - xRange.y);
        /*if (moveY)*/ difY = Mathf.Abs(yRange.x - yRange.y);
        /*if (moveZ)*/ difZ = Mathf.Abs(zRange.x - zRange.y);

        if (!positiveFirst)
        {
            difX = -difX;
            difY = -difY;
            difZ = -difZ;
        }

        //levelStartTime = System.DateTime.Now;
    }

    public override void Activate() // Waits until the oscillation is at a position close enough to the actual transform to start
    {
        waitingToActivate = true;
        base.Activate();
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

            if (waitingToActivate)
            {
                //float stepSize = new Vector3(difX, difY, difZ).magnitude / (timeDifSeconds / (waveTimeMS / 1000));
                if ((transform.position - new Vector3(x, y, z)).magnitude < new Vector3(0.1f, 0.1f, 0.1f).magnitude)
                {
                    waitingToActivate = false;
                }
            }
            else
            {
                transform.position = new Vector3(x, y, z);
            }
        }
    }

    protected virtual void SetStartingPosition()
    {
        transform.position = new Vector3(avgX, avgY, avgZ);
    }
}
