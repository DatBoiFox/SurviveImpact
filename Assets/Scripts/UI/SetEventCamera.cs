using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEventCamera : MonoBehaviour
{
    void Start()
    {
        GetComponent<Canvas>().worldCamera = FindObjectOfType<Pointer>().GetComponent<Camera>();
    }

}
