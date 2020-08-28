using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // Instance of itself
    private static SaveManager instance;
    // Base scene (player base)
    public string scene_0;
    // Town scene
    public string scene_1;

    private string currentScene;

    // Object list that are a part of the first scene
    [SerializeField]
    private List<GameObject> scene_0_objects;
    // Object list that are a part of the second scene
    [SerializeField]
    private List<GameObject> scene_1_objects;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if(instance == null)
        {
            scene_0_objects = new List<GameObject>();
            scene_1_objects = new List<GameObject>();
            instance = this;
            currentScene = Application.loadedLevelName;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveItem(string scene, GameObject item)
    {
        if (scene == scene_0)
        {
            scene_0_objects.Add(item);
        }else if (scene == scene_1)
        {
            scene_1_objects.Add(item);
        }
        else
        {
            return;
        }

        DontDestroyOnLoad(item);
    }

    // Sets list items to active or inactive.
    private void setItemsState(List<GameObject> itemList, bool state)
    {
        foreach (GameObject obj in itemList)
        {
            if (obj != null)
            {
                obj.SetActive(state);
            }
        }
    }

    // Handles all the objects before changing scenes (sets them active and inactive depending on the scene)
    public bool prepareForNextScene()
    {
        if (currentScene == scene_0)
        {
            foreach(GameObject obj in scene_0_objects)
            {
                if(obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
        else if (currentScene == scene_1)
        {
            foreach (GameObject obj in scene_1_objects)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
        return true;
    }

    // Checks if the item is not from this scene, and removes it from his old list and ads to the current scene item list (This is used to handle items that are in players hand or inventory)
    public void evaluateSceneItemDifference(GameObject item)
    {
        if(scene_0_objects.Contains(item) && currentScene != scene_0)
        {
            scene_0_objects.Remove(item);
            scene_1_objects.Add(item);
            item.SetActive(true);
        }else if (scene_1_objects.Contains(item) && currentScene != scene_1)
        {
            scene_1_objects.Remove(item);
            scene_0_objects.Add(item);
            item.SetActive(true);
        }
    }

    private void Update()
    {
        currentScene = Application.loadedLevelName;

        if (currentScene == scene_0)
        {
            setItemsState(scene_0_objects, true);
            setItemsState(scene_1_objects, false);
        }
        else if (currentScene == scene_1)
        {
            setItemsState(scene_1_objects, true);
            setItemsState(scene_0_objects, false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Application.LoadLevel(scene_0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Application.LoadLevel(scene_1);
        }
    }

    // Destroys all the items in the scenes
    public void CleanScenes()
    {
        foreach (GameObject obj in scene_0_objects)
            Destroy(obj);

        foreach (GameObject obj in scene_1_objects)
            Destroy(obj);
    }
}
