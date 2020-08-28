using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusSystem
{
    private float Health    = 100f;
    private float Hunger    = 100f;
    private float Hydration = 100f;

    private bool isAlive;

    public StatusSystem()
    {
        isAlive = true;
    }

    public StatusSystem(float Health, float Hunger, float Hydration)
    {
        isAlive = true;
        this.Health = Health;
        this.Hunger = Hunger;
        this.Hydration = Hydration;
    }

    // Setters
    public void SetHealth(float Health)
    {
        this.Health = Health;
    }

    public void SetHunger(float Hunger)
    {
        this.Hunger = Hunger;
    }

    public void SetHydration(float Hydration)
    {
        this.Hydration = Hydration;
    }
    // Getters

    public float GetHealth()
    {
        return this.Health;
    }

    public float GetHunger()
    {
        return this.Hunger;
    }

    public float GetHydration()
    {
        return this.Hydration;
    }

    // Logic

    public void ApplyDamage(float damage)
    {
        if(isAlive)
        {
            this.Health -= damage;
            this.Health = Mathf.Clamp(this.Health, 0, 100);
        }
    }

    public void ApplyHunger(float hunger)
    {
        if (isAlive)
        {
            this.Hunger -= hunger;
            this.Hunger = Mathf.Clamp(this.Hunger, 0, 100);
        }
    }

    public void ApplyHydration(float hydration)
    {
        if (isAlive)
        {
            this.Hydration -= hydration;
            this.Hydration = Mathf.Clamp(this.Hydration, 0, 100);
        }
    }

    public void ConsumeFood(FoodInfo food)
    {
        this.Health += food.Health;
        this.Hunger += food.Hunger;
        this.Hydration += food.Hydration;
    }

}
