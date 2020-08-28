﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPointer : MonoBehaviour
{

    public LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.SetPosition(1, this.transform.forward * 500);
    }
}
