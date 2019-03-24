using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float maxLength = 5000;
    public Vector3 raycastOffset;

    private LineRenderer lineRenderer;
    private RaycastHit rayHit;
    private Vector3 lineRenderEndPoint;
    
    void Start ()
    {
        LineRenderer laser = this.GetComponent<LineRenderer>();
        if (laser != null)
        {
            lineRenderer = laser;
        }
        else
        {
            lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        }

        lineRenderEndPoint = lineRenderer.GetPosition(1);
    }
	
	void Update ()
    {
        if (Physics.Raycast(transform.position + raycastOffset, -transform.forward, out rayHit))
        {
            if (rayHit.collider)
            {
                lineRenderer.SetPosition(1, new Vector3(lineRenderEndPoint.x, lineRenderEndPoint.y, -rayHit.distance));
            }
        }
        else
        {
            lineRenderer.SetPosition(1, new Vector3(lineRenderEndPoint.x, lineRenderEndPoint.y, -maxLength));
        }
	}
}
