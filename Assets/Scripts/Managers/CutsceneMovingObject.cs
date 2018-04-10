using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneMovingObject : MonoBehaviour
{
    public bool goToNextSceneWhenOver;

    public bool moveObject = true;
    public bool rotateObject = true;
    public bool teleportToStartingPosition = true;

    public List<Transform> positions;
    public List<float> durationsToStayAtPosition;
    public List<float> moveSpeedToNextPosition;
    public List<float> rotationSpeedToNextPosition;

    private int index;

    private bool waitingAtPosition;

    // Use this for initialization
    void Start()
    {
        if (teleportToStartingPosition)
        {
            SetTransform(positions[0]);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (waitingAtPosition)
        {
            return;
        }

        if ((!moveObject || Mathf.Abs((this.transform.position - positions[index].transform.position).magnitude) < 0.1) // if it's close enough in the position
            && (!rotateObject || Mathf.Abs((this.transform.rotation.eulerAngles - positions[index].transform.rotation.eulerAngles).magnitude) < 0.1)) // if it's close enough in the rotation
        {
            StartCoroutine(WaitToSelectNewTargetPosition(durationsToStayAtPosition[index]));
        }
        else
        {
            Vector3 newPosition = CalculateNewPosition();
            Vector3 newRotation = CalculateNewRotation();
            SetTransform(newPosition, Quaternion.Euler(newRotation));
        }
    }

    public IEnumerator WaitToSelectNewTargetPosition(float duration)
    {
        waitingAtPosition = true;
        yield return new WaitForSeconds(duration);

        waitingAtPosition = false;
        
        if (index < positions.Count - 1)
        {
            index++;
        }
        else if (goToNextSceneWhenOver)
        {
            FindObjectOfType<CampaignUIManager>().LoadNextLevel();
        }
    }

    private Vector3 CalculateNewPosition()
    {
        Vector3 positionDirection = (positions[index].position - this.transform.position).normalized;
        Vector3 dPosition = positionDirection * moveSpeedToNextPosition[index] * Time.deltaTime;
        Vector3 newPosition = this.transform.position + dPosition;

        if ((this.transform.position - positions[index].position).magnitude < dPosition.magnitude)
        {
            newPosition = positions[index].position;
        }

        return newPosition;
    }

    private Vector3 CalculateNewRotation()
    {
        Vector3 rotationDirection = CalculateRotationDirection();
        Vector3 dRotation = rotationDirection * rotationSpeedToNextPosition[index] * Time.deltaTime;
        Vector3 newRotation = this.transform.rotation.eulerAngles + dRotation; // Vector3.RotateTowards(this.transform.rotation.eulerAngles, positions[index].rotation.eulerAngles, rotationSpeedToNextPosition[index] * Time.deltaTime, rotationSpeedToNextPosition[index] * Time.deltaTime);

        //if ((newRotation - positions[index].rotation.eulerAngles).magnitude > (this.transform.rotation.eulerAngles - positions[index].rotation.eulerAngles).magnitude)
        //{
        //    newRotation = this.transform.rotation.eulerAngles - dRotation;
        //}

        if ((this.transform.rotation.eulerAngles - positions[index].rotation.eulerAngles).magnitude < dRotation.magnitude)
        {
            newRotation = positions[index].rotation.eulerAngles;
        }

        return newRotation;
    }

    private Vector3 CalculateRotationDirection()
    {
        float x = CalculateShortestDifferenceForAngle(this.transform.rotation.eulerAngles.x, positions[index].rotation.eulerAngles.x);
        float y = CalculateShortestDifferenceForAngle(this.transform.rotation.eulerAngles.y, positions[index].rotation.eulerAngles.y);
        float z = CalculateShortestDifferenceForAngle(this.transform.rotation.eulerAngles.z, positions[index].rotation.eulerAngles.z);

        return new Vector3(x, y, z).normalized;
    }

    private float CalculateShortestDifferenceForAngle(float startingAngle, float targetAngle) // Angles between 0 and 360
    {
        float difference = targetAngle - startingAngle;

        if (difference > 180)
        {
            difference = -1 * (360 - difference);
        }
        else if (difference < - 180)
        {
            difference = 360 - difference;
        }

        return difference;
    }

    public void SetTransform(Vector3 newPosition, Quaternion newRotation)
    {
        if (moveObject)
        {
            this.transform.position = newPosition;
        }

        if (rotateObject)
        {
            this.transform.rotation = newRotation;
        }
    }

    public void SetTransform(Transform newTransform)
    {
        SetTransform(newTransform.position, newTransform.rotation);
    }
}
