using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Item
{
    public AudioClip structureBuiltSound;
    public override void Drop(GameObject hand)
    {
        this.gameObject.layer = 0;
    }

    public override void PickUp(GameObject hand)
    {
        this.gameObject.layer = 8;
    }
    private void Start()
    {
        FindObjectOfType<SaveManager>().SaveItem(Application.loadedLevelName, this.gameObject);
        audio.PlayOneShot(structureBuiltSound);
    }
}
