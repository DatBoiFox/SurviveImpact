using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildable : Item
{
    //Objects prefab
    public GameObject prefab;

    public override void Drop(GameObject hand)
    {
        throw new System.NotImplementedException();
    }

    public override void PickUp(GameObject hand)
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        FindObjectOfType<SaveManager>().SaveItem(Application.loadedLevelName, this.gameObject);
    }


}
