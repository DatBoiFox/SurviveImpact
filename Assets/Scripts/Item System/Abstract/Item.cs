using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IDestroyable/*, ICollectable*/
{
    public string Name;
    public int Quantity;
    public double Health;
    public bool destructible;

    //Unique id
    public int ItemID;


    public GameObject LHand = null;
    public GameObject RHand = null;

    public abstract void PickUp(GameObject hand);
    public abstract void Drop(GameObject hand);

    // Audio
    [SerializeField]
    protected AudioSource audio;
    [SerializeField]
    private AudioClip damageSound;


    public void ApplyDamage(float damage)
    {
        if (!destructible)
            return;

        Debug.Log("Applying damage");
        audio.PlayOneShot(damageSound);
        if ((Health - damage) < 0)
        {
            Destroy();
        }
        else
        {
            Health -= damage;
        }
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

}
