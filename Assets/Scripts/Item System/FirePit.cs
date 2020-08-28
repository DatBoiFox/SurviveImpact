using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePit : MonoBehaviour
{
    // The number of wood added to the fire
    [SerializeField]
    private int LogsInside = 0;

    // Logs game object. (this game object is active if LogsInside > 0)
    [SerializeField]
    private GameObject logs;

    public GameObject FireFX;

    // The time it takes to burn one log
    public float burnDuration;

    [SerializeField]
    private bool isBurning;
    private bool logBurnt = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CraftingMaterial>() && other.GetComponent<CraftingMaterial>().type == MaterialType.Wood)
        {
            if (other.GetComponent<CraftingMaterial>().LHand != null || other.GetComponent<CraftingMaterial>().RHand != null)
            {
                Destroy(other.gameObject);
                LogsInside++;
            }
        }
    }

    private void Update()
    {
        if(LogsInside > 0)
        {
            logs.SetActive(true);
        }
        else
        {
            logs.SetActive(false);
        }

        if (isBurning && LogsInside > 0)
        {
            FireFX.SetActive(true);
            if (!logBurnt)
            {
                logBurnt = true;
                StartCoroutine(Burn());
                
            }
        }
        else
        {
            FireFX.SetActive(false);
            isBurning = false;
        }

    }

    public void setOnFire()
    {
        if(!isBurning && LogsInside > 0)
        {
            isBurning = true;
        }
    }

    private IEnumerator Burn()
    {
        yield return new WaitForSeconds(burnDuration);
        logBurnt = false;
        LogsInside--;
    }

}
