using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class Chainsaw : Melee
{
    public Chain chain;
    public bool active;
    public AudioSource audioSourceIdle;
    public AudioSource audioSourceCutting;
    public override void Drop(GameObject hand)
    {
        if (handQueue[0] == hand)
        {
            this.transform.parent = null;
            this.transform.localScale = Vector3.one;
            this.GetComponent<Rigidbody>().isKinematic = false;
            LHand = null;
            RHand = null;
            handQueue = new List<GameObject>();
        }
        handQueue.Remove(hand);
    }

    public override void PickUp(GameObject hand)
    {
        if (hand.GetComponent<HandGrab>().handType == SteamVR_Input_Sources.LeftHand)
        {
            LHand = hand;
            handQueue.Add(hand);
        }
        else
        {
            RHand = hand;
            handQueue.Add(hand);
        }

        if (handQueue.Count > 1)
        {
            if (twohanded)
            {
                //this.transform.LookAt()
                return;
            }
        }

        this.GetComponent<Rigidbody>().isKinematic = true;
        this.gameObject.layer = 8;
        Debug.Log(hand.name);
        this.transform.parent = hand.transform;
        this.transform.localPosition = new Vector3(-0.0017f, 0.0166f, -0.0848f);
        this.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void Update()
    {
        HandleSecondHand();
        
        if(handQueue.Count > 0)
        {
            if (handQueue[0].GetComponent<HandGrab>().fingerDown)
            {
                active = true;
            }else if (!handQueue[0].GetComponent<HandGrab>().fingerDown)
            {
                active = false;
            }

            if (active)
            {
                if (audioSourceIdle.isPlaying)
                    audioSourceIdle.Stop();
                if (!audioSourceCutting.isPlaying)
                    audioSourceCutting.Play();
            }
            else if (!active)
            {
                if (!audioSourceIdle.isPlaying)
                    audioSourceIdle.Play();
                if (audioSourceCutting.isPlaying)
                    audioSourceCutting.Stop();
            }
        }
        else
        {
            audioSourceIdle.Stop();
            audioSourceCutting.Stop();
        }
    }

    private void HandleSecondHand()
    {
        if (handQueue.Count <= 0)
            return;

        if (handQueue.Count > 1)
        {
            this.transform.LookAt(new Vector3(handQueue[1].transform.position.x, handQueue[1].transform.position.y, handQueue[1].transform.position.z));
        }
        else
        {
            this.transform.localPosition = new Vector3(-0.0017f, 0.0166f, -0.0848f);
            this.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
