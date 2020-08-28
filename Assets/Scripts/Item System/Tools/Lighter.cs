using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighter : Tool
{
    public Animator animator;

    // Effect object
    public GameObject FireFX;

    public AudioSource audioSource;

    private bool open = false;
    private bool closed = true;
    public override void DoToolStuff()
    {
        if (closed)
        {
            animator.SetTrigger("Open");
            closed = false;
            open = true;
            audioSource.PlayOneShot(audioSource.clip);
        }
        else if (open)
        {
            animator.SetTrigger("Close");
            open = false;
            closed = true;
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    private void Update()
    {
        if (open)
        {
            FireFX.SetActive(true);
        }
        else
        {
            FireFX.SetActive(false);
        }
    }
    private void Start()
    {
        FindObjectOfType<SaveManager>().SaveItem(Application.loadedLevelName, this.gameObject);
    }

    public override void PickUp(GameObject hand)
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.gameObject.layer = 8;
        this.transform.parent = hand.transform;
        this.transform.localRotation = Quaternion.Euler(1.071f, 92.53201f, 27.511f);
    }
    public override void Drop(GameObject hand)
    {
        this.transform.parent = null;
        this.gameObject.layer = 0;
        //this.transform.localScale = Vector3.one;
        this.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FirePit>())
        {
            if (open)
            {
                other.GetComponent<FirePit>().setOnFire();
            }
        }
    }
}
