using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sapling : Item
{
    public float timeToGrow;

    // List of trees that can be randomly selected
    public GameObject[] possibleTrees;

    private int selectedTree;
    public bool placed = false;

    private void Start()
    {
        selectedTree = Random.Range(0, possibleTrees.Length);
        FindObjectOfType<SaveManager>().SaveItem(Application.loadedLevelName, this.gameObject);

    }

    private IEnumerator Grow()
    {
        yield return new WaitForSeconds(timeToGrow);
        Debug.Log("Grew");
        Instantiate(possibleTrees[selectedTree], this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (placed)
        {
            placed = false;
            StartCoroutine(Grow());
        }
    }

    public override void PickUp(GameObject hand)
    {
        throw new System.NotImplementedException();
    }

    public override void Drop(GameObject hand)
    {
        throw new System.NotImplementedException();
    }
}
