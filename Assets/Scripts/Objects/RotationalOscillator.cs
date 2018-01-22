using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationalOscillator : Oscillator
{

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active)
        {
            float x = transform.position.x;
            float y = transform.position.y;
            float z = transform.position.z;

            float timeDifSeconds = Time.timeSinceLevelLoad; // (float)(System.DateTime.Now - levelStartTime).TotalSeconds;

            // if you set moveX to false, it will accelerate in the x direction very quickly.
            /*if (moveX)*/
            x = difX / 2 * Mathf.Sin(timeDifSeconds / (waveTimeMS / 1000)) + avgX;
            /*if (moveY)*/
            y = difY / 2 * Mathf.Sin(timeDifSeconds / (waveTimeMS / 1000)) + avgY;
            /*if (moveZ)*/
            z = difZ / 2 * Mathf.Sin(timeDifSeconds / (waveTimeMS / 1000)) + avgZ;

            if (waitingToActivate)
            {
                //float stepSize = new Vector3(difX, difY, difZ).magnitude / (timeDifSeconds / (waveTimeMS / 1000));
                if ((new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z) - new Vector3(x, y, z)).magnitude < new Vector3(0.1f, 0.1f, 0.1f).magnitude)
                {
                    waitingToActivate = false;
                }
            }
            else
            {
                transform.rotation = Quaternion.Euler(new Vector3(x, y, z));
            }
        }
    }

    protected override void SetStartingPosition()
    {
        transform.rotation = Quaternion.Euler(new Vector3(avgX, avgY, avgZ));
    }
}
