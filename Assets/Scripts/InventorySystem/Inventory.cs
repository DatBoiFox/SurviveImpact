using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Socket[] sockets;
    private void Start()
    {
        sockets = GetComponentsInChildren<Socket>();
    }

    // Removes items from the sockets
    public void DestroyItem(Item item)
    {
        Debug.Log(item.Name);
        foreach(Socket s in sockets)
        {
            if(s.currentItemsID == item.ItemID && s.currentlyAttachedItems.Count > 0)
            {
                GameObject temp = s.currentlyAttachedItems[0].gameObject;
                //Destroy(s.currentlyAttachedItems[0].gameObject);
                s.currentlyAttachedItems.RemoveAt(0);
                s.ItemCount.text = "" + s.currentlyAttachedItems.Count;
                Destroy(temp);
            }
        }
    }

    public int GetSocketWithSameItem(Item item)
    {
        for(int i = 0; i < sockets.Length-1; i++)
        {
            if(sockets[i].currentItemsID == item.ItemID)
            {
                return i;
            }
        }

        foreach (Socket s in sockets)
            if (s.currentlyAttachedItems.Count <= 0)
                return s.SocketID;
        return -1;
    }
}
