using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingMaterial : Item
{

    public MaterialType type;


    private void Start()
    {
        FindObjectOfType<SaveManager>().SaveItem(Application.loadedLevelName, this.gameObject);
    }

    public override void Drop(GameObject hand)
    {
        this.gameObject.layer = 14;
    }

    public override void PickUp(GameObject hand)
    {
        this.gameObject.layer = 8;
    }
}
