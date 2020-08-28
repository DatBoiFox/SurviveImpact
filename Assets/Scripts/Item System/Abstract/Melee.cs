using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Melee : Weapon
{
    [Range(0, 10f)]
    public float attackSpeed;
}
