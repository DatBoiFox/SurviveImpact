using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editable : MonoBehaviour
{
    [SerializeField]
    private GameObject manipulation_handle;

    private List<GameObject> manipulation_handles = new List<GameObject>();

    private Vector3[] sides = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right, Vector3.up, Vector3.down };

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i <6; i++)
        {
            manipulation_handles.Add(Instantiate(manipulation_handle, this.transform.position + sides[i], Quaternion.identity));
            manipulation_handles[i].transform.parent = this.transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //for(int i = 0; i < 6; i++) { 
        //    this.transform.position = manipulation_handles[i].transform.position - this.transform.position;
        //    //manipulation_handles[i].transform.position = this.transform.position + sides[i];
        //}

        this.transform.position = (manipulation_handles[0].transform.position - this.transform.position);

    }
}
