using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;
using Valve.VR.InteractionSystem;

public class Manipulation : MonoBehaviour
{
    public SteamVR_Input_Sources handType; // 1
    public SteamVR_Action_Boolean grab; // 3
    private Hand hand;

    private void Start()
    {
        hand = GetComponent<Hand>();
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;
        Debug.Log(pos);

        if(GetGrab())
            this.transform.parent = collision.transform;
    }

    private void Update()
    {
        
    }

    public bool GetGrab() // 2
    {
        return grab.GetLastStateDown(handType);
    }
}
