using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ammunition : Item
{
    public AmmunitionType ammunitionType;
    private void Awake()
    {
        //ItemID = this.GetType().Name.GetHashCode();
    }

    private void Start()
    {
        FindObjectOfType<SaveManager>().SaveItem(SceneManager.GetActiveScene().name, this.gameObject);
    }

    public override void Drop(GameObject hand)
    {
        throw new System.NotImplementedException();
    }

    public override void PickUp(GameObject hand)
    {
        throw new System.NotImplementedException();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Magazine>())
            collision.gameObject.GetComponent<Magazine>().AddBullets(this);

    }

}
