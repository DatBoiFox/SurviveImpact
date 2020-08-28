using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerStatus : MonoBehaviour
{
    // a reference to the action
    //public SteamVR_Action_Boolean Menu;
    // a reference to the hand
    public SteamVR_Input_Sources handType;
    [SerializeField]
    private bool isMenuActive = false;

    [SerializeField]
    private GameObject Stats;
    [SerializeField]
    private Image healthUI;
    [SerializeField]
    private Image hungerUI;
    [SerializeField]
    private Image HydrationUI;


    // UI position

    public Transform hand;
    [Range(-360f, 360f)]
    public float aSize;

    [Range(-360f, 360f)]
    public float bSize;

    // Status Info
    [SerializeField]
    private PlayerObject player;

    private bool inventoryOpen = false;
    public GameObject inventory;

    void Start()
    {
        player = FindObjectOfType<PlayerObject>();
        Stats.active = false;
    }

    private void Update()
    {

        if (hand.gameObject.transform.eulerAngles.z >= aSize && hand.gameObject.transform.eulerAngles.z <= bSize)
        {
            Stats.active = true;
            //if (!Stats.active)
            //{
            //    CalculateStatus();
            //    Stats.active = true;
            //}
        }
        else
        {
            Stats.active = false;
        }

        if (Stats.active)
        {
            CalculateStatus();
        }

    }

    public void CalculateStatus()
    {
        healthUI.fillAmount     = player.GetStatusSystem().GetHealth() * 0.01f;
        hungerUI.fillAmount     = player.GetStatusSystem().GetHunger() * 0.01f;
        HydrationUI.fillAmount  = player.GetStatusSystem().GetHydration() * 0.01f;
    }

    public void TogleInventory()
    {
        if (!inventoryOpen)
        {
            inventoryOpen = true;
            inventory.SetActive(true);
        }else if (inventoryOpen)
        {
            inventoryOpen = false;
            inventory.SetActive(false);
        }
    }

}
