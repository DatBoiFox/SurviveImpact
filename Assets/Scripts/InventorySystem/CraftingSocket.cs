using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSocket : Socket
{

    public WorkStation workStation;
    public Inventory inventory;

    private void Update()
    {
        if (currentlyAttachedItems != null)
        {
            if (hand != null)
            {
                if (hand.GrabAction())
                {
                    RemoveItem();
                }
            }
        }

        if (currentlyAttachedItems.Count <= 0)
        {
            currentItemsID = -1;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CraftingMaterial>())
        {
            if (other.GetComponent<CraftingMaterial>().LHand != null || other.GetComponent<CraftingMaterial>().RHand != null)
            {
                hoveringtItem = other.GetComponent<Item>();
                hoveringItemCollider = hoveringtItem.GetComponents<Collider>();
                //AddItem(hoveringtItem);
                AddAndGroupItem(hoveringtItem);
                hoveringtItem = null;
                hoveringItemCollider = null;
            }
        }
        else if (other.GetComponent<HandGrab>())
        {
            hand = other.GetComponent<HandGrab>();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<HandGrab>())
        {
            hand = null;
        }
    }

    public override void RemoveItem()
    {
        if (currentlyAttachedItems.Count <= 0)
            return;

        currentlyAttachedItems[0].GetComponent<Rigidbody>().isKinematic = false;
        currentlyAttachedItems[0].transform.parent = null;
        currentlyAttachedItems[0].transform.localScale = itemSize;
        hand.GrabObject(currentlyAttachedItems[0].gameObject);
        workStation.RemoveCraftingMaterials(currentlyAttachedItems[0].GetComponent<CraftingMaterial>().type);
        StartCoroutine(PlacementCooldown());
        
    }

    public override void AddItem(Item item)
    {
        if (item.GetComponent<CraftingMaterial>())
        {
            if (currentlyAttachedItems.Count <= 0)
            {
                currentItemsID = item.ItemID;
                itemSize = hoveringtItem.transform.lossyScale;

            }
            else if (currentItemsID != item.ItemID)
            {
                return;
            }
            if (hoveringtItem.LHand != null)
            {
                hoveringtItem.LHand.GetComponent<HandGrab>().collidingObject = null;
            }
            else if (hoveringtItem.RHand != null)
            {
                hoveringtItem.RHand.GetComponent<HandGrab>().collidingObject = null;
            }
            foreach (Collider c in hoveringItemCollider)
            {
                c.enabled = false;
            }
            hoveringtItem.GetComponent<Collider>().enabled = false;
            hoveringtItem.GetComponent<Rigidbody>().isKinematic = true;
            hoveringtItem.transform.position = this.transform.position;
            hoveringtItem.transform.parent = this.transform;
            hoveringtItem.transform.localScale = new Vector3(hoveringtItem.transform.localScale.x, hoveringtItem.transform.localScale.y, hoveringtItem.transform.localScale.z) * 0.2f;
            currentlyAttachedItems.Add(hoveringtItem);
            ItemCount.text = "" + currentlyAttachedItems.Count;
            workStation.AddCraftingMaterials(hoveringtItem.GetComponent<CraftingMaterial>());
        }
    }

    public override void AddAndGroupItem(Item item)
    {
        int socketID_m = inventory.GetSocketWithSameItem(item);
        Debug.Log("Index " + socketID_m);

        if(socketID_m == SocketID || socketID_m == -1)
        {
            AddItem(item);
        }
        else
        {
            item.transform.position = inventory.sockets[socketID_m].transform.position;
            inventory.sockets[socketID_m].AddItem(item);
        }

    }
}
