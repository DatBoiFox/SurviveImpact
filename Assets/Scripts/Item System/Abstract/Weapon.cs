using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item
{
    public float Damage;

    // if true player can take this item with two hands
    public bool twohanded = false;

    // Players layer number (to avoid collision)
    public int ignoreLayerNumber;

    // active hands on the weapon
    public List<GameObject> handQueue = new List<GameObject>();
}
