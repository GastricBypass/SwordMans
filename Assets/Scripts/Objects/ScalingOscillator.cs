using UnityEngine;

public class ScalingOscillator : Oscillator
{
    void FixedUpdate()
    {
        if (active)
        {
            float x = transform.position.x;
            float y = transform.position.y;
            float z = transform.position.z;

            float timeDifSeconds = Time.timeSinceLevelLoad;

            x = difX / 2 * Mathf.Sin(timeDifSeconds / (waveTimeMS / 1000)) + avgX;
            y = difY / 2 * Mathf.Sin(timeDifSeconds / (waveTimeMS / 1000)) + avgY;
            z = difZ / 2 * Mathf.Sin(timeDifSeconds / (waveTimeMS / 1000)) + avgZ;

            if (waitingToActivate)
            {
                //float stepSize = new Vector3(difX, difY, difZ).magnitude / (timeDifSeconds / (waveTimeMS / 1000));
                if ((new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z) - new Vector3(x, y, z)).magnitude < new Vector3(0.1f, 0.1f, 0.1f).magnitude)
                {
                    waitingToActivate = false;
                }
            }
            else
            {
                transform.localScale = new Vector3(x, y, z);
            }
        }
    }

    protected override void SetStartingPosition()
    {
        transform.localScale = new Vector3(avgX, avgY, avgZ);
    }
}
