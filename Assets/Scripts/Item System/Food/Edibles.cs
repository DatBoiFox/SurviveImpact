using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edibles : Food
{

    private void Awake()
    {
        //ItemID = this.GetType().Name.GetHashCode();
    }
    private void Start()
    {
        FindObjectOfType<SaveManager>().SaveItem(Application.loadedLevelName, this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            other.transform.root.GetComponent<PlayerObject>().GetStatusSystem().ApplyHunger(-foodInfo.Hunger);
            other.transform.root.GetComponent<PlayerObject>().GetStatusSystem().ApplyHydration(-foodInfo.Hydration);
            other.transform.root.GetComponent<PlayerObject>().GetStatusSystem().ApplyDamage(-foodInfo.Health);
            Destroy(this.gameObject);
        }
    }
}
