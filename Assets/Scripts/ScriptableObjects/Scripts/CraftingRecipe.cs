using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CraftingRecipes", menuName = "Craftings/Recipe", order = 1)]
public class CraftingRecipe : ScriptableObject
{
    public List<MaterialsInfo> requiredMaterials;
    public GameObject prefab;
    public string Title;
    public Sprite image;

    // Returns true if materials list contains all the materials required for item to be crafted.
    public bool canCraft(List<MaterialsInfo> materials)
    {
        //if (requiredMaterials.Count != materials.Count)
        //    return false;

        bool[] isAvalible = new bool[requiredMaterials.Count];

        for(int i = 0; i < requiredMaterials.Count; i++)
        {
            for(int j = 0; j < materials.Count; j++)
            {
                if(requiredMaterials[i].type == materials[j].type)
                {
                    isAvalible[i] = requiredMaterials[i].count <= materials[j].count ? true : false;
                    break;
                }
            }
        }

        foreach(bool b in isAvalible)
        {
            if (!b)
                return false;
        }

        return true;
    }

    public void Craft(List<MaterialsInfo> materials)
    {
        if (!canCraft(materials))
            return;
        for (int i = 0; i < requiredMaterials.Count; i++)
        {
            for (int j = 0; j < materials.Count; j++)
            {
                if (requiredMaterials[i].type == materials[j].type)
                {
                    materials[j].count -= requiredMaterials[i].count;
                    break;
                }
            }
        }
    }

}
