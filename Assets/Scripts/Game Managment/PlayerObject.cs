using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PlayerObject : MonoBehaviour
{
    [SerializeField]
    public StatusSystem playerStatus = new StatusSystem(100, 100, 100);
    //public Inventory inventory = new Inventory();
    public Rigidbody rigidbody;

    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;
    private bool isGrounded;

    // Pinter object that controls UI
    public Pointer Pointer;

    public bool isDead;

    // Hand to togle pointer

    public HandGrab hand;

    // Blood effect on the screen
    //public PostProcessVolume postProcessing;
    //private TextureOverlay bloodOverlay;
    //private BloodEffect bloodEffect;

    // Needs settings

    [Range(0, 0.001f)]
    public float HungerOverTime;
    [Range(0, 0.001f)]
    public float HydrationOverTime;
    [Range(0, 10)]
    public float HealthOverTime;

    private static PlayerObject instance;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);


        if (!isDead)
        {
            GetStatusSystem().ApplyHunger(HungerOverTime);
            GetStatusSystem().ApplyHydration(HydrationOverTime);

            if (GetStatusSystem().GetHunger() > 0.5f && GetStatusSystem().GetHydration() > 0.5f)
                GetStatusSystem().ApplyDamage(-HealthOverTime);
            if (GetStatusSystem().GetHunger() <= 0.1f && GetStatusSystem().GetHydration() <= 0.1f)
                GetStatusSystem().ApplyDamage(HealthOverTime);
        }

        if (hand.ToglePointer())
        {
            if (!Pointer.gameObject.active)
            {
                Pointer.gameObject.SetActive(true);
            }
            else
            {
                Pointer.gameObject.SetActive(false);
            }
        }

        if (playerStatus.GetHealth() <= 0)
            isDead = true;

    }

    public StatusSystem GetStatusSystem()
    {
        return playerStatus;
    }


}

//public class Inventory
//{
//    public List<Item> inventoryItems = new List<Item>();
//    public Dictionary<string, int> inventoryItemsDict = new Dictionary<string, int>();
//    public void AddItem(Item item)
//    {
//        if (!inventoryItemsDict.ContainsKey(item.Name))
//        {
//            inventoryItemsDict.Add(item.Name, item.Quantity);
//            inventoryItems.Add(item);
//        }
//        else if (inventoryItemsDict.ContainsKey(item.Name))
//        {
//            inventoryItemsDict[item.Name] += item.Quantity;
//        }
//    }

//    public void PrintAllItems()
//    {
//        foreach (KeyValuePair<string, int> i in inventoryItemsDict)
//        {
//            Debug.Log(i.Key + " Quantity: " + i.Value);
//        }
//    }

//    public void RemoveItem(Item item, int Quantity)
//    {
//        if (inventoryItemsDict.ContainsKey(item.Name))
//        {
//            if(inventoryItemsDict[item.Name] > Quantity)
//            {
//                inventoryItemsDict[item.Name] -= Quantity;
//            }
//            else
//            {
//                inventoryItemsDict.Remove(item.Name);
//                inventoryItems.Remove(item);
//            }
//        }

//    }


//}
