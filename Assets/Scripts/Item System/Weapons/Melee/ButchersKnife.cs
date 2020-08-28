using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButchersKnife : Melee
{

    private void Start()
    {
        FindObjectOfType<SaveManager>().SaveItem(Application.loadedLevelName, this.gameObject);
    }
    public override void Drop(GameObject hand)
    {
        //Hand = null;
        this.transform.parent = null;
        this.GetComponent<Rigidbody>().isKinematic = false;
        LHand = null;
        RHand = null;
        this.gameObject.layer = 0;
    }

    public override void PickUp(GameObject hand)
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.gameObject.layer = ignoreLayerNumber;
        this.transform.parent = hand.transform;
        transform.localPosition = new Vector3(-0.0371f, 0.0311f, -0.0916f);
        this.transform.localRotation = Quaternion.Euler(0, 0, 0);

        if (!this.transform.GetChild(0).GetComponent<Blade>().gameObject.active)
            this.transform.GetChild(0).GetComponent<Blade>().gameObject.SetActive(true);
    }
}
