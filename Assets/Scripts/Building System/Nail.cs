using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nail : MonoBehaviour
{
    private void Start()
    {
        Collider[] colliders = Physics.OverlapBox(this.transform.position, this.transform.localScale * 0.5f, transform.rotation);
        
        NailObjects(colliders);
    }

    private void NailObjects(Collider[] colliders)
    {
        if (colliders.Length <= 0)
            return;
        List<Rigidbody> rigidbodies = new List<Rigidbody>();
        foreach(Collider collider in colliders)
        {
            //if(collider.gameObject.GetComponent<BuildingMaterial>() || collider.gameObject.tag == "Ground")
            //{
            //    rigidbodies.Add(collider.GetComponent<Rigidbody>());
            //}
        }

        if (rigidbodies.Count < 1)
            return;

        for(int i = 1; i < rigidbodies.Count; i++)
        {
            rigidbodies[0].gameObject.AddComponent<FixedJoint>().connectedBody = rigidbodies[i];
        }

    }
}
