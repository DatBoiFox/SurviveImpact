using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Spear : Melee
{
    // Damage if the player holds it with two hands
    private float twoHandDamage;
    // Damage if player holds in one hand 
    private float oneHandDamage;

    [SerializeField]
    private Blade blade;
    private void Start()
    {
        twoHandDamage = Damage * 2;
        oneHandDamage = Damage;
        FindObjectOfType<SaveManager>().SaveItem(Application.loadedLevelName, this.gameObject);
    }

    private void Update()
    {
        
        HandleSecondHand();
    }

    public override void Drop(GameObject hand)
    {
        if (handQueue[0] == hand)
        {
            //blade.GetComponent<Rigidbody>().useGravity = true;
            this.transform.parent = null;
            this.gameObject.layer = 0;
            this.GetComponent<Rigidbody>().isKinematic = false;
            LHand = null;
            RHand = null;
            handQueue = new List<GameObject>();
        }
        handQueue.Remove(hand);
        if(handQueue.Count <= 0)
            this.gameObject.layer = 0;
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
                return;
            }
        }
        //blade.GetComponent<Rigidbody>().useGravity = false;
        this.gameObject.layer = ignoreLayerNumber;
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.transform.parent = hand.transform;
        transform.localPosition = new Vector3(-0.0371f, 0.0311f, -0.0916f);
        this.transform.localRotation = Quaternion.Euler(-30, 0, 0);

        if (!this.transform.GetChild(0).GetComponent<Blade>().gameObject.active)
            this.transform.GetChild(0).GetComponent<Blade>().gameObject.SetActive(true);

    }

    // Handles multiple hands
    private void HandleSecondHand()
    {
        if (handQueue.Count <= 0)
            return;
        if (handQueue.Count > 1)
        {
            Damage = twoHandDamage;
            this.transform.LookAt(new Vector3(handQueue[1].transform.position.x, handQueue[1].transform.position.y, handQueue[1].transform.position.z));
        }
        else
        {
            Damage = oneHandDamage;
            transform.localPosition = new Vector3(-0.0371f, 0.0311f, -0.0916f);
            this.transform.localRotation = Quaternion.Euler(-30, 0, 0);
        }
    }
}
