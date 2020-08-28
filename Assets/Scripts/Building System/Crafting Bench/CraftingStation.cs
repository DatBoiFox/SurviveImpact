using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingStation : WorkStation
{
    //public Text woodCount;

    // Buttons, representing available items to craft
    public List<GameObject> buttons = new List<GameObject>();
    public GameObject infoWindow;
    public RectTransform listItemPrefab;
    public RectTransform Context;
    //Buttons become active when crafting table has enough materials for item to craft 
    private bool[] activeButtons;



    private void Start()
    {
        FindObjectOfType<SaveManager>().SaveItem(Application.loadedLevelName, this.gameObject);

        foreach(CraftingRecipe recepie in recipes)
        {
            buttons.Add(Instantiate(listItemPrefab, Context).gameObject);
        }

        for(int i = 0; i < recipes.Count; i++)
        {
            buttons[i].transform.Find("Title").GetComponent<Text>().text = recipes[i].Title;
            buttons[i].transform.Find("Image").GetComponent<Image>().sprite = recipes[i].image;
            int n = i;
            //buttons[i].GetComponent<Button>().onClick.AddListener(() => CraftSelected(n));
            buttons[i].GetComponent<Button>().onClick.AddListener(() => ShowSelected(n));

        }
        activeButtons = new bool[buttons.Count];
    }

    //private void FixedUpdate()
    //{
    //    UpdateCurrentlyPlacedMaterials();
    //}

    // Adds crafting resources to the table.
    public override void AddCraftingMaterials(CraftingMaterial material)
    {
        int check = CheckIfMaterialAcceptable(material.type);

        if (check == -1)
            return;

        materialsInfo[check].count += 1;
    }

    // Removes crafting materials from the crafting table
    public override void RemoveCraftingMaterials(MaterialType removeType)
    {
        int ind = CheckIfMaterialAcceptable(removeType);
        if (ind == -1)
            return;

        if(materialsInfo[ind].count > 0)
        {
            materialsInfo[ind].count--;
            //Instantiate(materialsInfo[ind].materialPrefab, returnItemsPos.position, Quaternion.identity);
        }

    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9)
                canvas.active = true;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Close"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3)
                canvas.active = false;
        }
        //woodCount.text = "" + materialsInfo[0].count;
        ActivateAvalibleCraftings();
        checkIfAvalible();
    }

    // Activates buttons (paints them red or green), if the item that they represent can be crafted.
    private void ActivateAvalibleCraftings()
    {
        for (int i = buttons.Count - 1; i >= 0; i--)
        {
            if (!activeButtons[i])
            {

                buttons[i].GetComponent<Image>().color = Color.red;
            }
            else
            {
                buttons[i].GetComponent<Image>().color = Color.green;
            }
        }
    }

    //Checks if there are enough materials for item to craft by checking it's recepie.
    private void checkIfAvalible()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (recipes[i].canCraft(materialsInfo))
            {
                activeButtons[i] = true;
                Debug.Log(recipes[i].Title + "  " + recipes[i].canCraft(materialsInfo));
            }
            else
            {
                activeButtons[i] = false;
            }
        }
    }

    //Paints button.
    private ColorBlock ActivateButton(Button button,  Color color)
    {
        ColorBlock c = button.colors;
        c.normalColor = color;
        c.highlightedColor = color;
        c.selectedColor = color;

        return c;
    }

    private void CraftSelected(int i)
    {
        Debug.Log("Craft Called");
        //animator.Play();
        if (activeButtons[i])
        {
            recipes[i].Craft(materialsInfo);
            Instantiate(recipes[i].prefab, newCraftedItemsPos.position, Quaternion.identity);
            Debug.Log(recipes[i].requiredMaterials[0].materialPrefab.GetComponent<Item>().ItemID);
            foreach(MaterialsInfo mats in recipes[i].requiredMaterials)
            {
                for(int k = 0; k < mats.count; k++)
                {
                    itemPlacer.DestroyItem(mats.materialPrefab.GetComponent<Item>());
                }
            }

            //Instantiate(buildingMaterials[i], newCraftedItemsPos.position, Quaternion.identity);
            //currentlyHoldingMaterials -= buildingMaterials[i].BuildingPrice;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetTrigger("PlayerEnter");
            other.transform.parent.GetComponent<PlayerObject>().Pointer.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetTrigger("PlayerExit");
            other.transform.parent.GetComponent<PlayerObject>().Pointer.gameObject.SetActive(false);
        }
    }

    private void ShowSelected(int index)
    {
        infoWindow.transform.Find("Title").GetComponent<Text>().text = recipes[index].Title;
        infoWindow.transform.Find("Image").GetComponent<Image>().sprite = recipes[index].image;
        infoWindow.transform.Find("RequiredItems").GetComponent<Text>().text = "";
        infoWindow.transform.Find("Craft").GetComponent<Button>().onClick.RemoveAllListeners();
        for (int i = 0; i < recipes[index].requiredMaterials.Count; i++)
        {
            infoWindow.transform.Find("RequiredItems").GetComponent<Text>().text += recipes[index].requiredMaterials[i].count + " " + recipes[index].requiredMaterials[i].type + "\n";
        }
        infoWindow.transform.Find("Craft").GetComponent<Button>().onClick.AddListener(() => CraftSelected(index));
    }
}
