using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "Food/FoodInfo", order = 1)]
public class FoodInfo : ScriptableObject
{
    // Description
    public string Info;
    public float Hydration;
    public float Hunger;
    public float Health;
    [Range(0, 100)]
    public float LiquidAmount;
}