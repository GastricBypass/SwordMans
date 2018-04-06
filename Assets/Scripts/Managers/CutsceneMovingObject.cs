using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneMovingObject : MonoBehaviour
{
    public bool goToNextSceneWhenOver;

    public bool moveObject = true;
    public bool rotateObject = true;

    public List<Transform> positions;
    public List<float> durationsToStayAtPosition;
    public List<float> moveSpeedToNextPosition;
    public List<float> rotationSpeedToNextPosition;

    private int index;

    private bool waitingAtPosition;

    // Use this for initialization
    void Start()
    {
        SetTransform(positions[0]);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (waitingAtPosition)
        {
            return;
        }

        if ((!moveObject || this.transform.position == positions[index].transform.position) && (!rotateObject || this.transform.rotation == positions[index].transform.rotation))
        {
            StartCoroutine(WaitToSelectNewTargetPosition(durationsToStayAtPosition[index]));
        }
        else
        {
            Vector3 positionDirection = (positions[index].position - this.transform.position).normalized;
            Vector3 rotationDirection = (positions[index].rotation.eulerAngles - this.transform.rotation.eulerAngles).normalized;
            
            Vector3 dPosition = positionDirection * moveSpeedToNextPosition[index] * Time.deltaTime;
            Vector3 dRotation = rotationDirection * rotationSpeedToNextPosition[index] * Time.deltaTime;

            Vector3 newPosition = this.transform.position + dPosition;
            Vector3 newRotation = this.transform.rotation.eulerAngles + dRotation;

            if ((newRotation - positions[index].rotation.eulerAngles).magnitude > (this.transform.rotation.eulerAngles - positions[index].rotation.eulerAngles).magnitude)
            {
                newRotation = this.transform.rotation.eulerAngles - dRotation;
            }

            if ((this.transform.position - positions[index].position).magnitude < dPosition.magnitude)
            {
                newPosition = positions[index].position;
            }
            if ((this.transform.rotation.eulerAngles - positions[index].rotation.eulerAngles).magnitude < dRotation.magnitude)
            {
                newRotation = positions[index].rotation.eulerAngles;
            }

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
