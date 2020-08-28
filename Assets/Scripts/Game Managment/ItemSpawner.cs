using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // List of possible to spawn items selected randomly
    public List<SpawnableItem> possibleItems;
    // Points where items may be spawned
    private List<GameObject> spawnPoints;

    private void Start()
    {
        spawnPoints = new List<GameObject>();
        init();
        spawnItem();
    }

    private void init()
    {
        for(int i = 0; i < this.transform.childCount; i++)
        {
            spawnPoints.Add(this.transform.GetChild(i).gameObject);
        }
    }

    private void spawnItem()
    {
        for(int i = 0; i < spawnPoints.Count; i++)
        {
            int itemID = findPossibleItem(Random.Range(0, 100));
            if (itemID != -1)
            {
                Instantiate(possibleItems[itemID].pefab, spawnPoints[i].transform.position, Quaternion.identity);
            }
        }
    }

    // Checks for the most suitable item with selected chance (drop-rate)
    private int findPossibleItem(int chance)
    {
        List<int> items = new List<int>();
        for (int i = 0; i < possibleItems.Count; i++)
        {
            if(chance <= possibleItems[i].chance)
            {
                items.Add(i);
            }
        }

        if(items.Count > 0)
        {
            return items[Random.Range(0, items.Count)];
        }

        return -1;
    }
}
[System.Serializable]
public class SpawnableItem
{
    [Range(0, 100)]
    public int chance;
    public GameObject pefab;
}
