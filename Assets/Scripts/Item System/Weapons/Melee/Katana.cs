using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : Melee
{

    private void Start()
    {
        FindObjectOfType<SaveManager>().SaveItem(Application.loadedLevelName, this.gameObject);
    }

    public override void Drop(GameObject hand)
    {
        this.transform.parent = null;
        this.gameObject.layer = 0;
        this.GetComponent<Rigidbody>().isKinematic = false;
    }

    public override void PickUp(GameObject hand)
    {
        this.gameObject.layer = ignoreLayerNumber;
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.transform.parent = hand.transform;
        transform.localPosition = new Vector3(-0.0371f, 0.0311f, -0.0916f);
        this.transform.localRotation = Quaternion.Euler(-30, 0, 0);

        if(!this.transform.GetChild(0).GetComponent<Blade>().gameObject.active)
            this.transform.GetChild(0).GetComponent<Blade>().gameObject.SetActive(true);

    }
    
}
