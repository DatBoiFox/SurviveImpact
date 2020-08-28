using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Socket : MonoBehaviour
{
    public int SocketID;
    // Items
    public Item hoveringtItem;
    public Collider[] hoveringItemCollider;
    public List<Item> currentlyAttachedItems;
    public int currentItemsID;


    // Item Scale that is inside the container
    public Vector3 itemSize;

    // Hand that interacts with container
    public HandGrab hand;

    // Item Count
    public Text ItemCount;

    public IEnumerator PlacementCooldown()
    {
        Collider col = currentlyAttachedItems[0].GetComponent<Collider>();
        currentlyAttachedItems.RemoveAt(0);
        ItemCount.text = "" + currentlyAttachedItems.Count;
        yield return new WaitForSeconds(1f);
        foreach(Collider c in col.GetComponents<Collider>())
            c.enabled = true;
    }

    public abstract void AddItem(Item item);
    public abstract void AddAndGroupItem(Item item);

    public abstract void RemoveItem();

}

