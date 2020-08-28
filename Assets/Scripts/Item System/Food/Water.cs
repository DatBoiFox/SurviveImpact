using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Food
{

    public ParticleSystem particleSystem;
    [SerializeField]
    private float liquidLeft;

    public float flowRate;
    private PlayerObject player;
    private void Awake()
    {
        liquidLeft = foodInfo.LiquidAmount;
        //ItemID = this.GetType().Name.GetHashCode();
    }

    private void Start()
    {
        FindObjectOfType<SaveManager>().SaveItem(Application.loadedLevelName, this.gameObject);
    }

    private void Update()
    {
        float angle = Vector3.Angle(this.transform.up, Vector3.up);



        if (angle >= 95 && !particleSystem.isPlaying && liquidLeft > 0)
        {
            particleSystem.Play();
            if(player != null)
                player.GetStatusSystem().ApplyHydration(-flowRate);
            liquidLeft -= flowRate;
        }
        else if (!particleSystem.isPaused)
        {
            particleSystem.Stop();
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            if (liquidLeft > 0)
            {
                player = other.transform.root.GetComponent<PlayerObject>();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        player = null;
    }

}
