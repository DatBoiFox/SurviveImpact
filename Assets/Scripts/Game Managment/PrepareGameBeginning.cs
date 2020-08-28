using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareGameBeginning : MonoBehaviour
{
    // instance of itself
    private PrepareGameBeginning instance;
    [SerializeField]
    private bool firstTimeLoad = true;

    public GameObject Trees;

    // List of starting items
    public GameObject[] StartingItems;
    // List of places that starting item can be placed
    public Transform[] StartingItemPlaces;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (firstTimeLoad)
        {
            firstTimeLoad = false;
            FindObjectOfType<SaveManager>().SaveItem(Application.loadedLevelName, Instantiate(Trees, Vector3.zero, Quaternion.identity));
            SpawnStartingItems();
            FindObjectOfType<TeleportAreaUI>().SetTimerState(false);
        }
    }

    private void SpawnStartingItems()
    {
        for(int i = 0; i < StartingItems.Length; i++)
        {
            Instantiate(StartingItems[i], StartingItemPlaces[i].position, Quaternion.identity);
        }
    }

}
