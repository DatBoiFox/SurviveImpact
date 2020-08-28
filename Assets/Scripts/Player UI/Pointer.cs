using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour
{
    public float defaultPointerLenght = 1.0f;
    public GameObject dot;
    public VRInputModule inputModule;

    private LineRenderer lineRenderer = null;


    private void Start()
    {

        lineRenderer = GetComponent<LineRenderer>();
    }


    private void Update()
    {
        DrawLine();
        
    }

    private void DrawLine()
    {

        PointerEventData data = inputModule.GetData();

        float targetLenght = data.pointerCurrentRaycast.distance == 0 ? defaultPointerLenght : data.pointerCurrentRaycast.distance;

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, targetLenght);

        Vector3 endPoint = transform.position + (transform.forward * targetLenght);

        if (hit.collider != null)
        {
            endPoint = hit.point;
        }

        dot.transform.position = endPoint;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPoint);
    }

}
