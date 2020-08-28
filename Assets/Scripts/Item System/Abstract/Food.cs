using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Food : Item
{
    // Scriptable Object that represents the information of this item (amount of HP, Hunger and Hydration added after consuming)
    public FoodInfo foodInfo;

    public override void Drop(GameObject hand)
    {
        //this.gameObject.layer = 0;
    }

    public override void PickUp(GameObject hand)
    {
        //this.gameObject.layer = 8;
    }
}
