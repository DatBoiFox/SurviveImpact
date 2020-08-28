using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandGrab : MonoBehaviour
{

    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean triggerAction;
    public SteamVR_Action_Boolean toglePointerAction;

    // Object that collides with the hand
    public GameObject collidingObject; // 1
    // Object that is currently in the hand
    public GameObject objectInHand; // 2

    public int grabForce;

    public GameObject controllerModel;


    public bool fingerDown = false;

    private void Update()
    {
        // 1
        if (grabAction.GetLastStateDown(handType))
        {
            if (collidingObject)
            {
                if (collidingObject.tag == "BuildingMaterial" || collidingObject.tag == "Pickable" || collidingObject.tag == "Magazine")
                {
                    GrabObject();
                }
            }
            
        }

        // 2
        if (grabAction.GetLastStateUp(handType))
        {
            if (objectInHand)
            {
                ReleaseObject();
            }
        }

        if (triggerAction.GetLastStateDown(handType))
        {
            fingerDown = true;
            if (objectInHand)
            {
                //if (objectInHand.GetComponent<Firearm>())
                //    objectInHand.GetComponent<Firearm>().Shoot();
                if (objectInHand.GetComponent<Firearm>())
                    objectInHand.GetComponent<Firearm>().canShoot = true;
                if (objectInHand.GetComponent<Tool>())
                    objectInHand.GetComponent<Tool>().DoToolStuff();
            }
                
        }
        if (triggerAction.GetLastStateUp(handType))
        {
            fingerDown = false;
        }

        if (fingerDown)
        {
            if (objectInHand)
            {
                if (objectInHand.GetComponent<Firearm>())
                    objectInHand.GetComponent<Firearm>().Shoot();
            }
        }

        if(objectInHand != null)
        {
            FindObjectOfType<SaveManager>().evaluateSceneItemDifference(objectInHand.gameObject);
        }
    }

    private void SetCollidingObject(Collider col)
    {
        // 1
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        // 2
        collidingObject = col.gameObject;
    }
    // 1
    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    // 2
    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    // 3
    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }

        collidingObject = null;
    }

    // 3
    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = grabForce;
        fx.breakTorque = grabForce;
        return fx;
    }

    public void GrabObject()
    {
        // 1
        objectInHand = collidingObject;
        if (objectInHand.GetComponent<Item>())
        {
            //Debug.Log(this.handType);
            if (this.handType == SteamVR_Input_Sources.LeftHand)
                objectInHand.GetComponent<Item>().LHand = this.gameObject;
            else if (this.handType == SteamVR_Input_Sources.RightHand)
                objectInHand.GetComponent<Item>().RHand = this.gameObject;
            try
            {
                objectInHand.GetComponent<Item>().PickUp(this.gameObject);
            }
            catch
            {

            }

        }
        
        collidingObject = null;
        // 2
        if (objectInHand.GetComponent<Weapon>() || objectInHand.GetComponent<Tool>())
        {
            controllerModel.SetActive(false);
            return;
        }
        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    public void GrabObject(GameObject obj)
    {
        // 1
        objectInHand = obj;
        if (objectInHand.GetComponent<Item>())
        {
            if (this.handType == SteamVR_Input_Sources.LeftHand)
                objectInHand.GetComponent<Item>().LHand = this.gameObject;
            else if (this.handType == SteamVR_Input_Sources.RightHand)
                objectInHand.GetComponent<Item>().RHand = this.gameObject;
            try
            {
                objectInHand.GetComponent<Item>().PickUp(this.gameObject);
            }
            catch
            {

            }

        }

        if (objectInHand.GetComponent<Weapon>() || objectInHand.GetComponent<Tool>())
        {
            controllerModel.SetActive(false);
            return;
        }

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }


    public void ReleaseObject()
    {
        // 1
        //if (GetComponent<FixedJoint>())
        //{

            if (objectInHand.GetComponent<Item>())
            {
                if (this.handType == SteamVR_Input_Sources.LeftHand)
                    objectInHand.GetComponent<Item>().LHand = null;
                else if (this.handType == SteamVR_Input_Sources.RightHand)
                    objectInHand.GetComponent<Item>().RHand = null;
                else
                {
                    objectInHand.GetComponent<Item>().LHand = null;
                    objectInHand.GetComponent<Item>().RHand = null;
                }

                
                try
                {
                    objectInHand.GetComponent<Item>().Drop(this.gameObject);
                }
                catch
                {

                }
            }

            // 2
            foreach(FixedJoint fixedJoint in GetComponents<FixedJoint>())
            {
                fixedJoint.connectedBody = null;
                Destroy(fixedJoint);
            }
            // 3
            objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
            objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();

        //}
        // 4
        objectInHand = null;
        controllerModel.SetActive(true);
    }

    public bool GrabAction()
    {
        return grabAction.GetLastStateDown(handType);
    }
    public bool ReleaseAction()
    {
        return grabAction.GetLastStateUp(handType);
    }

    public bool TriggerPullAction()
    {
        return triggerAction.GetLastStateDown(handType);
    }
    public bool TriggerReleaseAction()
    {
        return triggerAction.GetLastStateUp(handType);
    }

    // Turn pointer, for the UI, on and off
    public bool ToglePointer()
    {
        return toglePointerAction.GetLastStateUp(handType);
    }

}
