using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySocket : Socket
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Item>())
        {
            if (other.GetComponent<Item>().LHand != null || other.GetComponent<Item>().RHand != null)
            {
                hoveringtItem = other.GetComponent<Item>();
                hoveringItemCollider = hoveringtItem.GetComponents<Collider>();
                AddItem(hoveringtItem);
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

    public override void AddAndGroupItem(Item item)
    {
        throw new System.NotImplementedException();
    }

    public override void AddItem(Item item)
    {
        if (item.GetComponent<Item>())
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
                hoveringtItem.LHand.GetComponent<HandGrab>().ReleaseObject();
            }
            else if (hoveringtItem.RHand != null)
            {
                hoveringtItem.RHand.GetComponent<HandGrab>().collidingObject = null;
                hoveringtItem.RHand.GetComponent<HandGrab>().ReleaseObject();
            }
            foreach (Collider c in hoveringItemCollider)
            {
                c.enabled = false;
            }
            hoveringtItem.GetComponent<Collider>().enabled = false;
            hoveringtItem.GetComponent<Rigidbody>().isKinematic = true;

            if (hoveringtItem.transform.childCount > 0)
            {
                if (hoveringtItem.transform.GetChild(0).GetComponent<Blade>() ||
                    hoveringtItem.transform.GetChild(0).GetComponent<AxeBlade>())
                {
                    hoveringtItem.transform.GetChild(0).gameObject.SetActive(false);
                }
            }

            hoveringtItem.transform.position = this.transform.position;
            hoveringtItem.transform.parent = this.transform;
            hoveringtItem.transform.localScale = new Vector3(hoveringtItem.transform.localScale.x, hoveringtItem.transform.localScale.y, hoveringtItem.transform.localScale.z) * 0.2f;
            currentlyAttachedItems.Add(hoveringtItem);
            ItemCount.text = "" + currentlyAttachedItems.Count;
        }
        hand = null;
    }

    private void Update()
    {
        if(hand != null)
        {
            if (hand.GrabAction())
            {
                RemoveItem();
            }
        }

        if(currentlyAttachedItems.Count <= 0)
        {
            currentItemsID = -1;
        }

        foreach(Item it in currentlyAttachedItems)
        {
            if(it.gameObject.active == false)
            {
                FindObjectOfType<SaveManager>().evaluateSceneItemDifference(it.gameObject);
            }
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
        StartCoroutine(PlacementCooldown());
        hand = null;
    }

    public new IEnumerator PlacementCooldown()
    {
        Collider col = currentlyAttachedItems[0].GetComponent<Collider>();
        this.GetComponent<Collider>().enabled = false;
        currentlyAttachedItems.RemoveAt(0);
        ItemCount.text = "" + currentlyAttachedItems.Count;
        yield return new WaitForSeconds(1f);
        this.GetComponent<Collider>().enabled = true;
        foreach (Collider c in col.GetComponents<Collider>())
            c.enabled = true;
    }
}
