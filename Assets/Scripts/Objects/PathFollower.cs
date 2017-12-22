using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : IEntity
{
    public List<Transform> waypoints;
    public float speed = 1;
    public bool reverse = false;

    public bool rotateWithMotion = false;
    public Vector3 upVector = new Vector3(0, 1, 0);

    public bool rotateSmoothly = false;
    public float rotateSpeed = 1;

    private Transform currentWaypoint;
    private Quaternion targetRotation;
    private int waypointIndex = 0;
    
	public void Start ()
    {
        currentWaypoint = waypoints[waypointIndex];
    }
	
	public void FixedUpdate ()
    {
        if (!active)
        {
            return;
        }

        Vector3 direction = (currentWaypoint.position - this.transform.position).normalized;
        Vector3 dPosition = direction * speed * Time.deltaTime;
        
        this.transform.position += dPosition;

        if (rotateWithMotion)
        {
            RotateToDirection(direction);
        }

        if ((this.transform.position - currentWaypoint.transform.position).magnitude < dPosition.magnitude)
        {
            this.transform.position = currentWaypoint.transform.position;
            SetNewCurrentWaypoint();
        }
    }

    private void SetNewCurrentWaypoint()
    {
        if (reverse)
        {
            waypointIndex--;
            if (waypointIndex < 0)
            {
                waypointIndex = waypoints.Count - 1;
            }
        }
        else
        {
            waypointIndex++;
            if (waypointIndex > waypoints.Count - 1)
            {
                waypointIndex = 0;
            }
        }

        currentWaypoint = waypoints[waypointIndex];
    }

    private void RotateToDirection(Vector3 direction)
    {
        if (rotateSmoothly)
        {
            Quaternion currentRotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(direction), rotateSpeed * Time.deltaTime);
            this.transform.rotation = currentRotation;
        }
        else
        {
            this.transform.rotation = Quaternion.LookRotation(direction, upVector);
        }
    }
}
