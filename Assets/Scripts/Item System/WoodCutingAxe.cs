using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodCutingAxe : Tool
{
    [SerializeField]
    private AxeBlade blade;
    [SerializeField]
    private int ignorePlayer;
    [SerializeField]
    private int baseLayer;

    private void Start()
    {
        FindObjectOfType<SaveManager>().SaveItem(Application.loadedLevelName, this.gameObject);
    }

    public override void DoToolStuff()
    {
        throw new System.NotImplementedException();
    }

    public override void PickUp(GameObject hand)
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.gameObject.layer = 8;
        this.transform.parent = hand.transform;
        int side = 0;
        this.transform.localPosition = new Vector3(-0.008f * side, -0.045f, -0.179f);
        this.transform.localRotation = Quaternion.Euler(1.071f, 92.53201f, 86.981f);
        if (!this.transform.GetChild(0).GetComponent<AxeBlade>().gameObject.active)
            this.transform.GetChild(0).GetComponent<AxeBlade>().gameObject.SetActive(true);
    }
    public override void Drop(GameObject hand)
    {
        this.transform.parent = null;
        this.gameObject.layer = 0;
        //this.transform.localScale = Vector3.one;
        this.GetComponent<Rigidbody>().isKinematic = false;
    }



}
