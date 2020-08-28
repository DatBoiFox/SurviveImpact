using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuVRInputModule : VRInputModule
{
    protected override void Awake()
    {
        data = new PointerEventData(eventSystem);
    }
}
