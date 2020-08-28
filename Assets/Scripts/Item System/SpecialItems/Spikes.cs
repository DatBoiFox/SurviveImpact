using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public float Damage;
    // Damage over time (tick rate, apply damage every selected tick)
    public float damageTimer;
    private float damageTimerTemp;

    // List of items that touches spikes and can be destroyed
    [SerializeField]
    private List<GameObject> destroyablesInside;



    private void Awake()
    {
        destroyablesInside = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDestroyable>() != null)
        {
            destroyablesInside.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IDestroyable>() != null)
        {
            destroyablesInside.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        if (destroyablesInside.Count <= 0) {
            damageTimerTemp = damageTimer;
        }
        else
        {
            damageTimerTemp += Time.deltaTime;
        }

        ApplyDamage();


    }

    private void ApplyDamage()
    {
        if(damageTimerTemp >= damageTimer)
        {
            foreach(GameObject obj in destroyablesInside)
            {
                if(obj != null)
                    obj.GetComponent<IDestroyable>().ApplyDamage(Damage);
                else
                {
                    destroyablesInside.Remove(obj);
                }
            }
            damageTimerTemp = 0;
        }
    }
}