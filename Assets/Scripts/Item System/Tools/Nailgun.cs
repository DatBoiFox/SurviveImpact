using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nailgun : Tool
{
    public GameObject Nail;
    public Transform pipe;

    private void Awake()
    {
        //ItemID = this.GetType().Name.GetHashCode();
    }

    public override void DoToolStuff()
    {
        var nail = Instantiate(Nail, pipe.position, pipe.rotation);
        Destroy(nail, .1f);
    }

    public override void Drop(GameObject hand)
    {
        throw new System.NotImplementedException();
    }

    public override void PickUp(GameObject hand)
    {
        throw new System.NotImplementedException();
    }
}
