using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class WorkStation : MonoBehaviour
{
    public string Name;
    // UI
    public GameObject canvas;
    // Position that new items are spawned in
    public Transform newCraftedItemsPos;
    public bool active;

    // List of materials inside
    public List<MaterialsInfo> materialsInfo;
    // List of recipes for items
    public List<CraftingRecipe> recipes;

    // Prefab of crafting materials, that can be return to the player if not used in crafting
    //public List<GameObject> materialPrefab;

    // Animations
    [SerializeField]
    public Animator animator;

    public Inventory itemPlacer;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<VR_BoddyCollision>())
        {
            //canvas.SetActive(true);
            //ItemCollector.gameObject.SetActive(true);
            active = true;
            animator.SetTrigger("PlayerEnter");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<VR_BoddyCollision>())
        {
            //canvas.SetActive(false);
            //ItemCollector.gameObject.SetActive(false);
            active = false;
            animator.SetTrigger("PlayerExit");
        }
    }
    public abstract void AddCraftingMaterials(CraftingMaterial material);

    public abstract void RemoveCraftingMaterials(MaterialType removeType);

    public int CheckIfMaterialAcceptable(MaterialType type)
    {
        for(int i = 0; i < materialsInfo.Count; i++)
        {
            if(materialsInfo[i].type == type)
            {
                return i;
            }
        }

        return -1;
    }

}