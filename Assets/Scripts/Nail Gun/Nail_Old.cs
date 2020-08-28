using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nail_Old : MonoBehaviour
{


    [SerializeField]
    private Collider[] collidingObjects;
    [SerializeField]
    private GameObject Base;

    public float speed;
    private Vector3 position;
    private Quaternion rotation;
    private void Start()
    {
        collidingObjects = Physics.OverlapBox(this.transform.position, Base.transform.localScale / 2, this.transform.rotation);
        foreach (Collider c in collidingObjects)
        {
            if(c.gameObject != this.gameObject && c.gameObject.tag == "BuildingMaterial")
            {
                FixedJoint j = this.gameObject.AddComponent<FixedJoint>();
                j.connectedBody = c.gameObject.GetComponent<Rigidbody>();
                j.enablePreprocessing = true;
                j.autoConfigureConnectedAnchor = false;
                j.connectedMassScale = 1000;
                j.massScale = 1000;
            }

        }

        //position = this.transform.position;
        //rotation = this.transform.rotation;
        //GetComponent<Rigidbody>().AddForce(-this.transform.right*speed);
        //NailObjects();

    }

    private GameObject FindParent(GameObject gameObject)
    {
        Transform parent = gameObject.transform;

        while(parent.parent != null)
        {
            parent = parent.parent.transform;
        }

        return parent.gameObject;
    }
    //private void NailObjects()
    //{
    //    collidingObjects = Physics.OverlapBox(this.transform.position, Base.transform.localScale / 2, this.transform.rotation);
    //    //if (collidingObjects.Length < 1)
    //    //    return;

    //    foreach (Collider gm in collidingObjects)
    //    {
    //        //if (gm.gameObject.tag != "BuildingMaterial")
    //        //    break;
    //        Debug.Log(gm.gameObject.name + " " + gm.gameObject.tag);
    //        if (gm.gameObject == this.gameObject)
    //            break;
    //        GameObject gameObject = FindParent(gm.gameObject);
    //        if (gameObject.GetComponent<Rigidbody>())
    //        {

    //            if (this.gameObject.GetComponent<Rigidbody>())
    //            {
    //                this.gameObject.GetComponent<Rigidbody>().mass += gameObject.GetComponent<Rigidbody>().mass;
    //            }
    //            else
    //            {
    //                this.gameObject.AddComponent<Rigidbody>().mass += gameObject.GetComponent<Rigidbody>().mass;
    //            }
    //            Destroy(gameObject.GetComponent<Rigidbody>());
    //        }
    //        gameObject.transform.SetParent(this.transform, true);
    //    }
    //    //CombineMesh();
    //}
    //private void CombineMesh()
    //{
    //    this.transform.position = Vector3.zero;
    //    this.transform.rotation = Quaternion.Euler(Vector3.zero);
    //    MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
    //    CombineInstance[] combine = new CombineInstance[meshFilters.Length];
    //    //Debug.Log(meshFilters[0].name);
    //    int i = 1;
    //    while (i < meshFilters.Length)
    //    {
    //        combine[i].mesh = meshFilters[i].sharedMesh;
    //        combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
    //        //meshFilters[i].gameObject.SetActive(false);
    //        Destroy(meshFilters[i].gameObject.GetComponent<BoxCollider>());

    //        i++;
    //    }
    //    transform.GetComponent<MeshFilter>().mesh = new Mesh();
    //    transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
    //    transform.GetComponent<MeshRenderer>().enabled = false;
    //    //transform.gameObject.SetActive(true);
    //    transform.gameObject.AddComponent<MeshCollider>().convex = true;
    //    transform.gameObject.AddComponent<Rigidbody>();
    //    this.transform.position = position;
    //    this.transform.rotation = rotation;
    //}
}
