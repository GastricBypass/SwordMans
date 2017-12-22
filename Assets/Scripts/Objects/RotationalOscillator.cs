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

            transform.rotation = Quaternion.Euler(new Vector3(x, y, z));
        }
    }
}
