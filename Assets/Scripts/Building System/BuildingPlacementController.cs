using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class BuildingPlacementController : MonoBehaviour
{

    public SteamVR_Input_Sources handType;
    public SteamVR_Input_Sources rotateHand;

    public SteamVR_Behaviour_Pose controllerPose;

    // Building Prev. and confirmation
    public SteamVR_Action_Single buildAction;
    public SteamVR_Action_Boolean confirmBuild;

    //Building rotation
    public SteamVR_Action_Boolean rotateLeft;
    public SteamVR_Action_Boolean rotateRight;

    // currently displaying object before building
    private GameObject currentObj;

    // rotation angle
    private float rotation;

    // Wich of the rotation buttons active
    private bool leftActive;
    private bool rightActive;

    [SerializeField]
    private HandGrab hand;

    private bool objectPrevActive;

    // Layers filter to ignore raycast on itself and on hands
    public LayerMask layers;


    // Start is called before the first frame update
    void Start()
    {
        rotation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (hand.objectInHand == null)
            return;
        if (hand.objectInHand.GetComponent<Buildable>())
        {

            if (rotateLeft.GetLastStateDown(rotateHand))
            {
                leftActive = true;
            }
            else if (rotateLeft.GetLastStateUp(rotateHand))
            {
                leftActive = false;
            }

            if (rotateRight.GetLastStateDown(rotateHand))
            {
                rightActive = true;
            }
            else if (rotateRight.GetLastStateUp(rotateHand))
            {
                rightActive = false;
            }
            objectPrevActive = buildAction.GetAxis(handType) > 0 ? true : false;
            Act();
            if (currentObj != null)
            {
                MoveObject();
                RotateObject();
                Build();
            }
        }
    }


    //Function that instantiates new object if trigger pressed. IF trigger pressed and then released, object is destroyed
    private void Act()
    {
        if (objectPrevActive)
        {
            if (currentObj == null)
            {
                currentObj = Instantiate(hand.objectInHand.GetComponent<Buildable>().prefab);
            }
        }
        else if (!objectPrevActive)
        {
            Destroy(currentObj);
        }
    }
    private void RotateObject()
    {
        Debug.Log("Rotate");
        if (leftActive)
            rotation += 2f * Time.deltaTime;
        else if (rightActive)
            rotation -= 2f * Time.deltaTime;

        currentObj.transform.Rotate(Vector3.up, rotation * 20);
    }
    private void MoveObject()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 10f, layers))
        {
            currentObj.transform.position = hit.point;
            currentObj.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
    }
    //Builds object
    private void Build()
    {
        if (confirmBuild.GetLastStateUp(handType))
        {
            Debug.Log("AA");
            if (currentObj != null)
            {
                if (currentObj.transform.childCount > 0)
                {
                    foreach (Collider collider in currentObj.transform.GetChild(0).gameObject.GetComponents<Collider>())
                        collider.enabled = true;
                }
                foreach (Collider collider in currentObj.transform.gameObject.GetComponents<Collider>())
                    collider.enabled = true;
                if (currentObj.GetComponent<Sapling>())
                {
                    currentObj.GetComponent<Sapling>().placed = true;
                }
                currentObj = null;
                rotation = 0;
                Destroy(hand.objectInHand.gameObject, .001f);
            }


        }
    }
}
