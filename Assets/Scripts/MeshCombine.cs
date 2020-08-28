using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CombineMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CombineMesh()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            //meshFilters[i].gameObject.SetActive(false);
            Destroy(meshFilters[i].gameObject.GetComponent<BoxCollider>());

            i++;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.GetComponent<MeshRenderer>().enabled = false;
        //transform.gameObject.SetActive(true);
        transform.gameObject.AddComponent<MeshCollider>().convex = true;
        transform.gameObject.AddComponent<Rigidbody>();
    }
}
